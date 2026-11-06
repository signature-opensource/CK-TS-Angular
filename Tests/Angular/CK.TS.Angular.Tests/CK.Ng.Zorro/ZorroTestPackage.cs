using CK.TS.Angular;
using CK.TypeScript;

namespace CK.Ng.Zorro;

/// <summary>
/// Minimal implementation of the real CK.Ng.Zorro.ZorroPackage. 
/// </summary>
[TypeScriptPackage]
[TypeScriptImportLibrary( "ng-zorro-antd", "^22", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@angular/cdk", "^22", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/angular-fontawesome", "^5", DependencyKind.Dependency )]
[TypeScriptImportLibrary( "@fortawesome/fontawesome-svg-core", "^7", DependencyKind.Dependency )]
[YarnPackageExtension( "ng-zorro-antd@*", DependencyKind.PeerDependency, "rxjs", "*",
                       Reason = "ng-zorro-antd uses rxjs (through @angular/cdk and @ant-design/icons-angular) but doesn't declare it: without the peer, its CDK is a second instance." )]
[AppStyleImport( "ng-zorro-antd/ng-zorro-antd.less" )]
[NgLocales( "zorro", "NzI18nInterface",
            CultureValues = "de:de_DE;en:en_US;en-gb:en_GB;es:es_ES;fr:fr_FR;fr-be:fr_BE;fr-ca:fr_CA;it:it_IT;nl:nl_NL;nl-be:nl_BE;pt:pt_PT;uk:uk_UA;zh:zh_CN;zh-hk:zh_HK;zh-tw:zh_TW",
            DefaultValue = "en_US",
            ValueImportFrom = "ng-zorro-antd/i18n",
            ServiceMember = "readonly #zorro = inject( NzI18nService );",
            SetLocaleCode = "this.#zorro.setLocale( config.zorro );" )]
[NgLocalesImport( "NzI18nInterface, NzI18nService", From = "ng-zorro-antd/i18n" )]
public class ZorroTestPackage : TypeScriptPackage
{
}
