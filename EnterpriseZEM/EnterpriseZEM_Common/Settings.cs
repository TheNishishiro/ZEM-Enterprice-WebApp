using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseZEM_Common
{
    public class Settings
    {
        public enum FieldTypes
        {
            ServerAddress,
            ServerPort,
            AuthServerAddress,
            AuthServerPort,
            AuthServerProtocol,
            UsePort
        }

        public static Dictionary<string, string> Properties = new Dictionary<string, string>();
    }
}
