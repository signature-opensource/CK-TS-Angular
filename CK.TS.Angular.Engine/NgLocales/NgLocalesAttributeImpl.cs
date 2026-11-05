using CK.Core;
using CK.Setup;
using CK.TypeScript.Engine;
using System;
using System.Runtime.CompilerServices;

namespace CK.TS.Angular.Engine;

/// <summary>
/// Implements <see cref="NgLocalesAttribute"/>.
/// </summary>
public sealed class NgLocalesAttributeImpl : TypeScriptGroupOrPackageAttributeImplExtension
{
    /// <summary>
    /// Initializes a new implementation.
    /// </summary>
    /// <param name="attr">The attribute.</param>
    /// <param name="type">The decorated type.</param>
    public NgLocalesAttributeImpl( NgLocalesAttribute attr, Type type )
        : base( attr, type )
    {
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    public new NgLocalesAttribute Attribute => Unsafe.As<NgLocalesAttribute>( base.Attribute );

    protected override bool OnResPackageAvailable( IActivityMonitor monitor,
                                                   TypeScriptContext context,
                                                   TypeScriptGroupOrPackageAttributeImpl tsPackage,
                                                   ResSpaceData spaceData,
                                                   ResPackage package )
    {
        context.GetAngularCodeGen().Locales.AddFeature( package.Index, Attribute, Type.ToCSharpName() );
        return true;
    }
}
