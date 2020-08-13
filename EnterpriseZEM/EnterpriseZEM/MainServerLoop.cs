using EnterpriseZEM_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnterpriseZEM_Common.Settings;

namespace EnterpriseZEM
{
    class MainServerLoop
    {
        Server _server;

        public MainServerLoop(Serilog.Core.Logger log)
        {
            if (!File.Exists("./Settings.ini"))
            {
                Console.WriteLine("Nie znaleziono pliku Settings.ini, zostanie on wygenerowany automatycznie.");
                log.Warning("Couldn't find Settings.ini, generating file...");
                using (StreamWriter sw = new StreamWriter("./Settings.ini"))
                {
                    sw.WriteLine("ServerAddress: 127.0.0.1");
                    sw.WriteLine("ServerPort: 7000");
                    sw.WriteLine("ConnectionString: ");
                }
                log.Warning("Settings.ini created.");
            }
            if(File.Exists("./Settings.ini"))
            {
                log.Information("Loading settings...");
                using (StreamReader sr = new StreamReader("./Settings.ini"))
                {
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            string[] entries = sr.ReadLine().Split(':');
                            Settings.Properties.Add(entries[0].Trim(), entries[1].Trim());
                        }
                        catch(Exception ex)
                        {
                            log.Error($"{ex.Message}, {ex.InnerException}");
                            return;
                        }
                    }
                }
            }

            _server = new Server(int.Parse(Settings.Properties[FieldTypes.ServerPort.ToString()]), Settings.Properties[FieldTypes.ServerAddress.ToString()], log);
            _server.StartListening();
        }
    }
}
