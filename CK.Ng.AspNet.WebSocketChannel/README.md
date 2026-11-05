# CK.Ng.AspNet.WebSocketChannel

The features of a client application that need messages pushed by the server share one WebSocket: the `WSConnection`
of [CK.TS.AspNet.WebSocketChannel](https://github.com/signature-opensource/CK-TS-AspNet/blob/stable/CK.TS.AspNet.WebSocketChannel/README.md).
In an Angular application, this connection must be one service that every feature finds, and it must be opened once.
This package does both: it provides the one `WSConnection` of the application and opens it during the bootstrap.

## The package

[`NgWebSocketChannelPackage`](NgWebSocketChannelPackage.cs) is a TypeScript package that requires the
`WebSocketChannelPackage` and registers two providers at the root of the application:

- `WSConnection`, created on the URL of the `WS_CONNECTION_URL` injection token when it is provided, on `/ws`
  otherwise:

  ```ts
  { provide: WSConnection, useFactory: () => new WSConnection( inject( WS_CONNECTION_URL, { optional: true } ) ?? '/ws' ) }
  ```

- `provideWSConnectionSupport()` ([`Res/ws-connection-support.ts`](Res/ws-connection-support.ts)): an application
  initializer that starts the connection (`WSConnection.start`).

The initializer doesn't wait for the connection: Angular renders at once. A feature waits where it needs to, through
`WSConnection.whenConnectedAsync` or the `WSTopicHandler.onConnected` callback of its topic.

## Using the channel

A feature that uses the channel requires this package and injects `WSConnection`: it never opens a socket, it registers
the handler of its topic (`WSConnection.addHandler`).

The `/ws` default is the default path of the server
(`WebApplicationBuilderExtensions.DefaultPath` of [CK.AspNet.WebSocketChannel](https://github.com/signature-opensource/CK-AspNet/blob/stable/CK.AspNet.WebSocketChannel/README.md)).
When the server maps another path, the application provides the [`WS_CONNECTION_URL`](Res/WS_CONNECTION_URLToken.ts)
token (an `InjectionToken<string>`, exported by `@local/ck-gen`) with this path.

The opening during the bootstrap is the provider whose source is
`CK.Ng.AspNet.WebSocketChannel.NgWebSocketChannelPackage#Support`: an application that opens the connection itself
removes it with `CKGenAppModule.Providers.exclude( "CK.Ng.AspNet.WebSocketChannel.NgWebSocketChannelPackage#Support" )`
(see [CK.TS.Angular.Engine](../CK.TS.Angular.Engine/README.md#the-providers)).

## Requires

- [CK.TS.AspNet.WebSocketChannel](https://github.com/signature-opensource/CK-TS-AspNet/blob/stable/CK.TS.AspNet.WebSocketChannel/README.md)
  for the `WSConnection`.
- [CK.TS.Angular](../CK.TS.Angular/README.md) for the providers registration.
