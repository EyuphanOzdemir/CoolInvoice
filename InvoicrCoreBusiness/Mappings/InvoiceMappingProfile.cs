using AutoMapper;
using InvoicrCoreModels.Models.InvoiceModels;
using InvoicrCoreModels.Models.InvoiceEventModels;
using InvoicrCoreModels.Models.InvoiceLineItemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreBusiness.Mappings
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            CreateMap<InvoiceLineItem, InvoiceLineItemDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceEvent, InvoiceEventDto>();
        }
    }
}
