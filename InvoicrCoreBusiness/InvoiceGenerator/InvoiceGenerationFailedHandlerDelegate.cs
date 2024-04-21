using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceGenerator
{
	public class InvoiceGenerationFailedEventArgs(string source, string errorMessage)
	{
		public string Source { get; } = source;
		public string ErrorMessage { get; } = errorMessage;

		public void Deconstruct(out string source, out string errorMessage)
		{
			source = Source;
			errorMessage = ErrorMessage;
		}
	}
	public delegate void InvoiceGenerationFailedHandlerDelegate(object sender, InvoiceGenerationFailedEventArgs e);
}
