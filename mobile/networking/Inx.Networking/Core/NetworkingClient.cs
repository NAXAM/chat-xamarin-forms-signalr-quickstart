using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Inx.Networking.Core
{
    public interface INetworkingClient
    {
        Task<bool> DeleteAsync(string urlOrResource);
        Task<T> GetAsync<T>(string urlOrResource);
        Task<TResponse> PostAsync<TRequest, TResponse>(string urlOrResource, TRequest dto);
        Task<TResponse> PutAsync<TRequest, TResponse>(string urlOrResource, TRequest dto);
    }

    public class DefaultNetworkingClient : INetworkingClient
    {
        static string CONTENT_TYPE = "APPLICATION/JSON";

        readonly HttpClient client;
        readonly Func<JsonSerializer> jsonSerializerGetter;
        readonly Func<KeyValuePair<string, string>> authorizationTokenGetter;

        public DefaultNetworkingClient(
            Func<JsonSerializer> jsonSerializerGetter,
            Func<KeyValuePair<string, string>> authorizationTokenGetter,
            string baseUrl
        )
        {
            this.authorizationTokenGetter = authorizationTokenGetter;
            this.jsonSerializerGetter = jsonSerializerGetter;
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
        }

        public async Task<bool> DeleteAsync(string url)
        {
            EnsureRequiredHeaders();
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            EnsureRequiredHeaders();
            try
            {
                var response = await client.GetAsync(url);
                var result = await Deserialize<T>(response);
                return result;
            }
            catch
            {
                return default(T);
            }

        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string urlOrResource, TRequest dto)
        {
            var content = Serialize(dto);
            EnsureRequiredHeaders();
            try
            {
                var response = await client.PostAsync(urlOrResource, content);
                return await Deserialize<TResponse>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return default(TResponse);
            }
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string urlOrResource, TRequest dto)
        {
            var content = Serialize(dto);
            EnsureRequiredHeaders();
            try
            {
                var response = await client.PutAsync(urlOrResource, content);
                return await Deserialize<TResponse>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return default(TResponse);
            }
        }

        void EnsureRequiredHeaders()
        {
            var kvp = authorizationTokenGetter.Invoke();

            if (string.IsNullOrWhiteSpace(kvp.Key)
                || string.IsNullOrWhiteSpace(kvp.Value))
            {
                return;
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                kvp.Key,
                kvp.Value
            );
        }

        async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializer = jsonSerializerGetter.Invoke();

                if (jsonSerializer == null) return default(T);

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            return jsonSerializer.Deserialize<T>(jsonReader);
                        }
                    }
                }
            }
            return default(T);
        }

        StringContent Serialize<T>(T dto)
        {
            var jsonSerializer = jsonSerializerGetter.Invoke();

            if (jsonSerializer == null) return null;

            var jsonBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(jsonBuilder))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    jsonSerializer.Serialize(jsonWriter, dto);
                }
            }

            return new StringContent(jsonBuilder.ToString(), Encoding.UTF8, CONTENT_TYPE);
        }
    }

}
