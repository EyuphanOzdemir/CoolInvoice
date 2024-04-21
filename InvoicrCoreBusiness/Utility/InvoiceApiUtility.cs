using InvoicrCoreBusiness.InvoiceHandler;
using InvoicrCoreBusiness.InvoicePersistance;

namespace InvoicrCoreBusiness.Utility
{
    public class InvoiceApiUtility(AppConfiguration configuration)
    {
        public string GenerateUri(long lastEventId)
        {
			if (lastEventId > 0)
            {
                return $"{configuration.Base_API_Url}?afterEventId={lastEventId}";
            }

            return configuration.Base_API_Url;
        }
    }
}
