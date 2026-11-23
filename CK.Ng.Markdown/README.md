# CK.Ng.Markdown

A text written in Markdown (a package readme, a documentation summary) is rendered the same way everywhere in an
Angular application: this package provides the component that does it, with the [marked](https://marked.js.org/)
library.

## The package

[`MarkdownPackage`](MarkdownPackage.cs) is a TypeScript package that:

- declares the `marked` library (`^18`) as a dependency;
- adds [`Res/markdown.component.ts`](Res/markdown.component.ts) to the generated code, which exports the standalone
  `MarkdownComponent`.

## The component

```html
<ck-markdown [text]="readme" />
```

- `text` is the Markdown text (GitHub flavored); nothing is rendered when it is null, undefined or empty.
- The HTML is bound to `[innerHTML]`: Angular sanitizes it, a text can't inject a script or an event handler.
- The rendered elements are not part of the component's template: its styles are global (`ViewEncapsulation.None`),
  scoped by the `ck-markdown` class of its root element (no margin before the first block and after the last one,
  scrollable code blocks, bordered tables).
