using InvoicrCoreBusiness.Specifications.InvoiceEventProcessLogSpecifications;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using InvoicrInfrastructure.Data;

namespace InvoicrCoreBusiness.InvoicePersistance
{
    public class InvoicePersistanceDB(EFRepository<InvoiceEventLastProcessedEvent> eventLogRepository) : IInvoicePersistance
	{
		private readonly EFRepository<InvoiceEventLastProcessedEvent> _eventLogRepository = eventLogRepository;

		public async Task<long> GetLastInvoiceEventIdAsync()
		{
			InvoiceEventLastProcessedEvent lastEventProcessed = null;

            try
			{
				lastEventProcessed = (await _eventLogRepository.ListAsync()).FirstOrDefault();
				if (lastEventProcessed == null)
					return 0;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return lastEventProcessed?.EventId ?? 0;
		}
		public async Task SaveLastProcessedEventAsync(long eventId)
		{
			var willAdd = new InvoiceEventLastProcessedEvent() { EventId = eventId };
			var lastEventProcessed = await _eventLogRepository.FirstOrDefaultAsync(new InvoiceEventProcessLogOrderDesc());
			if (lastEventProcessed == null)
				await _eventLogRepository.AddAsync(willAdd);
			else
			{
				lastEventProcessed.EventId = eventId;
				await _eventLogRepository.UpdateAsync(lastEventProcessed);
			}
		}
	}
}
