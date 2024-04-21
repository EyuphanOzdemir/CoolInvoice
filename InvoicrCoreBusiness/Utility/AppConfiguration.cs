using Microsoft.Extensions.Configuration;

namespace InvoicrCoreBusiness.Utility
{
	public sealed class AppConfiguration
	{
		public string Data_Folder { get; set; }
		public string Last_Processed_File_Name { get; set; }
		public string Invoice_Folder { get; set; }

		public string Base_API_Url { get; set; }
		public string Prince_Exe_Path { get; set; }
		public string Default_Connection_String { get; set; }
        public string SMTP_Server { get; set; }

        public string Email_To { get; set; }
        public string Email_From { get; set; }
        public int SMTP_port { get; set; }
        public int Worker_Frequency { get; set; }

        public AppConfiguration()
        {
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			config.GetSection("AppConfiguration").Bind(this);
			Default_Connection_String = config.GetConnectionString("DefaultConnection");
		}

	}
}