using CK.Core;
using CK.Setup;
using CK.TypeScript.Engine;
using System;
using System.Runtime.CompilerServices;

namespace CK.TS.Angular.Engine;

/// <summary>
/// Implements <see cref="NgLocalesImportAttribute"/>.
/// </summary>
public sealed class NgLocalesImportAttributeImpl : TypeScriptGroupOrPackageAttributeImplExtension
{
    /// <summary>
    /// Initializes a new implementation.
    /// </summary>
    /// <param name="attr">The attribute.</param>
    /// <param name="type">The decorated type.</param>
    public NgLocalesImportAttributeImpl( NgLocalesImportAttribute attr, Type type )
        : base( attr, type )
    {
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    public new NgLocalesImportAttribute Attribute => Unsafe.As<NgLocalesImportAttribute>( base.Attribute );

    protected override bool OnResPackageAvailable( IActivityMonitor monitor,
                                                   TypeScriptContext context,
                                                   TypeScriptGroupOrPackageAttributeImpl tsPackage,
                                                   ResSpaceData spaceData,
                                                   ResPackage package )
    {
        context.GetAngularCodeGen().Locales.AddImport( package.Index, Attribute.SymbolNames, Attribute.From );
        return true;
    }
}
