using InvoicrCoreBusiness.InvoicePersistance;
using InvoicrCoreModels.Models.InvoiceEventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceHandler
{
    public interface IInvoiceHandler
	{
		Task ProcessEventAsync(InvoiceEvent invoiceEvent);
		Task SaveLastProcessedEventAsync(long lastInvoiceId);
	}
}
