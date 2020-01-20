using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace DNI.Shared.Shared.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> GetBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static string DisplayIf(this string value, bool condition = true, string valueOnConditionFalse = default)
        {
            return condition ? value : valueOnConditionFalse;
        }

        public static IHtmlContent Replace(this IHtmlContent value, IHtmlHelper htmlHelper, string findExpression, IHtmlContent replacedHtmlContent)
        {
            var textWriter = new StringWriter();
            value.WriteTo(textWriter, HtmlEncoder.Create(new TextEncoderSettings()));
            textWriter.ToString()
            var stringOriginalHtmlContent = replacedHtmlContent.ToString();

            var stringReplacedHtmlContent = value.Replace(findExpression, stringHtmlContent);

            return htmlHelper.Raw(stringReplacedHtmlContent);
        }

        private static string GetString()
        {
            DisposableHelper.Use(var textWriter = new StringWriter())
        }
    }
}
