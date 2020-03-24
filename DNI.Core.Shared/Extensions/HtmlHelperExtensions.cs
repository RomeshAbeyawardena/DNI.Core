namespace DNI.Core.Shared.Extensions
{
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Represents an if with an optional else block.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="condition"></param>
        /// <param name="displayOnConditionTrue"></param>
        /// <param name="displayOnConditionFalse"></param>
        /// <returns></returns>
        public static IHtmlContent DisplayIf(
            this IHtmlHelper htmlHelper,
            bool condition,
            IHtmlContent displayOnConditionTrue,
            IHtmlContent displayOnConditionFalse = default)
        {
            if (condition)
            {
                return displayOnConditionTrue;
            }

            if (displayOnConditionFalse != default)
            {
                return displayOnConditionFalse;
            }

            return htmlHelper.Raw(string.Empty);
        }

        /// <summary>
        /// Represents an async if with an optional else block, without blocking the current thread until its needed to be displayed.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="condition"></param>
        /// <param name="displayOnConditionTrue"></param>
        /// <param name="displayOnConditionFalse"></param>
        /// <returns></returns>
        public static async Task<IHtmlContent> DisplayIfAsync(
            this IHtmlHelper htmlHelper,
            Task<bool> condition,
            Task<IHtmlContent> displayOnConditionTrue,
            Task<IHtmlContent> displayOnConditionFalse = default)
        {
            if (await condition
                .ConfigureAwait(false))
            {
                return await displayOnConditionTrue
                    .ConfigureAwait(false);
            }

            if (displayOnConditionFalse != default)
            {
                return await displayOnConditionFalse
                    .ConfigureAwait(false);
            }

            return htmlHelper.Raw(string.Empty);
        }

        /// <summary>
        /// Represents an async if with an optional else block, without blocking the current thread until its needed to be displayed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="condition"></param>
        /// <param name="displayOnConditionTrue"></param>
        /// <param name="displayOnConditionFalse"></param>
        /// <returns></returns>
        public static async Task<IHtmlContent> DisplayIfAsync<T>(
            this IHtmlHelper htmlHelper,
            Task<bool> condition,
            Task<T> displayOnConditionTrue,
            Task<T> displayOnConditionFalse = default)
        {
            return await DisplayIfAsync(
                htmlHelper,
                condition,
                htmlHelper.HtmlRawAsync(displayOnConditionTrue),
                htmlHelper.HtmlRawAsync(displayOnConditionFalse));
        }

        /// <summary>
        /// Represents an async if with an optional else block, without blocking the current thread until its needed to be displayed.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="condition"></param>
        /// <param name="displayonConditionTrue"></param>
        /// <param name="displayonConditionFalse"></param>
        /// <returns></returns>
        public static async Task<IHtmlContent> DisplayIfAsync(
            this IHtmlHelper htmlHelper,
            Task<bool> condition,
            Task<string> displayonConditionTrue,
            Task<string> displayonConditionFalse = default)
        {
            return await DisplayIfAsync(
                htmlHelper,
                condition,
                htmlHelper.HtmlRawAsync(displayonConditionTrue),
                htmlHelper.HtmlRawAsync(displayonConditionFalse));
        }

        /// <summary>
        /// Converts a string from an async request to IHTML, without blocking the current thread until its needed to be displayed.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="contentTask"></param>
        /// <returns></returns>
        public static async Task<IHtmlContent> HtmlRawAsync(this IHtmlHelper htmlHelper, Task<string> contentTask)
        {
            var content = await contentTask;

            return htmlHelper.Raw(content);
        }

        public static async Task<IHtmlContent> HtmlRawAsync<T>(this IHtmlHelper htmlHelper, Task<T> contentTask)
        {
            var content = await contentTask;

            return htmlHelper.Raw(content);
        }

        /// <summary>
        /// Represents an if with an optional else block.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="condition"></param>
        /// <param name="displayOnConditionTrue"></param>
        /// <param name="displayOnConditionFalse"></param>
        /// <returns></returns>
        public static IHtmlContent DisplayIf(
            this IHtmlHelper htmlHelper,
            bool condition,
            string displayOnConditionTrue,
            string displayOnConditionFalse = default)
        {
            return DisplayIf(
                htmlHelper,
                condition,
                htmlHelper.Raw(displayOnConditionTrue),
                displayOnConditionFalse == null ? null : htmlHelper.Raw(displayOnConditionFalse));
        }

        public static IHtmlContent FormatContent(this IHtmlHelper htmlHelper, string format, params object[] values)
        {
            return htmlHelper.Raw(string.Format(format, values));
        }

        public static IHtmlContent Switch<TKey, TValue>(this IHtmlHelper htmlHelper, ISwitch<TKey, TValue> @switch, TKey currentValue)
        {
            return htmlHelper.Raw(@switch.Case(currentValue));
        }
    }
}
