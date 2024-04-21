using AutoMapper;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models.InvoiceEventModels;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceGenerator
{
    public class InvoiceGeneratorTextFile(ILogger<InvoiceGeneratorTextFile> logger, FileUtility fileUtility, IMapper mapper) : IInvoiceGenerator
	{
		private readonly ILogger<InvoiceGeneratorTextFile> _logger = logger;
        private readonly FileUtility _fileUtility = fileUtility;
		private readonly IMapper _mapper = mapper;

		public event InvoiceGenerationFailedHandlerDelegate InvoiceGenerationFailed;

		public async Task<string> SaveInvoice(InvoiceEvent invoiceEvent)
		{
			_logger.LogInformation($"{invoiceEvent.Content.InvoiceNumber} is being saved...");
			var invoiceEventDto = _mapper.Map<InvoiceEventDto>(invoiceEvent);
			
			Directory.CreateDirectory(_fileUtility.InvoiceFolder);

			//create a list of string.
			//the first line will be the header line summarizin invoice number. status, and crated/Due date.
			var lines = new List<string>
			{
				$"Invoice Event ID: {invoiceEventDto.Id}",
				$"Invoice Number: {invoiceEventDto.Content.InvoiceNumber}",
				$"Status: {invoiceEventDto.Content.Status}",
				$"Created Date: {invoiceEventDto.Content.CreatedDateUtc:O}",
				$"Due Date: {invoiceEventDto.Content.DueDateUtc:O}",
				Environment.NewLine
			};

			//Then for each line item we are creating another line and adding all these into the lines collection.
			lines.AddRange(invoiceEventDto.Content.LineItems.SelectMany(li =>
				new[]
				{
					$"Item description: {li.Description}",
					$"Item quantity: {li.Quantity}",
					$"Item cost: {li.UnitCost}",
					$"Item total cost: {li.LineItemTotalCost}",
					Environment.NewLine
				}));

			lines.Add($"Line Count:{invoiceEventDto.Content.LineCount}");
			lines.Add($"Invoice Total:{invoiceEventDto.Content.Total}");

			try
			{
				//throw new Exception("Artificial exception");
				await File.WriteAllLinesAsync(_fileUtility.GetInvoiceFilePathText(invoiceEventDto.Content.InvoiceId), lines);
				return string.Empty;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in writing invoice text: {ex.Message}");
				InvoiceGenerationFailed?.Invoke(this, new InvoiceGenerationFailedEventArgs(this.GetType().ToString(), ex.Message));
				return ex.Message;
			}
		}
	}
}
