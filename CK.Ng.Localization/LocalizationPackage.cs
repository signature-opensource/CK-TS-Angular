using CK.TS.Angular;
using CK.TypeScript;

namespace CK.Ng.Localization;

/// <summary>
/// Imports the "@ngx-translate/core" library, adds automatic translations loader
/// based on <c>ck-gen/ts-locales/locales.ts</c>.
/// </summary>
[TypeScriptPackage]
[TypeScriptImportLibrary( "@ngx-translate/core", "^18", DependencyKind.Dependency )]

[TypeScriptFile( "TranslationsLoader.ts", "CKTranslationsLoader" )]
[NgProviderImport( "provideTranslateService, provideTranslateLoader", From = "@ngx-translate/core" )]
[NgProviderImport( "CKTranslationsLoader", From = "@local/ck-gen" )]
[NgProviderImport( "DEFAULT_LOCALE_INFO", From = "@local/ck-gen/ts-locales/locales" )]
[NgProvider( "provideTranslateService( { fallbackLang: DEFAULT_LOCALE_INFO.ngxTranslate, lang: DEFAULT_LOCALE_INFO.ngxTranslate, loader: provideTranslateLoader( CKTranslationsLoader ) })" )]

[NgLocales( "ngxTranslate", "string",
            ValuePattern = "'{0}'",
            ServiceMember = "readonly #translate = inject( TranslateService );",
            ConstructorCode = "this.#translate.addLangs( supported );\nthis.#translate.setFallbackLang( '{DefaultCulture}' );",
            // ngx-translate switches its current language once the translations of the language are loaded:
            // setLocale resolves then.
            SetLocaleCode = "await firstValueFrom( this.#translate.use( config.ngxTranslate ) );" )]
[NgLocalesImport( "TranslateService", From = "@ngx-translate/core" )]
[NgLocalesImport( "firstValueFrom", From = "rxjs" )]
public class LocalizationPackage : TypeScriptPackage
{
}
