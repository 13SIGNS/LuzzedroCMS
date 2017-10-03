using System;
using System.Text;
using System.Web.Mvc;

namespace LuzzedroCMS.WebUI.Infrastructure.Helpers
{
    public static class PagingExtensions
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tagPreviuosAnchor = new TagBuilder("a");
            tagPreviuosAnchor.InnerHtml = "<span aria-hidden=\"true\">&laquo;</span>";
            TagBuilder tagPreviuosLi = new TagBuilder("li");
            if (pagingInfo.CurrentPage == 1)
            {
                tagPreviuosAnchor.MergeAttribute("href", "#");
                tagPreviuosLi.AddCssClass("disabled");
            }
            else
            {
                tagPreviuosAnchor.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
            }
            tagPreviuosLi.InnerHtml = tagPreviuosAnchor.ToString();
            result.Append("<nav aria-label=\"Page navigation\">" +
                "<ul class=\"pagination\">");
            result.Append(tagPreviuosLi.ToString());

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tagLi = new TagBuilder("li");
                TagBuilder tag = new TagBuilder("a");
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tagLi.AddCssClass("active");
                    tag.MergeAttribute("href", "#");
                }
                else
                {
                    tag.MergeAttribute("href", pageUrl(i));
                }
                tagLi.InnerHtml = tag.ToString();
                result.Append(tagLi.ToString());
            }
            TagBuilder tagNextAnchor = new TagBuilder("a");
            tagNextAnchor.InnerHtml = "<span aria-hidden=\"true\">&raquo;</span>";
            TagBuilder tagNextLi = new TagBuilder("li");
            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
            {
                tagNextAnchor.MergeAttribute("href", "#");
                tagNextLi.AddCssClass("disabled");
            }
            else
            {
                tagNextAnchor.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
            }
            tagNextLi.InnerHtml = tagNextAnchor.ToString();
            result.Append(tagNextLi.ToString());
            result.Append("</ul></nav>");
            return MvcHtmlString.Create(pagingInfo.TotalPages > 1 ? result.ToString() : string.Empty);
        }
    }
}