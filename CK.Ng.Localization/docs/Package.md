Makes the generated translations (`ck-gen/ts-locales`) the translations of ngx-translate in an Angular application:
the `LocalizationPackage` declares the `@ngx-translate/core` library, adds the `CKTranslationsLoader` (a
`TranslateLoader` on the generated `loadTranslations`) and provides ngx-translate at the root with the default culture
as its language.
See the [README](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.Ng.Localization/README.md).
