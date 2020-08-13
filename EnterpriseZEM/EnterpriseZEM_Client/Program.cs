using EnterpriseZEM_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EnterpriseZEM_Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(!File.Exists("./Settings.ini"))
            {
                MessageBox.Show("Nie znaleziono pliku Settings.ini, zostanie on wygenerowany automatycznie.");
                using (StreamWriter sw = new StreamWriter("./Settings.ini"))
                {
                    sw.WriteLine("ServerAddress: 127.0.0.1");
                    sw.WriteLine("ServerPort: 7000");
                    sw.WriteLine("AuthServerAddress: localhost");
                    sw.WriteLine("AuthServerPort: 5000");
                    sw.WriteLine("AuthServerProtocol: http");
                    sw.WriteLine("UsePort: true");
                    sw.WriteLine("UseAuth: true");
                }
            }
            if (File.Exists("./Settings.ini"))
            {
                using (StreamReader sr = new StreamReader("./Settings.ini"))
                {
                    while(!sr.EndOfStream)
                    {
                        string[] entries = sr.ReadLine().Split(':');
                        Settings.Properties.Add(entries[0].Trim(), entries[1].Trim());
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
