using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using InvoicrApp.Tests.InvoiceHandlerTests.PDFInvoice;
using InvoicrApp.Tests.InvoiceWorkerTests.PDFInvoice;
using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.InvoiceWorker;
using InvoicrCoreBusiness.Specifications.InvoiceEventProcessLogSpecifications;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Extensions;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using InvoicrCoreModels.Models.InvoiceEventModels;
using InvoicrCoreModels.Models.InvoiceLineItemModels;
using InvoicrCoreModels.Models.InvoiceModels;
using InvoicrInfrastructure.Data;
using Xunit;


namespace InvoicrApp.Tests.InvoiceGeneratorExceptionTests
{
	public class InvoiceWorkerTextInvoiceTestException(InvoiceWorkerTextInvoiceTestFixtureException fixture) : IClassFixture<InvoiceWorkerTextInvoiceTestFixtureException>
	{
		private readonly InvoiceWorkerTextInvoiceTestFixtureException _fixture = fixture;
		private readonly IInvoiceWorker _worker = fixture.InvoiceWorker;
		//private readonly EFRepository<InvoiceEventLastProcessedEvent> _invoiceEventLastProcessedRepository = fixture.InvoiceEventLastProcessedRepository;
		private readonly FileUtility _fileUtility = fixture.FileUtility;

		[Fact]
		public async Task ExecuteSinleAsync_ShouldHandleException()
		{
			//reset the last invoice processed id to 0
			//if (await _invoiceEventLastProcessedRepository.CountAsync() > 0)
			//	try
			//	{
			//		_invoiceEventLastProcessedRepository.DeleteRangeAsync(new InvoiceEventProcessLogAll()).GetAwaiter().GetResult();
			//	}catch { }

			var exceptionEventFilePath = Path.Combine(_fileUtility.InvoiceFolder, TestParams.EXCEPTION_EVENT_FILE_NAME);
			var exceptionMediatrFilePath = Path.Combine(_fileUtility.InvoiceFolder, TestParams.EXCEPTION_MEDIATR_FILE_NAME);

			await _worker.ExecuteSingleAsync(System.Threading.CancellationToken.None);
			//test standard event handling	
			File.Exists(exceptionEventFilePath).Should().BeTrue();
			var exceptionEventContent = File.ReadAllText(exceptionEventFilePath);
			exceptionEventContent.Contains(TestParams.EXCEPTION_EVENT_FILE_CONTENT).Should().BeTrue();

			//test mediart event handling	
			File.Exists(exceptionMediatrFilePath).Should().BeTrue();
			var exceptionMediatrContent = File.ReadAllText(exceptionMediatrFilePath);
			exceptionMediatrContent.Contains(TestParams.EXCEPTION_MEDIATR_FILE_CONTENT).Should().BeTrue();
		}
	}
}
