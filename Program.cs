using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMS_Sender.Services;

namespace SMS_Sender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            //CreateDatabaseIfNotExist(host);

            host.Run();
        }

        //private static void CreateDatabaseIfNotExist(IHost host)
        //{
        //    using var scope = host.Services.CreateScope();
        //    var services = scope.ServiceProvider;

        //    var context = services.GetRequiredService<AppDbContext>();
        //    context.Database.EnsureCreated();

        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    AppSettings.Configuration = configuration;
                    AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection");
                    AppSettings.ApiKey = configuration.GetSection("Api:ApiKey").Value;
                    AppSettings.ApiSecret = configuration.GetSection("Api:ApiSecret").Value;

                    var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
                    optionBuilder.UseSqlServer(AppSettings.ConnectionString);

                    services.AddScoped<AppDbContext>(d => new AppDbContext(optionBuilder.Options));

                    services.AddHostedService<Worker>();
                });
    }
}
