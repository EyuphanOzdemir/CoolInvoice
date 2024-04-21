using InvoicrCoreBusiness.InvoiceEventProvider;
using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.InvoicePersistance;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models;
using InvoicrCoreModels.Models.InvoiceEventResponseModels;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace InvoicrCoreBusiness.InvoiceWorker
{
    public class InvoiceWorker(IInvoiceHandler invoiceHandler, IInvoiceEventProvider invoiceEventProvider, IInvoicePersistance invoicePersistance, ILogger<InvoiceWorker> logger, AppConfiguration configuration) : IInvoiceWorker
    {
		private readonly IInvoicePersistance _invoicePersistance = invoicePersistance;
        private readonly IInvoiceEventProvider _invoiceEventProvider = invoiceEventProvider;
		private readonly IInvoiceHandler _invoiceHandler = invoiceHandler;
        private readonly ILogger<InvoiceWorker> _logger = logger;
        private readonly AppConfiguration _configuration = configuration;

        public async Task ExecuteSingleAsync(CancellationToken stoppingToken)
        {
            InvoiceEventResponse invoiceEventResponse = new();
            //todo: log each execution
            _logger.LogInformation("Checking for new invoices...");
            if (!stoppingToken.IsCancellationRequested)
            {
                var lastInvoiceEventId = await _invoicePersistance.GetLastInvoiceEventIdAsync();
                try
                {
                    invoiceEventResponse = await _invoiceEventProvider.GetInvoiceEventsAsync(lastInvoiceEventId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (invoiceEventResponse.Items.Any())
                {
                    var lastProcessedInvoiceEventID = 0L;
                    foreach (var invoiceEvent in invoiceEventResponse.Items)
                    {
                       await _invoiceHandler.ProcessEventAsync(invoiceEvent);
                       lastProcessedInvoiceEventID = invoiceEvent.Id;
                    }
					await _invoiceHandler.SaveLastProcessedEventAsync(lastProcessedInvoiceEventID);
				}
            }
        }
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Background task has started");
                while (!stoppingToken.IsCancellationRequested)
                {
                    await ExecuteSingleAsync(stoppingToken);
                    // Add a delay of X seconds between cycles
                    await Task.Delay(TimeSpan.FromSeconds(_configuration.Worker_Frequency), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                _logger.LogInformation("Background task has been canceled.");
            }
        }
    }
}
