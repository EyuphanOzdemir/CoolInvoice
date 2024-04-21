using InvoicrCoreModels.Models.InvoiceModels;
using System;

namespace InvoicrCoreModels.Models.InvoiceEventModels
{
    public class InvoiceEvent
    {
        public long Id { get; set; }
        public string EventType { get; set; }
        public Invoice Content { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
