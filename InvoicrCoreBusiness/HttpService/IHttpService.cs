using InvoicrCoreModels.Models.InvoiceEventResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.HttpService
{
    public interface IHttpService
    {
        public Task<InvoiceEventResponse> GetInvoiceEventsAsync(string requestURI);
    }
}
