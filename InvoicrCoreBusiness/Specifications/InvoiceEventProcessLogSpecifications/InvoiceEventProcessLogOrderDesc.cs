using Ardalis.Specification;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using System.Linq;

namespace InvoicrCoreBusiness.Specifications.InvoiceEventProcessLogSpecifications
{
    public class InvoiceEventProcessLogOrderDesc : Specification<InvoiceEventLastProcessedEvent>, ISingleResultSpecification<InvoiceEventLastProcessedEvent>
  {
    public InvoiceEventProcessLogOrderDesc()
    {
       Query.OrderByDescending(e => e.EventId);
    }
  }
}
