# CK.TS.Angular.Engine

The packages built on [CK.TS.Angular](../CK.TS.Angular/README.md) describe Angular components, modules, routes and
providers in C#. This engine turns them into Angular code in the `ck-gen` folder of the target application, and wires
this code into the application: it creates the Angular application when there is none, keeps its `angular.json` and its
Jest setup in line with the configuration, and upgrades an application that is on an older Angular major to the major of
the engine (Angular 22, the major of `AngularCodeGeneratorImpl.DefaultAngularCliVersion`, `"^22"`).

A package doesn't need to reference this engine: CK.TS.Angular declares it as its engine
(`[assembly: CK.Setup.RequiredEngine( "CK.TS.Angular.Engine" )]`), and its `CKGenAppModule` type triggers the
[`AngularCodeGeneratorImpl`](AngularCodeGeneratorImpl.cs).

## The configuration

An Angular application is always integrated in `Inline` mode, with its tests: the
`TypeScriptBinPathAspectConfiguration.IntegrationMode` must be `Inline` and `TypeScriptBinPathAspectConfiguration.AutoInstallJest`
must be true, otherwise the run fails.

## The Angular application

When the target project has no `angular.json`, the engine creates the application. It installs `@angular/cli` (the
`@angular/cli` entry of the `LibraryVersions` configuration, `^22` by default) in a temporary `_ckNew_` folder and runs:

```
ng new <name> --style=less --package-manager=yarn --interactive=false --standalone=true --skip-install=true --skip-git=true --zoneless=true
```

Then it adapts the new application before moving it into the target project:

- karma and jasmine are removed from the `package.json`; its `test` script is `jest`;
- `src/app/app.ts` imports `CKGenAppModule`, `src/app/app.config.ts` adds `...CKGenAppModule.Providers` to the
  providers, `src/app/app.routes.ts` adds `...CKGenRoutes` to the routes, `src/app/app.html` is a single
  `<router-outlet />`, and `src/app/app.spec.ts` creates the `App` with the `appConfig.providers`;
- the `angular.json` loses its `test` target and its production budgets' `maximumError` is `1GB`.

A file that already exists in the target project is not overwritten: the new one goes to a `_ckConflict_` folder (ignored
by git), and a warning says so. The `.gitignore` of the new application is appended to an existing one.

On every run, after the integration, the engine updates the `angular.json`:

