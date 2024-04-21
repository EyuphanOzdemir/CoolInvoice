using Ardalis.Specification;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using System.Linq;

namespace InvoicrCoreBusiness.Specifications.InvoiceEventProcessLogSpecifications
{
    public class InvoiceEventProcessLogAll : Specification<InvoiceEventLastProcessedEvent>
  {
    public InvoiceEventProcessLogAll()
    {
       Query.Where(x => true);
    }
  }
}
