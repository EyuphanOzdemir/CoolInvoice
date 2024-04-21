using InvoicrCoreBusiness.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoicePersistance
{
	public class InvoicePersistanceFile(FileUtility fileUtility) : IInvoicePersistance
	{
		private readonly FileUtility _fileUtility = fileUtility;
        public async Task<long> GetLastInvoiceEventIdAsync()
		{
			if (File.Exists(_fileUtility.LastProcessedEventFilePath) && long.TryParse(await File.ReadAllTextAsync(_fileUtility.LastProcessedEventFilePath), out var eventId))
			{
				return eventId;
			}
			return 0;
		}
		public async Task SaveLastProcessedEventAsync(long eventId)
		{
			Directory.CreateDirectory(_fileUtility.LastProcessedEventFolder);
			await File.WriteAllTextAsync(_fileUtility.LastProcessedEventFilePath, eventId.ToString());
		}
	}
}
