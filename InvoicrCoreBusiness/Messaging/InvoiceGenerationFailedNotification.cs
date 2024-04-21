using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.Messaging
{
	// Define a MediatR request for handling the invoice generation failure event
	public class InvoiceGenerationFailedNotification(string source, string errorMessage) : INotification
	{
		public string Source { get; } = source;
		public string ErrorMessage { get; } = errorMessage;
	}
}
