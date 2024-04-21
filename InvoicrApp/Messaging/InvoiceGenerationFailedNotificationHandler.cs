using InvoicrCoreBusiness.Messaging;
using InvoicrCoreBusiness.Utility;
using InvoicrInfrastructure.Email;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoicrApp.Messaging
{
	// Create a handler for the invoice generation failure event
	public class InvoiceGenerationFailedNotificationHandler(ILogger<InvoiceGenerationFailedNotificationHandler> logger, IEmailSender emailSender, AppConfiguration configuration) : INotificationHandler<InvoiceGenerationFailedNotification>
	{
		private readonly ILogger<InvoiceGenerationFailedNotificationHandler> _logger = logger;
		private readonly AppConfiguration _configuration = configuration;

		public IEmailSender _emailSender = emailSender;
		public Task Handle(InvoiceGenerationFailedNotification notification, CancellationToken cancellationToken)
		{
			// Handle the failure event here
			_logger.LogError($"INVOICE GENERATION FAILED. Source:{notification.Source}, error message: {notification.ErrorMessage}");
			// Send email notification
			_emailSender.SendEmailAsync(_configuration.Email_To, 
				                        _configuration.Email_From, 
										"Invoice Generation Failed!", 
										$"Source:{notification.Source} {Environment.NewLine} Error Message:{notification.ErrorMessage}");
			
			return Task.CompletedTask;
		}
	}
}
