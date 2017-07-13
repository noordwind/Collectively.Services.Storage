﻿using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Collectively.Services.Storage.Tests.EndToEnd.Framework
{
    public interface IHttpClient
    {
        Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint);
        Task<T> GetAsync<T>(string endpoint);
        Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<Stream> GetStreamAsync(string endpoint);
        Task<HttpResponseMessage> PostAsync(string endpoint, object data);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<HttpResponseMessage> PutAsync(string endpoint, object data);
        Task<HttpResponseMessage> DeleteAsync(string endpoint);
        void SetHeader(string name, string value);
    }
}