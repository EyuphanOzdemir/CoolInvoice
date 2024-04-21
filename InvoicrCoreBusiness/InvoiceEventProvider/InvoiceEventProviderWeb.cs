using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models.InvoiceEventResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceEventProvider
{
    public class InvoiceEventProviderWeb(InvoiceApiUtility invoiceApiUtility, HttpService.IHttpService httpService) : IInvoiceEventProvider
	{
		private readonly HttpService.IHttpService _httpService = httpService;
		InvoiceApiUtility _invoiceApiUtility = invoiceApiUtility;

        public async Task<InvoiceEventResponse> GetInvoiceEventsAsync(long lastInvoiceEventId = 0)
		{
			var requestURI = _invoiceApiUtility.GenerateUri(lastInvoiceEventId);
			return await _httpService.GetInvoiceEventsAsync(requestURI);
		}
	}
}
