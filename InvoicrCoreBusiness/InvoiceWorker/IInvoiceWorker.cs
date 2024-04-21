using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.InvoiceWorker
{
	public interface IInvoiceWorker
	{
		Task ExecuteAsync(CancellationToken stoppingToken);
        Task ExecuteSingleAsync(CancellationToken stoppingToken);
    }
}
