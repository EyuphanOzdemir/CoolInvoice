using InvoicrCoreModels.Models.InvoiceEventResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceEventProvider
{
    public interface IInvoiceEventProvider
	{
		Task<InvoiceEventResponse> GetInvoiceEventsAsync(long lastInvoiceEventId = 0);
	}
}
