using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultHttpClientService : IHttpClientService
    {
        private readonly ISwitch<string, HttpClient> _httpClientSwitch;

        public HttpClient GetHttpClient(string key, Uri baseUri = null, HttpMessageHandler messageHandler = null)
        {
            var httpClient = _httpClientSwitch.Case(key);
            
            if(httpClient != null)
                return httpClient;

            if(baseUri == null)
                throw new ArgumentNullException(nameof(baseUri));

            httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = baseUri
            };

            _httpClientSwitch.CaseWhen(key, httpClient);
            
            return httpClient;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool gc)
        {
            foreach(var (key, client) in _httpClientSwitch)
            {
                client?.Dispose();
            }
        }

        public DefaultHttpClientService()
            : this(null)
        {

        }

        public DefaultHttpClientService(ISwitch<string, HttpClient> httpClientSwitch = null)
        {
            _httpClientSwitch = httpClientSwitch ?? Switch.Create<string, HttpClient>();
        }
    }
}
