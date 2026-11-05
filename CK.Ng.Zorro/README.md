# CK.Ng.Zorro

This package wraps [ng-zorro-antd](https://ng.ant.design/) and provides shared services, styles, and utilities designed to be used across all angular projects.

## Dependencies

Depends on:

- [CK.TS.Angular](../CK.TS.Angular/README.md)
- [CK.Ng.Localization](../CK.Ng.Localization/README.md)

## Features

- Preconfigured `ng-zorro-antd` package
- Shared services and utilities
- Global styles and themes
- ngx-translate support, with ts-locales loading, through the required CK.Ng.Localization package

## Installation

Just install CK.Ng.Zorro package in your dotnet project, and voila.

After running CK-Build :

- An `@import` of `ng-zorro-antd/ng-zorro-antd.less` appears in the application's `src/styles.less` global stylesheet
  (`[AppStyleImport( "ng-zorro-antd/ng-zorro-antd.less" )]`).
- A `provideNzI18n( DEFAULT_LOCALE_INFO.zorro )` provider is added to `CKGenAppModule.Providers`: the library's components
  use the ng-zorro-antd locale of the default culture of the generated `ck-gen/ts-locales` (cf. <https://ng.ant.design/docs/i18n/en>).
- The ngx-translate service is provided by [CK.Ng.Localization](../CK.Ng.Localization/README.md), with the same default
  culture. No language is hard-coded.
