using DNI.Shared.Contracts.Enumerations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;
using System.IO;
using System.Text;
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

        
        public static IHtmlContent Base64Encode(this IHtmlHelper htmlHelper, IHtmlContent parameter, Encoding encoding)
        {
            var originalString = parameter.GetString();
            return htmlHelper
                .Raw(Convert
                    .ToBase64String(encoding
                        .GetBytes(originalString)));
        }

        
        public static IHtmlContent Replace(this IHtmlContent value, IHtmlHelper htmlHelper, string findExpression, IHtmlContent replacementHtmlContent)
        {
            var originalString = value.GetString();
            var replacementString = replacementHtmlContent.GetString();
            var stringReplacedHtmlContent = originalString.Replace(findExpression, replacementString);

            return htmlHelper.Raw(stringReplacedHtmlContent);
        }

        public static string Case(this string value, StringCase @case, CultureInfo cultureInfo = default)
        {
            if(cultureInfo == default)
                cultureInfo = CultureInfo.InvariantCulture;

            if(@case == StringCase.None)
                return value;

            if(@case == StringCase.Upper)
                return value.ToUpper(cultureInfo);

            if(@case == StringCase.Lower)
                return value.ToLower(cultureInfo);

            return value;
        }
    }
}
