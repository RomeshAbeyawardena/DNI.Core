using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Text.Json;

namespace DNI.Shared.Shared.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ToObject<T>(this HttpContent httpContent)
        {
            if(!httpContent.Headers.TryGetValues("content-type", out var contentTypes) 
                || !contentTypes.Any(contentType => contentType.ToLower().StartsWith("application/json")))
                throw new FormatException();

            var content = await httpContent.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
