using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicrCoreModels.Models.InvoiceLineItemModels;

namespace InvoicrCoreModels.Models.InvoiceModels
{
    public class InvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public IEnumerable<InvoiceLineItemDto> LineItems { get; set; }
        public string Status { get; set; }
        public DateTime DueDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }

        public decimal Total { get; set; }
		public decimal LineCount { get; set; }
	}
}
