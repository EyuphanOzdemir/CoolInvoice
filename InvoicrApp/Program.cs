using System;
using System.Threading;
using System.Threading.Tasks;
using InvoicrCoreBusiness.InvoiceEventProvider;
using InvoicrCoreBusiness.InvoiceGenerator;
using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.InvoicePersistance;
using InvoicrCoreBusiness.InvoiceWorker;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Logging;
using InvoicrInfrastructure.Logger.SeriLogger;
using InvoicrInfrastructure.Data;
using InvoicrCoreBusiness.Utility;
using InvoicrInfrastructure.JSON2PDF;
using InvoicrInfrastructure.Json2PDF;
using InvoicrApp.Messaging;
using MediatR;
using System.Reflection;
using InvoicrInfrastructure.Email;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using System.Text.Json.Serialization;
using System.Text.Json;
using InvoicrCoreBusiness.HttpService;
using InvoicrInfrastructure.JSONSerializer;

namespace InvoicrApp
{
    public class Program
    {
		public static async Task Main(string[] args)
        {
			// Build configuration
			var _configuration = new InvoicrCoreBusiness.Utility.AppConfiguration();

            var _serviceProvider = new ServiceCollection().
            AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(SerilogConfiguration.GetSeriLogger());
            })
            .AddSingleton(JsonSerializerOptionsDefault.GetOptions())
            .AddHttpClient()
            .AddSingleton<AppConfiguration>(_configuration)
            .AddSingleton<FileUtility>()
            .AddSingleton<InvoiceApiUtility>()
            .AddSingleton<IEmailSender>(new EmailSender(new EmailConfig(_configuration.SMTP_Server, _configuration.SMTP_port)))
            .AddSingleton<IJSON2PDF>(new JSON2PDFPrince(_configuration.Prince_Exe_Path))
            .AddSingleton<IInvoiceHandler, InvoiceHandler>()
            .AddSingleton<InvoiceWorker>()
            .AddSingleton<EFRepository<InvoiceEventLastProcessedEvent>>()
            .AddSingleton<IInvoicePersistance, InvoicePersistanceDB>()
            .AddSingleton<IInvoiceGenerator, InvoiceGeneratorPDF>()
            .AddSingleton<IHttpService, HttpServiceDefault>()
            .AddSingleton<IInvoiceEventProvider, InvoiceEventProviderWeb>()
            .AddSingleton<IInvoiceWorker, InvoiceWorker>()
            .AddDbContext<AppDbContext>()
            .AddMediatR(config => { config.Lifetime = ServiceLifetime.Scoped; config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); })
            .AddAutoMapper(typeof(InvoicrCoreBusiness.Mappings.InvoiceMappingProfile).Assembly)
			.BuildServiceProvider();

            try
			{
                //todo: stop the background service by pressing any key
                Console.WriteLine("Press any key to stop the background task...");
                
                using var cancellationTokenSource = new CancellationTokenSource();

				// Start the background task
				Task backgroundTask = _serviceProvider.GetService<IInvoiceWorker>().ExecuteAsync(cancellationTokenSource.Token);

				// Wait for user input to cancel the task
				Console.ReadKey();

                // Cancel the task
                cancellationTokenSource.Cancel();

                // Wait for the background task to complete
                await backgroundTask;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"The program ended with an exception:{exception.Message}");
            }
        }
    }
}
