using InvoicrCoreBusiness.InvoiceGenerator;
using InvoicrCoreBusiness.InvoicePersistance;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models.InvoiceEventModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace InvoicrCoreBusiness.InvoiceHandler
{
    public class InvoiceHandler(IInvoicePersistance invoicePersistance, IInvoiceGenerator invoiceGenerator) : IInvoiceHandler
    {
        private readonly IInvoicePersistance _invoicePersistance = invoicePersistance;
		private readonly IInvoiceGenerator _invoiceGenerator = invoiceGenerator;

		public IInvoicePersistance InvoicePersistance=>_invoicePersistance;

		public async Task ProcessEventAsync(InvoiceEvent invoiceEvent)
        {
            //Save invoice
            await _invoiceGenerator.SaveInvoice(invoiceEvent);
        }

		public async Task SaveLastProcessedEventAsync(long lastInvoiceEventId)
		{
			await _invoicePersistance.SaveLastProcessedEventAsync(lastInvoiceEventId);
		}
	}
}
