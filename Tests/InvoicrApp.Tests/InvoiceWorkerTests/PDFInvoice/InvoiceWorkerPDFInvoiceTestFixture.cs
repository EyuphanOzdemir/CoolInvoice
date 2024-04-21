using System;
using System.Net.Http;
using System.Text.Json;
using Autofac;
using Autofac.Core;
using AutoMapper;
using InvoicrCoreBusiness.InvoiceEventProvider;
using InvoicrCoreBusiness.InvoiceGenerator;
using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.InvoicePersistance;
using InvoicrCoreBusiness.InvoiceWorker;
using InvoicrCoreBusiness.Utility;
using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using InvoicrInfrastructure.Data;
using InvoicrInfrastructure.Email;
using InvoicrInfrastructure.Json2PDF;
using InvoicrInfrastructure.JSON2PDF;
using Microsoft.Extensions.Logging;
using InvoicrCoreBusiness.HttpService;
using InvoicrInfrastructure.JSONSerializer;
using InvoicrApp.Tests.Logger;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;


namespace InvoicrApp.Tests.InvoiceHandlerTests.PDFInvoice
{
    public class InvoiceWorkerPDFInvoiceTestFixture : IDisposable
    {
        public IContainer Container { get; private set; }
        public IInvoiceWorker InvoiceWorker { get; private set; }
        public EFRepository<InvoiceEventLastProcessedEvent> InvoiceEventLastProcessedRepository { get; private set; }
        public FileUtility FileUtility { get; private set; }

        public InvoiceWorkerPDFInvoiceTestFixture()
        {
            // Configure Autofac DI container
            var builder = new ContainerBuilder();

            //----AppConfiguration-----
            var _configuration = new AppConfiguration();
            builder.RegisterInstance(_configuration).SingleInstance();

            //-----AutoMapper -----
            builder.Register(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<InvoicrCoreBusiness.Mappings.InvoiceMappingProfile>(); // Register mapping profile here
                });
                return config.CreateMapper();
            }).As<IMapper>().InstancePerLifetimeScope();

            //-----Logger-----
            //Create a LoggerFactory instance
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                // Add the custom logger provider
                builder.AddProvider(new MockLoggerProvider());
            });
            // Create a logger from the LoggerFactory
            var _generator_pdf_logger = loggerFactory.CreateLogger<InvoiceGeneratorPDF>();
            builder.RegisterInstance(_generator_pdf_logger).As<ILogger<InvoiceGeneratorPDF>>();
            var _worker_logger = loggerFactory.CreateLogger<InvoiceWorker>();
            builder.RegisterInstance(_worker_logger).As<ILogger<InvoiceWorker>>();


			// Register JSON serializer options with Autofac container
			builder.RegisterInstance(JsonSerializerOptionsDefault.GetOptions()).As<JsonSerializerOptions>().SingleInstance();


            ////-----HTTPClient-----
            builder.Register(c => new HttpClient()).AsSelf().InstancePerLifetimeScope();

            //-----AppDBContext-----
            builder.RegisterType<AppDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();


			//-----Mediatr-----
			var configuration = MediatRConfigurationBuilder
				.Create(GetType().Assembly)
				.WithAllOpenGenericHandlerTypesRegistered()
				.WithRegistrationScope(RegistrationScope.Scoped)
				.Build();

			builder.RegisterMediatR(configuration);

            //-----Others-----
			builder.RegisterType<FileUtility>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceApiUtility>().InstancePerLifetimeScope();
            builder.RegisterInstance(new EmailSender(new EmailConfig(_configuration.SMTP_Server, _configuration.SMTP_port))).As<IEmailSender>().SingleInstance();
            builder.RegisterType<EFRepository<InvoiceEventLastProcessedEvent>>().InstancePerLifetimeScope();
            builder.RegisterType<InvoicePersistanceDB>().As<IInvoicePersistance>().InstancePerLifetimeScope();
            builder.RegisterType<HttpServiceDefault>().As<IHttpService>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceEventProviderWeb>().As<IInvoiceEventProvider>().InstancePerLifetimeScope();
            builder.RegisterInstance(new JSON2PDFPrince(_configuration.Prince_Exe_Path)).As<IJSON2PDF>().SingleInstance();
            builder.RegisterType<InvoiceGeneratorPDF>().As<IInvoiceGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceHandler>().As<IInvoiceHandler>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceWorker>().As<IInvoiceWorker>().InstancePerLifetimeScope();
            
            // Build Autofac container
            Container = builder.Build();
            // Resolve InvoiceHandler from Autofac container
            InvoiceWorker = Container.Resolve<IInvoiceWorker>();
            InvoiceEventLastProcessedRepository = Container.Resolve<EFRepository<InvoiceEventLastProcessedEvent>>();
            FileUtility = Container.Resolve<FileUtility>();
        }

        // Dispose the Autofac container when the application is disposed
        public void Dispose() 
        { 
          Container.Dispose();  // Dispose the container at the end of tests. Will be automatically called.
        } 

    }
}
