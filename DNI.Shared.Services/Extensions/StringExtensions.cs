using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Text.Encodings.Web;

namespace DNI.Shared.Services.Extensions
{
    public static class StringExtensions
    {
        public static string GetString(this IHtmlContent htmlContent, HtmlEncoder htmlEncoder = default)
        {
            if(htmlEncoder == default)
                htmlEncoder = HtmlEncoder.Create();

            return DisposableHelper
                .Use<string,StringWriter>((stringWriter) => { 
                    htmlContent.WriteTo(stringWriter, htmlEncoder); 
                    return stringWriter.ToString();
                });
        }

        
        public static IHtmlContent Replace(this IHtmlContent value, IHtmlHelper htmlHelper, string findExpression, IHtmlContent replacementHtmlContent)
        {
            var originalString = value.GetString();
            var replacementString = replacementHtmlContent.GetString();
            var stringReplacedHtmlContent = originalString.Replace(findExpression, replacementString);

            return htmlHelper.Raw(stringReplacedHtmlContent);
        }

    }
}
