using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists("./LOGS"))
                Directory.CreateDirectory("./LOGS");
            
            var log = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.File("./LOGS/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

#if DEBUG
            Service1 s = new Service1(log);
            MainServerLoop server = new MainServerLoop(log);
            Console.ReadKey();
#else
                ServiceBase[] ServicesToRun;
                ServicesToRun=new ServiceBase[] 
                { 
                    new Service1(log) 
                };
                ServiceBase.Run(ServicesToRun);
#endif

            Log.CloseAndFlush();
        }
    }
}
