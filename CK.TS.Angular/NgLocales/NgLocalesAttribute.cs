using CK.Setup;
using CK.TypeScript;
using System;

namespace CK.TS.Angular;

/// <summary>
/// Contributes a feature of a <see cref="TypeScriptPackage"/> to the generated <c>ck-gen/ts-locales/locales.ts</c> of an
/// Angular application: a field of <c>LocaleInfo</c> with its value for each active culture, and the code that applies
/// the current locale in the <c>LocaleService</c> (a member, constructor lines, <c>setLocale</c> lines).
/// <para>
/// The symbols that this code uses are imported with <see cref="NgLocalesImportAttribute"/>.
/// The contributions are written in the order of their packages.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class NgLocalesAttribute : ContextBoundDelegationAttribute
{
    /// <summary>
    /// Initializes a new <see cref="NgLocalesAttribute"/>.
    /// </summary>
    /// <param name="fieldName">The name of the <c>LocaleInfo</c> field.</param>
    /// <param name="fieldType">The TypeScript type of the <c>LocaleInfo</c> field.</param>
    public NgLocalesAttribute( string fieldName, string fieldType )
        : base( "CK.TS.Angular.Engine.NgLocalesAttributeImpl, CK.TS.Angular.Engine" )
    {
        FieldName = fieldName;
        FieldType = fieldType;
    }

    /// <summary>
    /// Gets the name of the <c>LocaleInfo</c> field.
    /// </summary>
    public string FieldName { get; }

    /// <summary>
    /// Gets the TypeScript type of the <c>LocaleInfo</c> field.
    /// </summary>
    public string FieldType { get; }

    /// <summary>
    /// Gets or sets the TypeScript expression of the field's value for a culture, where <c>{0}</c> is the culture name
    /// (<c>"'{0}'"</c> is the culture name as a string). Ignored when <see cref="CultureValues"/> is set.
    /// </summary>
    public string? ValuePattern { get; set; }

    /// <summary>
    /// Gets or sets the field's value of the cultures, as <c>"culture:value"</c> pairs separated by <c>;</c>
    /// (<c>"en:en_US;fr:fr_FR"</c>). A culture without value takes the value of its root culture (<c>fr</c> for
    /// <c>fr-ca</c>), then the <see cref="DefaultValue"/> with a warning (an error without default value).
    /// </summary>
    public string? CultureValues { get; set; }

    /// <summary>
    /// Gets or sets the value of a culture that <see cref="CultureValues"/> doesn't cover.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the module that exports the values when they are identifiers (<see cref="CultureValues"/>):
    /// the values used by the active cultures are added to the import from this module.
    /// </summary>
    public string? ValueImportFrom { get; set; }

    /// <summary>
    /// Gets or sets a member of the <c>LocaleService</c> (like <c>"readonly #translate = inject( TranslateService );"</c>).
    /// </summary>
    public string? ServiceMember { get; set; }

    /// <summary>
    /// Gets or sets the lines (separated by <c>\n</c>) added to the constructor of the <c>LocaleService</c>, after
    /// <c>const supported = Object.keys( locales );</c>. <c>{DefaultCulture}</c> is the name of the default culture.
    /// </summary>
    public string? ConstructorCode { get; set; }

    /// <summary>
    /// Gets or sets the lines (separated by <c>\n</c>) added to <c>setLocale( locale: string )</c> of the
    /// <c>LocaleService</c>, after <c>this.currentLocale.set( locale );</c>: <c>config</c> is the <c>LocaleInfo</c> of
    /// the locale.
    /// </summary>
    public string? SetLocaleCode { get; set; }
}
