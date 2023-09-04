using Newtonsoft.Json;
using PaymentGateway.Application.Infrastructure;

namespace PaymentGateway.Infrastructure.ExternalData
{
    public class HttpProcessor: IHttpProcessor
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpProcessor(IHttpClientFactory httpClientFactory)
        { 
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// A Generic function to process HttpRequests and serialize input and Deserialize output
        /// </summary>
        /// <typeparam name="TInput">A generic model  as an input http request</typeparam>
        /// <typeparam name="TOutput">A generic model as output of http request</typeparam>
        /// <param name="input">A generic model  as an input of http request</param>
        /// <param name="method">Http method; POST, GET, PUT</param>
        /// <param name="url">representing the URL http request will be sent to</param>
        /// <returns>a deserialized object from response of http request</returns>
        public async Task<TOutput> SendRequest<TInput, TOutput>(TInput input, HttpMethod method, string url)
        {
            var content = input is null ? null: new StringContent(JsonConvert.SerializeObject(input), System.Text.Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = new Uri(url),
                Content = content,

            };
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(request);
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseObject =
                JsonConvert.DeserializeObject<TOutput>(responseString);
            return responseObject;
        }
    }
}
