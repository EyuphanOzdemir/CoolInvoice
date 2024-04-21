using InvoicrCoreModels.Models.InvoiceEventResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.HttpService
{
    public class HttpServiceDefault : IHttpService
    {
        readonly JsonSerializerOptions _jsonSerializerOptions;
        readonly HttpClient _httpClient;

        public HttpServiceDefault(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            //_jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            _httpClient = httpClient;
        }
        public async Task<InvoiceEventResponse> GetInvoiceEventsAsync(string requestURI)
        {
            return await _httpClient.GetFromJsonAsync<InvoiceEventResponse>(requestURI, _jsonSerializerOptions);
        }
    }
}
