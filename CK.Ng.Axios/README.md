# CK.Ng.Axios

The features of an Angular application that call the server with [axios](https://axios-http.com/) share one
`AxiosInstance` rather than each creating its own. This package provides it: it brings the axios library and the Angular
`AXIOS` injection token, provided with a default instance.

## The package

[`AxiosPackage`](AxiosPackage.cs) is a TypeScript package that:

- declares the `axios` library (`^1.7.7`) as a peer dependency;
- adds [`Res/AXIOSToken.ts`](Res/AXIOSToken.ts) to the generated code (in `ck-gen/CK/Ng`), which exports the `AXIOS`
  token: an `InjectionToken<AxiosInstance>`;
- provides the token at the root of the application: `{ provide: AXIOS, useValue: axios.create() }`.

A feature injects it:

```ts
import { AXIOS } from '@local/ck-gen';

readonly #axios = inject( AXIOS );
```

An application that needs another configuration of the instance excludes the provider by its source,
`CKGenAppModule.Providers.exclude( "CK.Ng.Axios.AxiosPackage" )`, and provides `AXIOS` itself (see
[CK.TS.Angular.Engine](../CK.TS.Angular.Engine/README.md#the-providers)).

## Requires

- [CK.TS.Angular](../CK.TS.Angular/README.md) for the providers registration.
