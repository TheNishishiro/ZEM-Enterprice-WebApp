using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM
{
    public partial class Service1 : ServiceBase
    {
        Serilog.Core.Logger _log;

        public Service1(Serilog.Core.Logger log)
        {
            InitializeComponent();
            _log = log;
        }

        protected override void OnStart(string[] args)
        {
            MainServerLoop MSL = new MainServerLoop(_log); 
        }

        protected override void OnStop()
        {
        }
    }
}
