Angular support of the TypeScript engine: the generated `@local/ck-gen` code integrated in an Angular application.
Packages describe their Angular parts in C#: components (`NgComponent`, `[NgComponent]`), routed components
(`NgRoutedComponent`, `[NgRoutedComponent<T>]`, under the `AppComponent` or a component with routes), modules
(`NgModule`), providers (`[NgProvider]`, `[NgProviderImport]`) and abstract components (`INgPrivatePageComponent`,
`INgUserInfoBoxComponent`, `[NgSingleAbstractComponent]`). The application uses the generated `CKGenAppModule`, its
`CKGenAppModule.Providers` and the generated routes. The engine is
[CK.TS.Angular.Engine](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.TS.Angular.Engine/README.md).
See the [README](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.TS.Angular/README.md).
