using Elfie.Serialization;
using InvoicrCoreBusiness.Messaging;
using InvoicrCoreBusiness.Utility;
using InvoicrInfrastructure.Email;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace InvoicrApp.Tests.Messaging
{
	// Create a handler for the invoice generation failure event
	public class InvoiceGenerationFailedNotificationHandler(FileUtility fileUtility) : INotificationHandler<InvoiceGenerationFailedNotification>
	{
		public async Task Handle(InvoiceGenerationFailedNotification notification, CancellationToken cancellationToken)
		{
			//to test the event handling via Mediatr
			await File.WriteAllTextAsync(Path.Combine(fileUtility.InvoiceFolder, TestParams.EXCEPTION_MEDIATR_FILE_NAME), String.Join(Environment.NewLine,TestParams.EXCEPTION_MEDIATR_FILE_CONTENT, notification.Source, notification.ErrorMessage));
		}
	}
}
