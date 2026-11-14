using CK.AppIdentity;
using CK.Core;
using CK.TypeScript;
using System.Diagnostics;
using System.Linq;

namespace CK.TS.Angular;

/// <summary>
/// The "NgApp" client feature: the client is an Angular application, the TypeScript framework of the client
/// (<see cref="ITypeScriptFramework"/>) and its web build (<see cref="IWebAppBuild"/>: the Angular builder's output,
/// "ng serve"). What it is comes from the build (its <see cref="Info"/>): its section has only compile time keys.
/// </summary>
public sealed class NgAppClientFeature : ClientFeature, ITypeScriptFramework, IWebAppBuild
{
    internal NgAppClientFeature( PartyClient client, ImmutableConfigurationSection configuration, ClientInfo info )
        : base( client, configuration )
    {
        Info = info;
    }

    /// <inheritdoc />
    public ClientInfo Info { get; }

    /// <inheritdoc />
    public NormalizedPath ProjectPath => Info.ProjectPath;

    /// <inheritdoc />
    public NormalizedPath OutputPath => Info.OutputPath;

    /// <summary>
    /// Gets "ng serve --port {port}" through the yarn of the project.
    /// </summary>
    /// <param name="yarnPath">The yarn release.</param>
    /// <param name="port">The port of the dev server.</param>
    /// <returns>The process to start.</returns>
    public ProcessStartInfo GetDevServerStart( string yarnPath, int port ) => new ProcessStartInfo( "node", $"\"{yarnPath}\" ng serve --port {port}" );

    /// <summary>
    /// Contributes the facts of the client (<see cref="ClientInfo.Facts"/>, the facts of its TypeScript packages).
    /// </summary>
    /// <param name="builder">The description of the client.</param>
    protected override void ContributeDescription( ClientDescriptionBuilder builder ) => builder.AddFacts( Info.Facts );
}

/// <summary>
/// Creates the "NgApp" feature of the clients that declare it: the client applications of the <see cref="ClientDirectory"/>.
/// </summary>
public sealed class NgAppClientFeatureDriver : ClientFeatureDriver<NgAppClientFeature>
{
    readonly ClientDirectory _directory;

    /// <summary>
    /// Initializes a new driver.
    /// </summary>
    /// <param name="directory">The client applications built by the application.</param>
    public NgAppClientFeatureDriver( ClientDirectory directory ) => _directory = directory;

    /// <inheritdoc />
    protected override NgAppClientFeature? Create( IActivityMonitor monitor, PartyClient client, ImmutableConfigurationSection configuration )
    {
        var info = _directory.Clients.FirstOrDefault( c => c.Name == client.Name );
        if( info == null )
        {
            monitor.Error( $"The {client} declares \"NgApp\" but the application builds no client application '{client.Name}' (the builder declares it in its \"ckonf.json\")." );
            return null;
        }
        // Its keys are compile time keys: they are here only when the App keeps them ("ProjectCompileKeys").
        var unknown = configuration.GetChildren().Where( c => !CK.Setup.TypeScriptBinPathAspectConfiguration.ClientConfigurationKeys.Contains( c.Key ) ).ToList();
        if( unknown.Count > 0 )
        {
            monitor.Error( $"Unknown \"{unknown[0].Path}\": the keys of \"NgApp\" are compile time keys ('{CK.Setup.TypeScriptBinPathAspectConfiguration.ClientConfigurationKeys.Order().Concatenate( "', '" )}')." );
            return null;
        }
        return new NgAppClientFeature( client, configuration, info );
    }
}
