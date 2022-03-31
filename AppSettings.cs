using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SMS_Sender
{
    public static class AppSettings
    {
        public static IConfiguration Configuration { get; set; }
        public static string ConnectionString { get; set; }
        public static string ApiKey { get; set; }
        public static string ApiSecret { get; set; }
    }
}
