using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Autofac;
using AutoMapper;
using InvoicrApp.Tests.Logger;
using InvoicrCoreBusiness.HttpService;
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
using InvoicrInfrastructure.JSONSerializer;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace InvoicrApp.Tests.InvoiceGeneratorExceptionTests
{
    public class InvoiceWorkerTextInvoiceTestFixtureException : IDisposable
    {
        public IContainer Container { get; private set; }
        public FileUtility FileUtility { get; private set; }
        public IInvoiceGenerator InvoiceGeneratorException { get; private set; }

		public IInvoiceWorker InvoiceWorker { get; private set; }

		//public EFRepository<InvoiceEventLastProcessedEvent> InvoiceEventLastProcessedRepository { get; private set; }

		public InvoiceWorkerTextInvoiceTestFixtureException()
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
			builder.RegisterType<InvoicePersistanceFile>().As<IInvoicePersistance>().InstancePerLifetimeScope();
			builder.RegisterType<HttpServiceDefault>().As<IHttpService>().InstancePerLifetimeScope();
			builder.RegisterType<InvoiceEventProviderMemory>().As<IInvoiceEventProvider>().InstancePerLifetimeScope();
			builder.RegisterType<InvoiceGeneratorException>().As<IInvoiceGenerator>().InstancePerLifetimeScope();
			builder.RegisterType<InvoiceHandler>().As<IInvoiceHandler>().InstancePerLifetimeScope();
			builder.RegisterType<InvoiceWorker>().As<IInvoiceWorker>().InstancePerLifetimeScope();

			// Build Autofac container
			Container = builder.Build();
			FileUtility = Container.Resolve<FileUtility>();
			InvoiceWorker = Container.Resolve<IInvoiceWorker>();
			InvoiceGeneratorException = Container.Resolve<IInvoiceGenerator>();
            InvoiceGeneratorException.InvoiceGenerationFailed += (sender, args) =>
			{
				//to test the standard event handling
				(string source, string errorMessage) = args;
                File.WriteAllText(Path.Combine(FileUtility.InvoiceFolder, TestParams.EXCEPTION_EVENT_FILE_NAME), String.Join(Environment.NewLine, source, errorMessage));
			};
		}

        public void Dispose()
        {
            // Dispose the container at the end of tests
            Container.Dispose();
        }
    }
}
