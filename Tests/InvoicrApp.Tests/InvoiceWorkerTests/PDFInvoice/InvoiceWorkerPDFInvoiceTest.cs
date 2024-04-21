using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InvoicrApp.Tests.InvoiceHandlerTests.PDFInvoice;
using InvoicrCoreBusiness.InvoiceWorker;
using InvoicrCoreBusiness.Specifications.InvoiceEventProcessLogSpecifications;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Extensions;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using InvoicrInfrastructure.Data;
using Xunit;


namespace InvoicrApp.Tests.InvoiceWorkerTests.PDFInvoice
{
	[Collection("Sequential")]
	public class InvoiceWorkerPDFInvoiceTest(InvoiceWorkerPDFInvoiceTestFixture fixture) : IClassFixture<InvoiceWorkerPDFInvoiceTestFixture>
    {
        private readonly InvoiceWorkerPDFInvoiceTestFixture _fixture = fixture;
        private readonly IInvoiceWorker _worker = fixture.InvoiceWorker;
        private readonly EFRepository<InvoiceEventLastProcessedEvent> _invoiceEventLastProcessedRepository = fixture.InvoiceEventLastProcessedRepository;
        private readonly FileUtility _fileUtility = fixture.FileUtility;

		[Fact]
        public async Task ExecuteSingleAsync_ShouldPDFCountMatch()
        {
			//reset the last invoice processed id
			_invoiceEventLastProcessedRepository.DeleteRangeAsync(new InvoiceEventProcessLogAll()).GetAwaiter().GetResult();

			//delete all the pdf files
			var invoiceDirectory =new DirectoryInfo(_fileUtility.InvoiceFolder);
            var extension = ".pdf";
			invoiceDirectory.DeleteFiles(extension);

            int cycleCount = 2;

            //test cycleCount times
			for (int i = 1; i <= cycleCount; i++)
            {
                await _worker.ExecuteSingleAsync(CancellationToken.None);
            }
            //check PDF count
            invoiceDirectory.CountFiles(extension).Should().Be(cycleCount * TestParams.PDF_COUNT_PER_CYCLE);
		}
    }
}
