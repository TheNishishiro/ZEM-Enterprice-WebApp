using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace ZEM_Enterprice_WebApp.API
{
    [Route("api/hardware")]
    [ApiController]
    public class HardwareController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<HardwareStatus>> GetHardware()
        {
            HardwareStatus HS = new HardwareStatus();

            Process proc = Process.GetCurrentProcess();
            HS.TotalCPUTime = proc.TotalProcessorTime.TotalSeconds;
            HS.UsedRAM = proc.NonpagedSystemMemorySize64;
            //HS.test = proc.StandardOutput.ReadToEnd();

            return HS;
        }
    }

    public class HardwareStatus
    {
        public int TotalRAM { get; set; }
        public string test { get; set; }
        public long UsedRAM { get; set; }
        public double TotalCPUTime { get; set; }
    }
}
