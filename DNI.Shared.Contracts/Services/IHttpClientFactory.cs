using System;
using System.Net.Http;

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
