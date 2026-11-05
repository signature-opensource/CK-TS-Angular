using CK.Setup;
using CK.TypeScript;
using System;

namespace CK.TS.Angular;

/// <summary>
/// Imports symbols used by the <see cref="NgLocalesAttribute"/> of a <see cref="TypeScriptPackage"/> in the generated
/// <c>ck-gen/ts-locales/locales.ts</c>: <c>import { <see cref="SymbolNames"/> } from '<see cref="From"/>';</c>.
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
public sealed class NgLocalesImportAttribute : ContextBoundDelegationAttribute
{
    /// <summary>
    /// Initializes a new <see cref="NgLocalesImportAttribute"/>.
    /// </summary>
    /// <param name="symbolNames">The comma-separated type or variable names to import.</param>
    public NgLocalesImportAttribute( string symbolNames )
        : base( "CK.TS.Angular.Engine.NgLocalesImportAttributeImpl, CK.TS.Angular.Engine" )
    {
        SymbolNames = symbolNames;
        From = "@local/ck-gen";
    }

    /// <summary>
    /// Gets the comma-separated type or variable names to import.
    /// </summary>
    public string SymbolNames { get; }

    /// <summary>
    /// Gets or sets the module name (like "rxjs"). Defaults to "@local/ck-gen".
    /// </summary>
    public string From { get; set; }
}
