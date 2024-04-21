using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Extensions;
using InvoicrCoreModels.Models.InvoiceEventModels;
using InvoicrCoreModels.Models.InvoiceLineItemModels;
using InvoicrCoreModels.Models.InvoiceModels;
using Xunit;
using Xunit.Abstractions;


namespace InvoicrApp.Tests.InvoiceHandlerTests.TextInvoice
{
	[Collection("Sequential")]
	public class InvoiceHandlerTextInvoiceTest
    {
        
        private readonly InvoiceHandlerTextInvoiceTestFixture _fixture;
        private readonly IInvoiceHandler _handler;
        private readonly FileUtility _fileUtility;


        public InvoiceHandlerTextInvoiceTest(ITestOutputHelper outputHelper)
        {
            _fixture = new(outputHelper); 
			// Build configuration
			var _configuration = new AppConfiguration();
            _handler = _fixture.InvoiceHandler;
            _fileUtility = _fixture.FileUtility;
        }

        [Fact]
        public async Task ProcessEventAsync_ShouldAddRecord()
        {
            // Arrange
            var @event = new InvoiceEvent
            {
                Content = new Invoice
                {
                    InvoiceId = Guid.NewGuid(),
                    InvoiceNumber = "INV-Test-001",
                    Status = "DRAFT",
                    CreatedDateUtc = new DateTime(2021, 3, 22, 19, 15, 0, DateTimeKind.Utc),
                    DueDateUtc = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LineItems = new[]
                    {
                        new InvoiceLineItem { Description = "Xero Supplier", Quantity = 2, UnitCost = 24.25m, LineItemTotalCost = 48.5m },
                        new InvoiceLineItem { Description = "Xero Supplier2", Quantity = 3, UnitCost = 4m, LineItemTotalCost = 12 },
                    }
                }
            };

            var expectedFileOutput =
$@"Invoice Event ID: 0
Invoice Number: INV-Test-001
Status: DRAFT
Created Date: 2021-03-22T19:15:00.0000000Z
Due Date: 2022-01-01T00:00:00.0000000Z


Item description: Xero Supplier
Item quantity: 2
Item cost: 24.25
Item total cost: 48.5


Item description: Xero Supplier2
Item quantity: 3
Item cost: 4
Item total cost: 12


Line Count:2
Invoice Total:60.5
";
            expectedFileOutput = expectedFileOutput.RemoveEmptyLines().Trim();

            // Act
            await _handler.ProcessEventAsync(@event);

            // Assert
            var invoiceFilePath = _fileUtility.GetInvoiceFilePathText(@event.Content.InvoiceId);
            File.Exists(invoiceFilePath).Should().BeTrue();
            var result = (await File.ReadAllTextAsync(invoiceFilePath)).RemoveEmptyLines().Trim();
            result.Should().BeEquivalentTo(expectedFileOutput);
        }
    }
}