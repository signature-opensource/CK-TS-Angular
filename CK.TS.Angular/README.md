# CK.TS.Angular
Supports Angular (the engine's major is 22) by enabling the @local/ck-gen code to interact with an Angular host application.

The [CK.TS.Angular.Engine](../CK.TS.Angular.Engine/README.md) implements this package: it creates the Angular
application when there is none, sets up its Jest tests, generates the code and upgrades an application on an older
Angular major. Its README gives the details.

## The "NgApp" client feature

A client of a party (CK.AppIdentity.PartyClient) that is an Angular application has the "NgApp" client feature: the
[`NgAppClientFeature`](NgAppClientFeature.cs), created by its `NgAppClientFeatureDriver` from the `ClientDirectory` of the
application (the client must be built by it). It is:

- the TypeScript framework of the client (`ITypeScriptFramework` of CK.TypeScript): its `Info` is the `ClientInfo`;
- its web build (`IWebAppBuild` of CK.AppIdentity.PartyClient): its `ProjectPath`, its `OutputPath` (the Angular builder's
  `dist/{project}/browser`) and its dev server (`ng serve --port {port}` through the yarn of the project), what the
  "Hosting" feature of CK.TS.AspNet.Clients serves;
- the facts of the client in its description (`ClientInfo.Facts`).

Its keys are its compile time model (`[CompileConfiguration<TypeScriptClientCompileConfiguration>]`: the TypeScript
configuration of the client, its `RequestPath` and its `DevServerPort`, see CK.TypeScript.Engine) and its options: the
run time ignores the compile time keys.

```jsonc
"PartyClient": {
  "MiniApp": {
    "Types": [ "CK.Ng.Cris.AspNet.CrisAspNetPackage, CK.Ng.Cris.AspNet" ],
    "NgApp": { "ActiveCultures": [ "en", "fr" ], "DevServerPort": 4201 },
    "Hosting": true
  }
}
```

## Usage
When the target project has no Angular application, the engine creates it with `ng new` and wires the generated code
into it. An existing application must contain the following.

Installing this package generates a `ck-gen/CK/Angular/CKGenAppModule.ts` that:
- must be imported in the application component (in file `src/app/app.ts`):
```typescript
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CKGenAppModule } from '@local/ck-gen/CK/Angular/CKGenAppModule';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CKGenAppModule],
  templateUrl: './app.html',
  styleUrl: './app.less'
})
export class App { }
```
- has a `CKGenAppModule.Providers` static field that must be appended to the "root" injector (in file `src/app/app.config.ts`):
```typescript
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { CKGenAppModule } from '@local/ck-gen/CK/Angular/CKGenAppModule';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), ...CKGenAppModule.Providers],
};
```

A `ck-gen/CK/Angular/routes.ts` is also generated: the routes of the components routed to the `AppComponent`. These routes
must be included in the application routes (in file `src/app/app.routes.ts`):

```typescript
import { Routes } from '@angular/router';
import CKGenRoutes from '@local/ck-gen/CK/Angular/routes';

export const routes: Routes = [
    ...CKGenRoutes,
];
```

## How it works
The `/ck-gen/CK/Angular/CKGenAppModule.ts` file has 2 roles:
- For NgModule (if they are used), this `CKGenAppModule` module acts as a simple re-exporter of code generated modules.
- For Dependency Injection, it enables generated providers to be registered in the host application ("root") provider of services.
  `CKGenAppModule.Providers` is a `SourcedProviders`: each provider has the `source` that registered it (the first one is
  the `LocaleProvider` of the generated `ts-locales`), and `SourcedProviders.exclude( sourceName )` removes the providers
  of a source so that the application can provide them differently.
```typescript
@NgModule({
    imports: [
    // Registered NgModules come here.
    ],
    exports: [
    // Registered NgModules also come here.
    ] })
export class CKGenAppModule {
    static Providers : SourcedProviders = SourcedProviders.createFrom( [
      CKGenAppModule.s( LocaleProvider, "CK.TS.Angular" ),
      // Registered providers come here.
    ] );
}
```

The `/ck-gen/CK/Angular/routes.ts` file supports the routed components. When there is at least one route, it ends
with a `'**'` route that redirects an unknown URL to `/` (with the unknown URL in the `notFound` query parameter).
```typescript
export default [
    // RoutedComponents' routes come here.
] as Route[];
```

See [CK.TS.Angular.Engine](../CK.TS.Angular.Engine/README.md#the-generated-code) for the details of the generated code.


