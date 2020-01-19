using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent DisplayIf(this IHtmlHelper htmlHelper, 
            bool condition, IHtmlContent displayOnConditionTrue, 
            IHtmlContent displayOnConditionFalse = default)
        {
            if (condition)
                return displayOnConditionTrue;
            
            if(displayOnConditionFalse == default)
                return displayOnConditionFalse;
            
            return htmlHelper.Raw(string.Empty);
        }

        public static async Task<IHtmlContent> DisplayIfAsync(this IHtmlHelper htmlHelper, 
            Task<bool> condition, Task<IHtmlContent> displayOnConditionTrue,
            Task<IHtmlContent> displayOnConditionFalse = default)
        {
            if(await condition
                .ConfigureAwait(false))
                return await displayOnConditionTrue
                    .ConfigureAwait(false);

            if(displayOnConditionFalse == default)
                return await displayOnConditionFalse
                    .ConfigureAwait(false);

            return htmlHelper.Raw(string.Empty);
        }

        public static async Task<IHtmlContent> DisplayIfAsync(this IHtmlHelper htmlHelper,
            Task<bool> condition, Task<string> displayonConditionTrue, 
            Task<string> displayonConditionFalse = default)
        {
            return await DisplayIfAsync(htmlHelper, condition, 
                htmlHelper.HtmlRawAsync(displayonConditionTrue), 
                htmlHelper.HtmlRawAsync(displayonConditionFalse));
        }

        public static async Task<IHtmlContent> HtmlRawAsync(this IHtmlHelper htmlHelper, Task<string> contentTask)
        {
            var content = await contentTask;

            return htmlHelper.Raw(content);
        }

        public static IHtmlContent DisplayIf(this IHtmlHelper htmlHelper, 
            bool condition, string displayOnConditionTrue, 
            string displayOnConditionFalse = default)
        {
            return DisplayIf(htmlHelper, condition, 
                htmlHelper.Raw(displayOnConditionTrue), 
                displayOnConditionFalse == null 
                ? null 
                : htmlHelper.Raw(displayOnConditionFalse));
        }
    }
}
