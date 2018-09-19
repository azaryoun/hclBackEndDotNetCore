using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace derNotrneldesDiensts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                    .UseUrls("http://localhost:8085")
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .ConfigureLogging(f => f.AddConsole(LogLevel.Information))
                    .Build();
            host.Run();



        }
    }
}
