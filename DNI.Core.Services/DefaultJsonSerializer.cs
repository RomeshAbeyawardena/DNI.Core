using DNI.Core.Contracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DefaultJsonSerializer(IOptions<JsonSerializerOptions> options)
        {
            _jsonSerializerOptions = options.Value;
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, _jsonSerializerOptions);
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, _jsonSerializerOptions);
        }
    }
}
