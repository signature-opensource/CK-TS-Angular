using CK.TypeScript;

namespace CK.Ng.Markdown;

/// <summary>
/// Renders Markdown texts in an Angular application: imports the marked library and adds the standalone
/// <c>MarkdownComponent</c> (<c>&lt;ck-markdown [text]="..."&gt;</c>) to the generated code.
/// </summary>
[TypeScriptPackage]
[TypeScriptImportLibrary( "marked", "^18", DependencyKind.Dependency )]
[TypeScriptFile( "markdown.component.ts", "MarkdownComponent" )]
public class MarkdownPackage : TypeScriptPackage
{
}
