using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels
{
    public class InvoiceEventLastProcessedEvent
    {
        public int Id { get; set; }

        public long EventId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
