using InvoicrCoreModels.Models.InvoiceEventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceGenerator
{
    public interface IInvoiceGenerator
	{
		event InvoiceGenerationFailedHandlerDelegate InvoiceGenerationFailed;
		Task<string> SaveInvoice(InvoiceEvent invoiceEvent);
	}
}
