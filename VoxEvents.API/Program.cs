using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VoxEvents.API.Entities;

namespace VoxEvents.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            MigrateDatabase(host);
            host.Run();
            BuildWebHost(args).Run();
        }

        public static void MigrateDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VoxEventsContext>();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
    }
}
