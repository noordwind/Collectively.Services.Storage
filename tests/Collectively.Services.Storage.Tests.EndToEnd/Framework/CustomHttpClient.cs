﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Collectively.Services.Storage.Tests.EndToEnd.Framework
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient(string url)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            _httpClient.DefaultRequestHeaders.Remove("Accept");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
        }

        public async Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint)
            => await GetAsync<IEnumerable<T>>(endpoint);

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return default(T);

            return await DeserializeAsync<T>(response);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
            => await _httpClient.GetAsync(endpoint);

        public async Task<Stream> GetStreamAsync(string endpoint)
            => await _httpClient.GetStreamAsync(endpoint);

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
            => await _httpClient.PostAsync(endpoint, GetJsonContent(data));

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var response = await PostAsync(endpoint, data);
            if (!response.IsSuccessStatusCode)
                return default(T);

            return await DeserializeAsync<T>(response);
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, object data)
            => await _httpClient.PutAsync(endpoint, GetJsonContent(data));

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
            => await _httpClient.DeleteAsync(endpoint);

        public void SetHeader(string name, string value)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(name))
                _httpClient.DefaultRequestHeaders.Remove(name);

            _httpClient.DefaultRequestHeaders.Add(name, value);
        }

        private static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
            {
                ContractResolver = new MyContractResolver()
            });

            return result;
        }

        private static StringContent GetJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private class MyContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                .Where(p => p.CanWrite)
                                .Select(p => base.CreateProperty(p, memberSerialization))
                            .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                       .Select(f => base.CreateProperty(f, memberSerialization)))
                            .ToList();
                props.ForEach(p => { p.Writable = true; p.Readable = true; });
                return props;
            }
        }
    }
}