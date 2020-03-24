namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using Microsoft.Extensions.Options;

    public class DefaultJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public DefaultJsonSerializer(IOptions<JsonSerializerOptions> options)
        {
            jsonSerializerOptions = options.Value;
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, jsonSerializerOptions);
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, jsonSerializerOptions);
        }
    }
}
