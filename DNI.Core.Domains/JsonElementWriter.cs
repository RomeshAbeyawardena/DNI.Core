namespace DNI.Core.Domains
{
    using System;
    using Newtonsoft.Json;

    public sealed class JsonElementWriter : IDisposable
    {
        private readonly JsonTextWriter jsonTextWriter;

        public JsonElementWriter(JsonTextWriter jsonTextWriter, string value)
        {
            this.jsonTextWriter = jsonTextWriter;
            this.jsonTextWriter.WritePropertyName(value);
        }

        public void Dispose()
        {
            jsonTextWriter.WriteEnd();
        }
    }
}
