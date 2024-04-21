using InvoicrCoreModels.Models.InvoiceLineItemModels;
using System;
using System.Collections.Generic;

namespace InvoicrCoreModels.Models.InvoiceModels
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public IEnumerable<InvoiceLineItem> LineItems { get; set; }
        public string Status { get; set; }
        public DateTime DueDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }

        public decimal Total => LineItems.Sum(item => item.LineItemTotalCost);
		public decimal LineCount => LineItems.Count();
	}
}
