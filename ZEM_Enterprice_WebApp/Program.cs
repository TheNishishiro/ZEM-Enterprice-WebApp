using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace ZEM_Enterprice_WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            if (!Directory.Exists("./LOGS/"))
                Directory.CreateDirectory("./LOGS/");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile("./LOGS/Log_{Date}.txt", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureHostConfiguration(app =>
                {
                    app.SetBasePath(Directory.GetCurrentDirectory());
                    app.AddJsonFile("hostsettings.json", optional: true);
                    app.AddEnvironmentVariables(prefix: "ZEM_");
                    app.AddCommandLine(args);
                }).UseSerilog().UseWindowsService();

        
    }
}
