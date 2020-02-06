using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface IHttpClientService : IDisposable
    {
        HttpClient GetHttpClient(string key, Uri baseUri = null, HttpMessageHandler messageHandler = null);
    }
}
