using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrInfrastructure.JSON2PDF
{
	public interface IJSON2PDF
	{
		string PrinceExePath { get; set; }
		string GeneratePDF<T>(string json, string pdfFilePath, string templateFilePath = "template.hbs", string logFileName = "PrinceLog.txt", string tempHTMLFileName = "temp.html", bool deleteTempHTMLFile = true);
	}
}
