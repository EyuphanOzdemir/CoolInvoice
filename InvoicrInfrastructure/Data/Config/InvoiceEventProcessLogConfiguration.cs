using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicrInfrastructure.Data.Config
{
    public class InvoiceEventProcessLogConfiguration : IEntityTypeConfiguration<InvoiceEventLastProcessedEvent>
    {
        // Implementation of the IEntityTypeConfiguration interface
        public void Configure(EntityTypeBuilder<InvoiceEventLastProcessedEvent> builder)
        {
            // Configure the main table for patients
            builder.ToTable("InvoiceEventLastProcessedEvent").HasKey(k => k.Id);

            //Default value for CreatedDateUtc
            builder.Property(e => e.CreatedDateUtc).HasDefaultValueSql("getdate()");
		}
    }
}
