using CK.Core;
using CK.TS.Angular;
using CK.TypeScript;

namespace CK.Ng.Zorro;

[TypeScriptPackage]
[TypeScriptImportLibrary( "ng-zorro-antd", "^22", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "luxon", "^3", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@types/luxon", "^3", DependencyKind.DevDependency )]
[TypeScriptImportLibrary( "@angular/cdk", "^22", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/angular-fontawesome", "^5", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/fontawesome-svg-core", "^7", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/free-brands-svg-icons", "^7", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/free-regular-svg-icons", "^7", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/free-solid-svg-icons", "^7", DependencyKind.Dependency )]

[NgProviderImport( "provideNzI18n", From = "ng-zorro-antd/i18n" )]
[NgProviderImport( "DEFAULT_LOCALE_INFO", From = "@local/ck-gen/ts-locales/locales" )]
[NgProvider( "provideNzI18n( DEFAULT_LOCALE_INFO.zorro )" )]

// https://github.com/NG-ZORRO/ng-zorro-antd/tree/master/components/i18n/languages
[NgLocales( "zorro", "NzI18nInterface",
            CultureValues = "de:de_DE;en:en_US;en-gb:en_GB;es:es_ES;fr:fr_FR;fr-be:fr_BE;fr-ca:fr_CA;it:it_IT;nl:nl_NL;nl-be:nl_BE;pt:pt_PT;uk:uk_UA;zh:zh_CN;zh-hk:zh_HK;zh-tw:zh_TW",
            DefaultValue = "en_US",
            ValueImportFrom = "ng-zorro-antd/i18n",
            ServiceMember = "readonly #zorro = inject( NzI18nService );",
            SetLocaleCode = "this.#zorro.setLocale( config.zorro );" )]
[NgLocalesImport( "NzI18nInterface, NzI18nService", From = "ng-zorro-antd/i18n" )]

[RegisterTypeScriptType( typeof( SimpleUserMessage ) )]
[RegisterTypeScriptType( typeof( UserMessageLevel ) )]
[TypeScriptFile( "date-helper.ts", "utcDateToLocal" )]
[TypeScriptFile( "datetime.pipe.ts", "DateFormatPipe" )]
[TypeScriptFile( "notification.service.ts", "NotificationService" )]
[TypeScriptFile( "responsive.directive.ts", "ResponsiveDirective" )]

[AppStyleImport( "ng-zorro-antd/ng-zorro-antd.less" )]
[Requires<CK.Ng.Localization.LocalizationPackage>]
public class ZorroPackage : TypeScriptPackage
{
}
