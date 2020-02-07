using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface IHttpClientFactory : IDisposable
    {
        HttpClient GetHttpClient(string key, Uri baseUri = null, HttpMessageHandler messageHandler = null);
        HttpClient GetHttpClient(string key, Uri baseUri = null, Action<HttpRequestMessage> sendAsync = null);
        HttpClient GetHttpClient(string key, string baseUri = null, HttpMessageHandler messageHandler = null);
        HttpClient GetHttpClient(string key, string baseUri = null, Action<HttpRequestMessage> sendAsync = null);
    }
}
