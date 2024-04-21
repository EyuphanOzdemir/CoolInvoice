using AutoMapper;
using Elfie.Serialization;
using InvoicrCoreBusiness.Messaging;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models.InvoiceEventModels;
using InvoicrInfrastructure.JSON2PDF;
using MediatR;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace InvoicrCoreBusiness.InvoiceGenerator
{
    public class InvoiceGeneratorPDF(IJSON2PDF json2pdf, ILogger<InvoiceGeneratorPDF> logger, IMapper mapper, FileUtility fileUtility, IMediator mediator) : IInvoiceGenerator
	{
        private IJSON2PDF JSON2PDF { get; set; } = json2pdf;
        private readonly  ILogger<InvoiceGeneratorPDF> _logger = logger;
        private readonly  IMapper _mapper = mapper;
        private readonly FileUtility _fileUtility = fileUtility;

        public event InvoiceGenerationFailedHandlerDelegate InvoiceGenerationFailed;

		public Task<string> SaveInvoice(InvoiceEvent invoiceEvent)
		{
			Directory.CreateDirectory(_fileUtility.InvoiceFolder);
			string json = JsonConvert.SerializeObject(_mapper.Map<InvoiceEventDto>(invoiceEvent));
			string pdfPath = _fileUtility.GetInvoiceFilePathPDF(invoiceEvent.Content.InvoiceId);
			var result = JSON2PDF.GeneratePDF<InvoiceEvent>(json, pdfPath); //change pdfPath to something faulty like "pdfPath\\xxx" here to see the failed PDF generation emails

            if (!string.IsNullOrEmpty(result)) //invoice generation failed
			{
				var errorMessage = $"Error in writing invoice PDF: {result}";
				var source = this.GetType().ToString();
				//log the error
				_logger.LogError(errorMessage);
				//if there is an event defined, call the event handler
				InvoiceGenerationFailed?.Invoke(this, new InvoiceGenerationFailedEventArgs(source, result));
				//publish mediatr message so that any handler handle this message (InvoicrApp will send an email for example)
				mediator.Publish(new InvoiceGenerationFailedNotification(source, errorMessage));
			}

			return Task.FromResult(result);
		}
	}
}
