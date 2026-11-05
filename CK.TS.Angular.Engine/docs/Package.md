Engine of [CK.TS.Angular](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.TS.Angular/README.md):
the `AngularCodeGeneratorImpl` generates the Angular code of the packages in `ck-gen` (`CKGenAppModule` and its
`Providers`, the routes, the named components resolver), creates the Angular application when there is none, keeps its
`angular.json` and its Jest setup in line with the configuration, and upgrades an application on an older Angular major
to the engine's major (Angular 22), with a safety check before and an undo on failure. Other engines reach its
extension points through `IAngularContext` (`TypeScriptContextExtensions.GetAngularContext`).
See the [README](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.TS.Angular.Engine/README.md).
