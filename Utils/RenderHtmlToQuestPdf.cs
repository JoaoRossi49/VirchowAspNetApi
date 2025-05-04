using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using IElement = AngleSharp.Dom.IElement;

namespace VirchowAspNetApi.Utils
{ 

    public static class RenderHtmlToQuestPdf
    {
        public static void RenderHtml(this IContainer container, string html)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            container.Column(col =>
            {
                foreach (var paragraph in document.QuerySelectorAll("p"))
                {
                    col.Item().Text(text =>
                    {
                        foreach (var node in paragraph.ChildNodes)
                        {
                            switch (node)
                            {
                                case IText textNode:
                                    text.Span(textNode.Text);
                                    break;

                                case IElement element:
                                    var content = element.TextContent;

                                    switch (element.TagName.ToLower())
                                    {
                                        case "b":
                                        case "strong":
                                            text.Span(content).Bold();
                                            break;
                                        case "i":
                                        case "em":
                                            text.Span(content).Italic();
                                            break;
                                        case "u":
                                            text.Span(content).Underline();
                                            break;
                                        case "br":
                                            text.Span("\n");
                                            break;
                                        default:
                                            text.Span(content);
                                            break;
                                    }

                                    break;
                            }
                        }
                    });
                }
            });
        }

    }
}
