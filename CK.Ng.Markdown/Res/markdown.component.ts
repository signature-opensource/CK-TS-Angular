import { Component, computed, input, ViewEncapsulation } from '@angular/core';
import { marked } from 'marked';

/**
 * Renders a Markdown text (GitHub flavored, by marked) as HTML.
 * The HTML is bound to [innerHTML]: Angular sanitizes it (no script, no event handler).
 */
@Component( {
    selector: 'ck-markdown',
    standalone: true,
    template: '<div class="ck-markdown" [innerHTML]="html()"></div>',
    // The rendered elements are not part of the template: the styles are global, scoped by the ck-markdown class.
    encapsulation: ViewEncapsulation.None,
    styles: [`
        .ck-markdown > :first-child { margin-top: 0; }
        .ck-markdown > :last-child { margin-bottom: 0; }
        .ck-markdown pre { overflow: auto; padding: 8px; background: rgba( 0, 0, 0, 0.04 ); }
        .ck-markdown code { font-size: 0.9em; }
        .ck-markdown table { border-collapse: collapse; }
        .ck-markdown th, .ck-markdown td { border: 1px solid rgba( 0, 0, 0, 0.15 ); padding: 2px 6px; }
    `]
} )
export class MarkdownComponent {
    /** The Markdown text. Nothing is rendered when it is null, undefined or empty. */
    readonly text = input<string | null | undefined>();

    /** The HTML of the text. */
    readonly html = computed( () => {
        const t = this.text();
        return t ? marked.parse( t, { async: false } ) as string : '';
    } );
}
