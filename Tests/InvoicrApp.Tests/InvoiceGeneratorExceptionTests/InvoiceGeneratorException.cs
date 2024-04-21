using Autofac.Core.Lifetime;
using Elfie.Serialization;
using InvoicrCoreBusiness.InvoiceGenerator;
using InvoicrCoreBusiness.Messaging;
using InvoicrCoreModels.Models.InvoiceEventModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace InvoicrApp.Tests.InvoiceGeneratorExceptionTests
{
	[Collection("Sequential")]
	public class InvoiceGeneratorException(IMediator mediator) : IInvoiceGenerator
    {
        public event InvoiceGenerationFailedHandlerDelegate InvoiceGenerationFailed;

        public Task<string> SaveInvoice(InvoiceEvent invoiceEvent)
        {
            try
            {
                throw new Exception(TestParams.EXCEPTION_EVENT_FILE_CONTENT);
            }
            catch(Exception ex) 
            {
                var source = GetType().ToString();
                var message = ex.Message;
				//to test standard event handler
				InvoiceGenerationFailed?.Invoke(this, new InvoiceGenerationFailedEventArgs(source, message));

				//to test mediatr event handling
				mediator.Publish(new InvoiceGenerationFailedNotification(source, message));
			}
            return Task.FromResult(String.Empty);
        }
    }
}
