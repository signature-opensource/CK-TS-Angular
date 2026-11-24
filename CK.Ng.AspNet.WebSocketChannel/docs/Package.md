Angular support of the single WebSocket channel of
[CK.TS.AspNet.WebSocketChannel](https://github.com/signature-opensource/CK-TS-AspNet/blob/stable/CK.TS.AspNet.WebSocketChannel/README.md):
provides the one `WSConnection` of the application and opens it during the bootstrap.

The `NgWebSocketChannelPackage` provides the `WSConnection` on `/ws`, or on the `WS_CONNECTION_URL` injection token
when it is provided, and opens it with `provideWSConnectionSupport`. A feature injects `WSConnection` and registers the
handler of its topic.

See the [README](https://github.com/signature-opensource/CK-TS-Angular/blob/stable/CK.Ng.AspNet.WebSocketChannel/README.md).
