Brings [ng-zorro-antd](https://ng.ant.design/) to an Angular application: the `ZorroPackage` declares ng-zorro-antd,
`@angular/cdk`, luxon and the Font Awesome libraries, imports the ng-zorro-antd styles in the application, provides
`provideNzI18n` with the default culture of the generated locales, and adds shared utilities: the `NotificationService`
(messages and notifications, with translated default titles), the `isoUtcToLocal` pipe (`DateFormatPipe`), the
`appResponsive` directive (`ResponsiveDirective`), less variables and translations of common buttons.
It requires [CK.Ng.Localization](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.Ng.Localization/README.md).
See the [README](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.Ng.Zorro/README.md).
