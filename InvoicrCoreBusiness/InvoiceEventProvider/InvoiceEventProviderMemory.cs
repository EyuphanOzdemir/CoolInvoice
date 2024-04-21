using InvoicrCoreModels.Models.InvoiceEventResponseModels;
using InvoicrCoreModels.Models.InvoiceLineItemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceEventProvider
{
    public class InvoiceEventProviderMemory : IInvoiceEventProvider
	{
		public async Task<InvoiceEventResponse> GetInvoiceEventsAsync(long lastInvoiceEventId = 0)
		{
			var eventResponse = new InvoiceEventResponse()
			{ Items = 
			    [
				  new(){Id = ++lastInvoiceEventId,
						CreatedDateUtc = DateTime.UtcNow,
						EventType = "INVOICE_CREATED",
						Content = new (){CreatedDateUtc = DateTime.UtcNow,
										 InvoiceId = Guid.NewGuid(),
										 DueDateUtc = DateTime.UtcNow,
										 InvoiceNumber = "1789",
										 Status = "DRAFT",
										 UpdatedDateUtc = DateTime.UtcNow,
										 LineItems = [
													  new InvoiceLineItem()
														  {
															Description = "LineItem1",
															LineItemId = Guid.NewGuid(),
															Quantity = 1,
															UnitCost = 5.6m,
															LineItemTotalCost = 5.6m
														  },
													  new InvoiceLineItem()
														  {
															Description = "LineItem2",
															LineItemId = Guid.NewGuid(),
															Quantity = 2,
															UnitCost = 2.1m,
															LineItemTotalCost = 4.2m
														  }
													 ]
										}
					   }
				]
			};

			return await Task.FromResult(eventResponse);
		}
	}
}
