namespace DNI.Core.Services
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Services;

    internal sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly ISwitch<string, HttpClient> httpClientSwitch;

        public HttpClient GetHttpClient(string key, Uri baseUri = null, HttpMessageHandler messageHandler = null)
        {
            var httpClient = httpClientSwitch.Case(key);

            if (httpClient != null)
            {
                return httpClient;
            }

            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = baseUri,
            };

            httpClientSwitch.CaseWhen(key, httpClient);

            return httpClient;
        }

        public HttpClient GetHttpClient(string key, Uri baseUri = null, Action<HttpRequestMessage> onSendAsync = null)
        {
            return GetHttpClient(key, baseUri, DefaultHttpMessageHandler.Create(onSendAsync));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool gc)
        {
            if (gc)
            {
                return;
            }

            foreach (var (key, client) in httpClientSwitch)
            {
                client?.Dispose();
            }
        }

        public HttpClient GetHttpClient(string key, string url = null, HttpMessageHandler messageHandler = null)
        {
            return GetHttpClient(key, new Uri(url), messageHandler);
        }

        public HttpClient GetHttpClient(string key, string url = null, Action<HttpRequestMessage> sendAsync = null)
        {
            return GetHttpClient(key, new Uri(url), sendAsync);
        }

        public DefaultHttpClientFactory()
            : this(null)
        {
        }

        public DefaultHttpClientFactory(ISwitch<string, HttpClient> httpClientSwitch = null)
        {
            this.httpClientSwitch = httpClientSwitch ?? Switch.Create<string, HttpClient>();
        }
    }

    internal sealed class DefaultHttpMessageHandler : HttpClientHandler
    {
        private readonly Action<HttpRequestMessage> sendAsync;

        private DefaultHttpMessageHandler(Action<HttpRequestMessage> sendAsync)
        {
            this.sendAsync = sendAsync;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            sendAsync(request);
            return await base.SendAsync(request, cancellationToken);
        }

        public static HttpMessageHandler Create(Action<HttpRequestMessage> sendAsync)
        {
            return new DefaultHttpMessageHandler(sendAsync);
        }
    }
}
