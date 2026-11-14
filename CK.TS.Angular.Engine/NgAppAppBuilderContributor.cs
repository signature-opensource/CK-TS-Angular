using CK.Setup;

[assembly: AppBuilderContributor( typeof( CK.TS.Angular.Engine.NgAppAppBuilderContributor ) )]

namespace CK.TS.Angular.Engine;

/// <summary>
/// Configures the TypeScript aspect from the clients declared by the parties of the Apps ("ckonf.json") that have the
/// "NgApp" client feature: see <see cref="TypeScriptFrameworkAppBuilderContributor"/>.
/// </summary>
public sealed class NgAppAppBuilderContributor : TypeScriptFrameworkAppBuilderContributor
{
    /// <summary>
    /// The name of the client feature: "NgApp".
    /// </summary>
    public const string NgAppFeatureName = "NgApp";

    /// <summary>
    /// Initializes a new contributor.
    /// </summary>
    /// <param name="appIdentity">The parties and their declared clients.</param>
    public NgAppAppBuilderContributor( AppIdentityAppBuilderContributor appIdentity )
        : base( appIdentity, NgAppFeatureName )
    {
    }
}
