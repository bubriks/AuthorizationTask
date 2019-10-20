using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PatiroApp.DataManagers;
using PatiroApp.Models;
using System.Collections.Generic;

namespace PatiroApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Seed.SeedData();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://localhost:4000");
                });

    }
}