- the Angular CLI analytics are disabled (`"cli": { "analytics": false }`), replacing any analytics identifier;
- the `ck-gen/ts-assets` folder is the first entry of the build `assets`;
- for a client application served by its host (a configuration with a `RequestPath`, see
  [CK.TypeScript](https://github.com/signature-opensource/CK-TypeScript/blob/stable/CK.TypeScript/README.md#client-applications)),
  the `baseHref` is the request path (with a trailing `/`) and the port of the `serve` target is the `DevServerPort`.

Before the integration, the engine adds the dependencies the Angular tests need (`@angular/platform-browser`,
`jest-preset-angular`) and `less` (a workaround of an Angular CLI issue), and sets the Jest setup to the
[`AngularJestSetupHandler`](AngularJestSetupHandler.cs): a Jest configuration with the `jest-preset-angular` preset
in a `jsdom` environment (30 seconds test timeout), and a setup file for a zoneless application, or with zone.js when
the application depends on `zone.js`. Both files start with a `//-AllowAutoUpdate` line: remove it to keep the engine
from updating the file. In `tsconfig.spec.json`, the `jasmine` types are replaced by `jest` and `node`.

## The generated code

| File | Content |
|------|---------|
| `ck-gen/CK/Angular/CKGenAppModule.ts` | The `CKGenAppModule` NgModule, that imports and exports the `[NgModule]` modules, and its static `CKGenAppModule.Providers`. |
| `ck-gen/CK/Angular/routes.ts` | The routes of the components routed to the `AppComponent`. |
| `routes.ts` of a component with routes | The child routes of a component whose `NgComponentAttribute.HasRoutes` is true, next to its file. |
| `ck-gen/CK/Angular/NamedComponentsResolver.ts` | `resolveNamedComponentTypeAsync( name )`: loads the component that implements `INgNamedComponent` under this name. |

### The providers

`CKGenAppModule.Providers` is a `SourcedProviders`: an array of providers where each one has the `source` that
registered it. The first one is the `LocaleProvider` of the generated `ts-locales` (source `"CK.TS.Angular"`). Each
`[NgProvider]` adds its `NgProviderAttribute.ProviderCode`, with the full C# name of the decorated package as its source,
followed by the `NgProviderAttribute.SourceNameSuffix` when there is one. An application that must configure a provider
differently removes it with `SourcedProviders.exclude( sourceName )` (it throws when no provider has this source) and
provides its own.

The symbols that the provider code uses are imported in `CKGenAppModule.ts` by the `[NgProviderImport]` attributes:
from `@local/ck-gen` by default, otherwise from the `NgProviderImportAttribute.From` library (a sub path like
`ng-zorro-antd/i18n` is supported), which is registered as a dev dependency when no package declares it.

### The routes

A routed component (`[NgRoutedComponent]`) appears in the routes of its `NgRoutedComponentAttribute.TargetComponent`
under its `NgRoutedComponentAttribute.Route`, by default its component name (`AppRoutedComponent` is `app-routed`). Its
route is `component: ...` by default, or `loadComponent: () => import( ... )` when its
`NgRoutedComponentAttribute.RegistrationMode` is `RouteRegistrationMode.Lazy`. A target can be an abstraction (like
`INgPrivatePageComponent`): it is resolved to its implementation.

When the application has at least one route, `ck-gen/CK/Angular/routes.ts` ends with a `'**'` route that redirects an
unknown URL to `/`, with the unknown URL in the `notFound` query parameter.

### The components and the modules

The [`NgComponentAttributeImpl`](NgComponentAttributeImpl.cs) checks that a component is sealed and that its type name
ends with "Component". Its component name is the kebab-case name without this suffix (`PublicPageComponent` is
`public-page`): it is the folder of the component and the name of its `.ts` file, which must be in its resources and
export the type name without the suffix (`PublicPage`). A component that implements an interface decorated with
`[NgSingleAbstractComponent]` must have the `NgSingleAbstractComponentAttribute.ComponentName` of this interface.

The [`NgModuleAttributeImpl`](NgModuleAttributeImpl.cs) does the same for a module: `DemoNgModule` is the
`demo-ng/demo-ng.module.ts` file that exports `DemoNgModule`.

## The extension point

`TypeScriptContextExtensions.GetAngularContext` gives the [`IAngularContext`](IAngularContext.cs) of a
`TypeScriptContext` to another engine: the import section of `CKGenAppModule.ts` (`IAngularContext.CKGenAppModuleImports`),
the `@angular/core` and `@angular/router` libraries, and the code part of the providers (`IAngularContext.ProviderPart`).

## The upgrade to the Angular major of the engine

After the code generation, an application whose `@angular/core` is on an older major than the engine's is upgraded,
one major at a time.

**Nothing is written unless the safety check passes** ([`AngularUpgradeSafety`](AngularUpgradeSafety.cs)). The upgrade
is refused, with a message that lists every issue and the next actions, when:

1. a crossed major has no upgrade tooling (an [`AngularMajorTooling`](AngularMajorTooling.cs) row: Angular 21 and 22,
   so the application must be on Angular 20 or above);
2. the Node.js version is unknown or outside the range of a crossed major;
3. the application folder is not under git, or has uncommitted changes (`ck-gen`, `.yarn` and `.yarnrc.yml` excepted);
4. a CK package (or the `LibraryVersions` configuration) declares an `@angular/*` or `ng-zorro-antd` version that doesn't
   accept the target major;
5. the `@angular/cli` entry of `LibraryVersions` pins another major;
6. more than two majors must be crossed while CK packages declare libraries outside Angular's numbering above the
   application's version;
7. the build runs on CI (detected as Yarn does: [`ContinuousIntegrationDetector`](ContinuousIntegrationDetector.cs)).

When the manual upgrade is an alternative, the message gives its PowerShell commands.

**Each step** ([`AngularUpgradePlanner`](AngularUpgradePlanner.cs)) sets the pre-update versions of the row
(`jest-preset-angular`, `zone.js`) when the application has them, runs `yarn install`, then
`ng update @angular/core@N @angular/cli@N ... --allow-dirty` (with `ng-zorro-antd` and `@angular/cdk` when the application
has them and, on the last step, the libraries that CK packages declare with a higher major), `yarn dedupe`, `yarn install`,
and the migrations of each package (`ng update <package> --migrate-only --from <installed version> --to N`). A row can
replace a package after the migrations (Angular 22 replaces `@angular/platform-browser-dynamic` by
`@angular/platform-browser`). Every yarn command times out after 10 minutes.

**A failure is undone.** Before its first write, the upgrade writes a `.ck-angular-upgrade` marker at the application
root. A failed step, or an integration that fails after the upgrade, moves the changes of the application folder into a
git stash named `ck-angular-upgrade undo <folder> <UTC time>` and runs `yarn install` on the restored files; the message
says what failed, the state of the folder and the next actions. A marker found at the start of a run means that a
previous upgrade was not verified: it is undone first.

**After the integration**, the upgraded application is compiled (`yarn ngc -p tsconfig.app.json --noEmit`), the marker
is deleted, and a summary is logged as a warning: the result of the compile check, the changed files, the optional
migrations that were not run, and the commands that undo the upgrade. Compile errors are reported with the hints of the crossed rows (like the ng-zorro-antd 22
renamings) and a link to the Angular update guide.

The parts of the upgrade are tested one by one in [CK.TS.Angular.Tests](../Tests/Angular/CK.TS.Angular.Tests)
(`AngularUpgradePlannerTests`, `AngularUpgradeSafetyTests`, `UpgradeFailureClassifierTests`, ...).

## Requires

- [CK.TS.Angular](../CK.TS.Angular/README.md) for the attributes it implements.
- [CK.TypeScript](https://github.com/signature-opensource/CK-TypeScript/blob/stable/CK.TypeScript/README.md), through
  CK.TypeScript.Engine, for the TypeScript code generation and the integration.
