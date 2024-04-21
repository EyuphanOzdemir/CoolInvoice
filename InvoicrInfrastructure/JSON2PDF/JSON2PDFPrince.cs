using HandlebarsDotNet;
using InvoicrInfrastructure.JSON2PDF;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrInfrastructure.Json2PDF
{
	public class JSON2PDFPrince(string princeExePath) : IJSON2PDF
	{
		public string PrinceExePath { get; set; } = princeExePath;

		public string GeneratePDF<T>(string json, string pdfFilePath, string templateFilePath = "template.hbs", string logFileName = "PrinceLog.txt", string tempHTMLFileName = "temp.html", bool deleteTempHTMLFile = true)
		{
			try
			{
				// Compile Handlebars template
				var template = Handlebars.Compile(File.ReadAllText(templateFilePath));

				// Parse JSON data into dynamic object
				dynamic jsonDataObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

				// Render the template with JSON data
				var htmlContent = template(jsonDataObject);

				// Write HTML content to temporary file
				File.WriteAllText(tempHTMLFileName, htmlContent);

				// Generate PDF from HTML content using Prince XML .NET Wrapper
				var princeDocument = new Prince(PrinceExePath);
				princeDocument.SetHTML(true);
				princeDocument.SetLog(logFileName);

				// Convert HTML to PDF and write to file
				if (!princeDocument.Convert(tempHTMLFileName, pdfFilePath))
					return $"Prince conversion failure. Please look at the {logFileName}";

				// Cleanup: Delete temporary HTML file
				if (deleteTempHTMLFile)
					File.Delete(tempHTMLFileName);
			}
			catch (Exception exception)
			{
				return exception.Message;
			}
			return String.Empty;
		}
	}
}