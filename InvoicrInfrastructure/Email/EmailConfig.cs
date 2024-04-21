using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrInfrastructure.Email
{
    public class EmailConfig(string server, int port)
    {
        public string SMTPServer { get; } = server;
        public int SMTPPort { get; } = port;
    }
}
