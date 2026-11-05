# CK.Ng.Localization

The TypeScript packages bring their translations in their `ts-locales` resources, and the engine generates from them the
`ck-gen/ts-locales` of the application: one translation file per active culture. This package makes them the translations
of [ngx-translate](https://github.com/ngx-translate/core) in an Angular application, without any code in the
application.

## The package

[`LocalizationPackage`](LocalizationPackage.cs) is a TypeScript package that:

- declares the `@ngx-translate/core` library (`^18`) as a dependency;
- adds [`Res/TranslationsLoader.ts`](Res/TranslationsLoader.ts) to the generated code: the `CKTranslationsLoader` is a
  `TranslateLoader` whose `getTranslation( lang )` returns the translations that `loadTranslations( lang )` of
  `@local/ck-gen/ts-locales/locales` loads;
- provides ngx-translate at the root of the application, with the default culture of the generated locales as its
  language and its fallback language:

```ts
provideTranslateService( { fallbackLang: DEFAULT_LOCALE_INFO.ngxTranslate, lang: DEFAULT_LOCALE_INFO.ngxTranslate, loader: provideTranslateLoader( CKTranslationsLoader ) })
```

This package contributes to the generated `ck-gen/ts-locales/locales.ts` with `[NgLocales]` (see
[CK.TS.Angular.Engine](../CK.TS.Angular.Engine/README.md#the-locales)): each locale gets its `ngxTranslate` language,
and `LocaleService.setLocale` switches the ngx-translate language too (it resolves once the translations of the
language are loaded).

## Requires

- [CK.TS.Angular](../CK.TS.Angular/README.md) for the providers registration.
