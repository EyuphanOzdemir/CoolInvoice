using InvoicrCoreModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.Utility
{
	public class FileUtility(AppConfiguration configuration)
	{
        private readonly string CurrentDirectory = Directory.GetCurrentDirectory();
		public  string LastProcessedEventFolder => Path.Combine(CurrentDirectory, configuration.Data_Folder);
		public  string LastProcessedEventFilePath => Path.Combine(LastProcessedEventFolder, configuration.Last_Processed_File_Name);
		public  string InvoiceFolder => Path.Combine(CurrentDirectory, configuration.Data_Folder, configuration.Invoice_Folder, DateTime.Today.ToString("ddMMyyy"));
		private  string GetInvoiceFileName(Guid invoiceId, string extension) => $"Invoice-{invoiceId}.{extension}";
		private  string GetInvoiceFileNameText(Guid invoiceId) => GetInvoiceFileName(invoiceId, "txt");
		private  string GetInvoiceFileNamePDF(Guid invoiceId) => GetInvoiceFileName(invoiceId, "pdf");
		public  string GetInvoiceFilePathText(Guid invoiceId) => Path.Combine(InvoiceFolder, GetInvoiceFileNameText(invoiceId));
		public  string GetInvoiceFilePathPDF(Guid invoiceId) => Path.Combine(InvoiceFolder, GetInvoiceFileNamePDF(invoiceId));
	}
}
