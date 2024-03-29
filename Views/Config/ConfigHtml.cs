using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SuperbrainManagement.Views.ConfigView
{
    public class ConfigHtml
    {
 

            public static string GenerateInlineList(int itemId, List<string> permissionItems)
            {
                StringBuilder htmlBuilder = new StringBuilder();

                htmlBuilder.Append("<ul class=\"list-inline mb-0\" style=\"width:200px;\">");

                // Li 1
                htmlBuilder.Append("<li class=\"list-inline-item\">");
                htmlBuilder.Append("<a href=\"javascript:void(0);\"");
                htmlBuilder.Append("data-bs-toggle=\"tooltip\"");
                htmlBuilder.Append("data-bs-placement=\"top\"");
                htmlBuilder.Append("title=\"Edit\"");
                htmlBuilder.Append("class=\"px-2 text-primary\">");
                htmlBuilder.Append("<i class=\"bx bx-pencil font-size-18\"></i>");
                htmlBuilder.Append("</a>");
                htmlBuilder.Append("</li>");

                // Li 2
                htmlBuilder.Append("<li class=\"list-inline-item\">");
                htmlBuilder.Append("<a href=\"javascript:void(0);\"");
                htmlBuilder.Append("data-bs-toggle=\"tooltip\"");
                htmlBuilder.Append("data-bs-placement=\"top\"");
                htmlBuilder.Append("title=\"Delete\"");
                htmlBuilder.Append("class=\"px-2 text-danger\">");
                htmlBuilder.Append("<i class=\"bx bx-trash-alt font-size-18\"></i>");
                htmlBuilder.Append("</a>");
                htmlBuilder.Append("</li>");

                // Li 3 - dropdown
                htmlBuilder.Append("<li class=\"list-inline-item dropdown\">");
                htmlBuilder.Append("<a class=\"text-muted dropdown-toggle font-size-18 px-2\"");
                htmlBuilder.Append("href=\"#\"");
                htmlBuilder.Append("id=\"drop\"");
                htmlBuilder.Append("data-bs-toggle=\"dropdown\"");
                htmlBuilder.Append("aria-expanded=\"false\">");
                htmlBuilder.Append("<i class=\"bx bx-dots-vertical-rounded\"></i>");
                htmlBuilder.Append("</a>");
                htmlBuilder.Append("<div class=\"dropdown-menu dropdown-menu-end\" aria-labelledby=\"drop2\" data-bs-popper=\"static\">");

                // Dropdown Items
                foreach (var permissionItem in permissionItems)
                {
                    htmlBuilder.AppendFormat("<a class=\"dropdown-item\" href=\"#\" onclick=\"getDataPermission({0})\" data-bs-toggle=\"modal\" data-bs-target=\"#exampleModal\">{1}</a>", itemId, permissionItem);
                }

                htmlBuilder.Append("</div>");
                htmlBuilder.Append("</li>");

                htmlBuilder.Append("</ul>");

                return htmlBuilder.ToString();
            }

}
}