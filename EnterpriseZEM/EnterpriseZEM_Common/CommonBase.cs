using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EnterpriseZEM_Common
{
    public class CommonBase
    {
        public int PORT;
        public string SERVER_IP;
        public const string dataStartToken = "<TRANSMISSION_START>";
        public const string dataEndToken = "<TRANSMISSION_END>";
        public const string pingToken = "<KEEPALIVE_PING>";
        public const string ackToken = "<MESSAGE_ACK>";
        public const string endConnectionToken = "<CLOSE_CONNECTION>";

        public const int bytesOfData = 4;

        public static byte[] Serialize<T>(T packet, out byte[] dataLen)
        {
            byte[] userDataBytes;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, packet);
            userDataBytes = ms.ToArray();
            dataLen = BitConverter.GetBytes((Int32)userDataBytes.Length);

            return userDataBytes;
        }

        public static T Deserialize<T>(byte[] readMsgBytes)
        {
            MemoryStream ms = new MemoryStream(readMsgBytes);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            object rawObj = bf1.Deserialize(ms);
            return (T)rawObj;
        }
    }

    public class EventLogger
    { 
    
    }
}
