
using InvoicrCoreModels.Models.InvoiceModels;

namespace InvoicrCoreModels.Models.InvoiceEventModels
{
    public class InvoiceEventDto
    {
        public long Id { get; set; }
        public string EventType { get; set; }
        public InvoiceDto Content { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
