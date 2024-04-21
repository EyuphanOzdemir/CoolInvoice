using System;
using Autofac;
using AutoMapper;
using InvoicrApp.Tests.Logger;
using InvoicrCoreBusiness.InvoiceEventProvider;
using InvoicrCoreBusiness.InvoiceGenerator;
using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.InvoicePersistance;
using InvoicrCoreBusiness.Utility;
using InvoicrInfrastructure.Email;
using InvoicrInfrastructure.Logger.XUnitLogger;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;


namespace InvoicrApp.Tests.InvoiceHandlerTests.TextInvoice
{
    public class InvoiceHandlerTextInvoiceTestFixture : IDisposable
    {
		public IContainer Container { get; private set; }
        public IInvoiceHandler InvoiceHandler { get; private set; }
        public FileUtility FileUtility { get; private set; }

        public InvoiceHandlerTextInvoiceTestFixture(ITestOutputHelper outputHelper)
		{
			// Configure Autofac DI container
			var builder = new ContainerBuilder();

            //----AppConfiguration-----
            var _configuration = new AppConfiguration();
            builder.RegisterInstance(_configuration).SingleInstance();

			//-----Logger------
            //XUnitLogger to see some logs in the test tool
            builder.Register(context =>
			{
				var loggerProvider = new XUnitLoggerProvider(outputHelper);
				var logger = loggerProvider.CreateLogger("TextInvoice");
				return new XUnitLoggerWrapper<InvoiceGeneratorTextFile>(logger);
			}).As<ILogger<InvoiceGeneratorTextFile>>()
			  .SingleInstance();

			//-----AutoMapper -----
			builder.Register(context =>
			{
				var config = new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<InvoicrCoreBusiness.Mappings.InvoiceMappingProfile>(); // Register mapping profile here
				});
				return config.CreateMapper();
			}).As<IMapper>().InstancePerLifetimeScope();

			builder.RegisterType<FileUtility>().SingleInstance();
            builder.RegisterType<InvoiceApiUtility>().SingleInstance();
            builder.RegisterInstance(new EmailSender(new EmailConfig(_configuration.SMTP_Server, _configuration.SMTP_port))).As<IEmailSender>().SingleInstance();
            builder.RegisterType<InvoicePersistanceFile>().As<IInvoicePersistance>().SingleInstance();
            builder.RegisterType<InvoiceEventProviderWeb>().As<IInvoiceEventProvider>().SingleInstance();
            builder.RegisterType<InvoiceGeneratorTextFile>().As<IInvoiceGenerator>().SingleInstance();
            builder.RegisterType<InvoiceHandler>().As<IInvoiceHandler>().SingleInstance();

            // Build Autofac container
            Container = builder.Build();
            // Resolve InvoiceHandler from Autofac container
            InvoiceHandler = Container.Resolve<IInvoiceHandler>();
            FileUtility = Container.Resolve<FileUtility>();
        }

        public void Dispose()
        {
            // Dispose the container at the end of tests
            Container.Dispose();
        }
    }
}
