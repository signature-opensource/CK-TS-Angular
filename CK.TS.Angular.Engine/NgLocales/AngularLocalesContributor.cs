using CK.Core;
using CK.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK.TS.Angular.Engine;

/// <summary>
/// The Angular part of the generated <c>ck-gen/ts-locales/locales.ts</c>: the Angular locale data of the active
/// cultures, the <c>LocaleService</c>, <c>LocaleId</c> and <c>LocaleProvider</c>. The <see cref="NgLocalesAttribute"/>
/// and <see cref="NgLocalesImportAttribute"/> of the packages add their imports, <c>LocaleInfo</c> fields and
/// <c>LocaleService</c> code, in the order of their packages.
/// </summary>
sealed class AngularLocalesContributor : TSLocalesContributor
{
    // https://app.unpkg.com/@angular/common@20.2.3/files/locales
    static readonly Dictionary<string, string> _angularLocaleImportByCultureName = new()
    {
        { "de", "@angular/common/locales/de" },
        { "de-be", "@angular/common/locales/de-BE" },
        { "en-gb", "@angular/common/locales/en-GB" },
        { "es", "@angular/common/locales/es" },
        { "fr", "@angular/common/locales/fr" },
        { "fr-fr", "@angular/common/locales/fr" },
        { "fr-ca", "@angular/common/locales/fr-CA" },
        { "fr-be", "@angular/common/locales/fr-BE" },
        { "it", "@angular/common/locales/it" },
        { "nl", "@angular/common/locales/nl" },
        { "nl-be", "@angular/common/locales/nl-BE" },
        { "pt", "@angular/common/locales/pt" },
        { "uk", "@angular/common/locales/uk" },
        { "zh", "@angular/common/locales/zh" }
    };

    readonly List<(int Order, string SymbolNames, string From)> _imports = new();
    readonly List<(int Order, Feature Feature)> _features = new();
    Feature[]? _orderedFeatures;

    sealed class Feature
    {
        readonly NgLocalesAttribute _attr;
        readonly string _packageName;
        readonly Dictionary<string, string>? _cultureValues;
        Dictionary<NormalizedCultureInfo, string?>? _values;

