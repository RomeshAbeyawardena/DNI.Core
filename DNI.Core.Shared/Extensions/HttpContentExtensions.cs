﻿namespace DNI.Core.Shared.Extensions
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class HttpContentExtensions
    {
        public static async Task<T> ToObject<T>(this HttpContent httpContent, Action<JsonSerializerOptions> jsonSerializerOptionsBuilder = null)
        {
            if (!httpContent.Headers.TryGetValues("content-type", out var contentTypes)
                || !contentTypes.Any(contentType => contentType.ToLower().StartsWith("application/json")))
            {
                throw new FormatException();
            }

            var jsonSerializerOptions = new JsonSerializerOptions();

            jsonSerializerOptionsBuilder?.Invoke(jsonSerializerOptions);

            var content = await httpContent.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
