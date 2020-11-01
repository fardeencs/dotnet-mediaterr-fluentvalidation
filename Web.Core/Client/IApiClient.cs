namespace Web.Core.Client
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IApiClient
    {
        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);

        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri);

        Task<HttpResponseMessage> GetEncodedDataFromPostReq(string baseUri, string reqUri, object message);
    }
}