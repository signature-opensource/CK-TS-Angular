# CK.Ng.PublicPage

An application has pages that anybody can see, before any authentication. The packages that bring such pages need one
place to route them: this package is that place, the public page. It is the routed component of the application root,
on the empty route, and it is nothing but a `<router-outlet />`: what it shows comes from the components routed to it
and from the transformers of the packages.

## The component

[`PublicPageComponent`](PublicPageComponent.cs) is an `NgRoutedComponent`:

```csharp
[NgRoutedComponent<AppComponent>( HasRoutes = true, Route = "" )]
[OptionalRequires<INgPrivatePageComponent>]
public sealed class PublicPageComponent : NgRoutedComponent
```

- its target is the `AppComponent`, on the `""` route: it is the root page of the application;
- it has routes (`HasRoutes = true`): a component routed to it with `[NgRoutedComponent<PublicPageComponent>]`
  appears as a child route;
- it requires the private page (`INgPrivatePageComponent`) when one is in the configuration.

Its resources are the `PublicPage` Angular component (selector `ck-public-page`):
[`public-page.ts`](Res/public-page.ts), [`public-page.html`](Res/public-page.html) (a single `<router-outlet />`) and
[`public-page.less`](Res/public-page.less), which takes the full width and height.

## Transforming the public page

A package changes the public page with its transformers (a `.t` file in its resources). The `public-page.less` has
insertion points for them: `<PreContent revert />` before the `ck-public-page` rule, `<CKPublicPage />` inside it
and `<PostContent />` after it.
For instance, the test package `CK.Ng.PublicSection` (in
[CK.TS.Angular.Tests](../Tests/Angular/CK.TS.Angular.Tests/CK.Ng.PublicSection/Res/public-page.t)) gives the public page a
background and a button:

```
create <less> transformer
begin
  inject """
         background-image: url("/images/background.jpg");
         background-size: cover;
         background-position: 70% 50%;
         display: inline-block;
         """ into <CKPublicPage>;
end

create <html> transformer
begin
    insert """
           <div>
           <button nz-button nzType="primary" routerLink="/auth">
                          {{ 'Button.GoToLogin' }}
           </button>
           </div>
 
           """ before "<router-outlet />";
end
```

and a `<ts>` transformer of the same file imports what the button uses (`NzButtonModule`, `RouterLink`) in the
component.

## Requires

- [CK.TS.Angular](../CK.TS.Angular/README.md) for the routed components.