        public Feature( NgLocalesAttribute attr, string packageName )
        {
            _attr = attr;
            _packageName = packageName;
            if( attr.CultureValues != null )
            {
                _cultureValues = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
                foreach( var pair in attr.CultureValues.Split( ';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries ) )
                {
                    var idx = pair.IndexOf( ':' );
                    if( idx > 0 ) _cultureValues[pair[..idx].Trim()] = pair[(idx + 1)..].Trim();
                }
            }
        }

        public NgLocalesAttribute Attribute => _attr;

        // The value of each culture, computed once (the warnings are emitted once).
        public string? GetValue( IActivityMonitor monitor, NormalizedCultureInfo culture )
        {
            _values ??= new Dictionary<NormalizedCultureInfo, string?>();
            if( !_values.TryGetValue( culture, out var value ) )
            {
                value = ComputeValue( monitor, culture );
                _values.Add( culture, value );
            }
            return value;
        }

        string? ComputeValue( IActivityMonitor monitor, NormalizedCultureInfo culture )
        {
            if( _cultureValues == null )
            {
                if( _attr.ValuePattern == null )
                {
                    monitor.Error( $"[NgLocales( \"{_attr.FieldName}\" )] of '{_packageName}' must specify its ValuePattern or its CultureValues." );
                    return null;
                }
                return string.Format( _attr.ValuePattern, culture.Name );
            }
            if( _cultureValues.TryGetValue( culture.Name, out var v ) ) return v;
            var dash = culture.Name.IndexOf( '-' );
            if( dash > 0 && _cultureValues.TryGetValue( culture.Name[..dash], out v ) ) return v;
            if( _attr.DefaultValue == null )
            {
                monitor.Error( $"No '{_attr.FieldName}' value for culture '{culture.Name}' in [NgLocales] of '{_packageName}' (and no DefaultValue)." );
                return null;
            }
            monitor.Warn( $"No '{_attr.FieldName}' value for culture '{culture.Name}' in [NgLocales] of '{_packageName}': the default '{_attr.DefaultValue}' is used." );
            return _attr.DefaultValue;
        }
    }

    /// <summary>
    /// Called by NgLocalesImportAttributeImpl.
    /// </summary>
    internal void AddImport( int order, string symbolNames, string from ) => _imports.Add( (order, symbolNames, from) );

    /// <summary>
    /// Called by NgLocalesAttributeImpl.
    /// </summary>
    internal void AddFeature( int order, NgLocalesAttribute attr, string packageName ) => _features.Add( (order, new Feature( attr, packageName )) );

    // OrderBy is stable: the registration order is kept for the same package.
    Feature[] Features => _orderedFeatures ??= _features.OrderBy( f => f.Order ).Select( f => f.Feature ).ToArray();

    public override void AddImports( IActivityMonitor monitor, ActiveCultureSet activeCultures, List<string> imports )
    {
        imports.Add( "import { inject, Injectable, LOCALE_ID, Provider, signal } from '@angular/core';" );
        // The identifiers used as values are added to the import from their module (or to a new one).
        var valueSymbols = new Dictionary<string, List<string>>();
        foreach( var f in Features )
        {
            var from = f.Attribute.ValueImportFrom;
            if( from == null ) continue;
            if( !valueSymbols.TryGetValue( from, out var symbols ) )
            {
                valueSymbols.Add( from, symbols = new List<string>() );
            }
            foreach( var c in activeCultures.AllActiveCultures )
            {
                var v = f.GetValue( monitor, c.Culture );
                if( v != null && !symbols.Contains( v ) ) symbols.Add( v );
            }
        }
        foreach( var (_, symbolNames, from) in _imports.OrderBy( i => i.Order ) )
        {
            var names = symbolNames;
            if( valueSymbols.Remove( from, out var symbols ) )
            {
                var declared = symbolNames.Split( ',', StringSplitOptions.TrimEntries );
                foreach( var s in symbols )
                {
                    if( !declared.Contains( s ) ) names += ", " + s;
                }
            }
            imports.Add( $"import {{ {names} }} from '{from}';" );
        }
        foreach( var (from, symbols) in valueSymbols )
        {
            if( symbols.Count > 0 ) imports.Add( $"import {{ {string.Join( ", ", symbols )} }} from '{from}';" );
        }
    }

    public override bool WritePrelude( IActivityMonitor monitor,
                                       ActiveCultureSet activeCultures,
                                       NormalizedCultureInfo defaultCulture,
                                       StringBuilder code )
    {
        // The Angular locale data of the active cultures ("en" is built in).
        if( activeCultures.AllActiveCultures.Any( c => c.Culture.Name.ToLowerInvariant() != "en" && c.Culture.Name.ToLowerInvariant() != "en-us" ) )
        {
            code.AppendLine( "import { registerLocaleData } from '@angular/common';" );
        }
        var importSB = new StringBuilder();
        var registerSB = new StringBuilder();
        foreach( var c in activeCultures.AllActiveCultures )
        {
            if( c.Culture.Name.ToLowerInvariant() != "en" && c.Culture.Name.ToLowerInvariant() != "en-us" )
            {
                if( _angularLocaleImportByCultureName.TryGetValue( c.Culture.Name, out var localeImportPath ) )
                {
                    if( importSB.ToString().Contains( localeImportPath ) ) continue;

                    var importName = $"locale{c.Culture.Name.Replace( "-", "" ).ToUpperInvariant()}";
                    importSB.AppendLine( $"import {importName} from '{localeImportPath}';" );
                    registerSB.AppendLine( $"registerLocaleData( {importName} );" );
                }
            }
        }
        code.Append( importSB.ToString() ).AppendLine();
        code.Append( registerSB.ToString() ).AppendLine();
        return true;
    }

    public override IReadOnlyList<TSLocaleInfoField> LocaleInfoFields
        => Features.Select( f => new TSLocaleInfoField( f.Attribute.FieldName, f.Attribute.FieldType, f.GetValue ) ).ToArray();

    public override bool WriteEpilogue( IActivityMonitor monitor,
                                        ActiveCultureSet activeCultures,
                                        NormalizedCultureInfo defaultCulture,
                                        StringBuilder code )
    {
        var defaultName = defaultCulture.Name;
        code.AppendLine( "@Injectable( { providedIn: 'root' } )" )
            .AppendLine( "export class LocaleService {" )
            .Append( "  " );
        bool first = true;
        foreach( var f in Features )
        {
            if( f.Attribute.ServiceMember == null ) continue;
            if( !first ) code.Append( "  " );
            code.AppendLine( f.Attribute.ServiceMember );
            first = false;
        }
        code.AppendLine()
            .AppendLine( $"  currentLocale = signal<string>( '{defaultName}' );" )
            .AppendLine();

        code.AppendLine( "  constructor() {" )
            .Append( "    const supported = Object.keys( locales );" );
        foreach( var line in Lines( Features.Select( f => f.Attribute.ConstructorCode ) ) )
        {
            code.AppendLine().Append( "    " ).Append( line.Replace( "{DefaultCulture}", defaultName ) );
        }
        code.AppendLine()
            .AppendLine( $"    this.setLocale( '{defaultName}' );" )
            .AppendLine( "  }" );

        code.AppendLine()
            .AppendLine( "  async setLocale( locale: string ): Promise<void> {" )
            .AppendLine( "    const config = locales[locale];" )
            .AppendLine( "    if ( !config ) throw new Error( `Unsupported locale: ${locale}` );" )
            .AppendLine( "    " )
            .AppendLine( "    this.currentLocale.set( locale );" );
        foreach( var line in Lines( Features.Select( f => f.Attribute.SetLocaleCode ) ) )
        {
            code.Append( "    " ).AppendLine( line );
        }
        code.AppendLine( "  }" )
            .AppendLine( "}" )
            .AppendLine()
            .AppendLine( "export class LocaleId extends String {" )
            .AppendLine( "  readonly #localeService = inject( LocaleService );" )
            .AppendLine( "  constructor() {" )
            .AppendLine( "    super();" )
            .AppendLine( "  }" )
            .AppendLine()
            .AppendLine( "  override toString(): string {" )
            .AppendLine( "    return this.#localeService.currentLocale();" )
            .AppendLine( "  }" )
            .AppendLine()
            .AppendLine( "  override valueOf(): string {" )
            .AppendLine( "    return this.toString();" )
            .AppendLine( "  }" )
            .AppendLine( "}" )
            .AppendLine()
            .AppendLine( "export const LocaleProvider: Provider = {" )
            .AppendLine( "  provide: LOCALE_ID," )
            .AppendLine( "  useClass: LocaleId," )
            .AppendLine( "  deps: [LocaleService]," )
            .Append( "};" );
        return true;

        static IEnumerable<string> Lines( IEnumerable<string?> codes ) => codes.Where( c => c != null )
                                                                              .SelectMany( c => c!.Split( '\n' ) )
                                                                              .Select( l => l.TrimEnd( '\r' ) );
    }
}
