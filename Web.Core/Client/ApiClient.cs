namespace Web.Core.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class ApiClient : IApiClient
    {
        private const string BaseUri = "http://localhost:18313";


        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(requestUri);
                return response;
            }
        }
        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                using (var content = new FormUrlEncodedContent(values))
                {
                    var query = await content.ReadAsStringAsync();
                    var requestUriWithQuery = string.Concat(requestUri, "?", query);
                    var response = await client.GetAsync(requestUriWithQuery);
                    return response;
                }
            }
        }

        public async Task<HttpResponseMessage> GetEncodedDataFromPostReq(string baseUri, string reqUri, object message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var stringContent = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"))
                {

                    HttpResponseMessage response = await client.PostAsync(reqUri, stringContent);
                    return response;
                }
            }
        }
    }
}