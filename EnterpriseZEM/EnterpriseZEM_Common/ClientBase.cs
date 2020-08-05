using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace EnterpriseZEM_Common
{
    public class ClientBase : CommonBase
    {
        private TcpClient client;
        private bool isConnected;
        private object sendLock;
        private int tickRate = 40000;

        private Thread keepAliveThread;
        private readonly ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        public ClientBase(int port = 5000, string ip = "127.0.0.1") : base()
        {
            PORT = port;
            SERVER_IP = ip;
            isConnected = false;
            sendLock = new object();
        }

        public void Connect()
        {
            client = new TcpClient(SERVER_IP, PORT);

            isConnected = true;

            keepAliveThread = new Thread(KeepAlive);
            keepAliveThread.Start();
        }

        private void SendMsg(CustomPacket packet, out NetworkStream nwStream)
        {
            nwStream = client.GetStream();
            byte[] bytesToSend = Serialize<CustomPacket>(packet, out byte[] len);

            nwStream.Write(len, 0, bytesOfData);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        public void Disconnect()
        {
            lock (sendLock)
            {
                CustomPacket packet = new CustomPacket(FlagType.endConnection, HeaderTypes.basic, endConnectionToken, new List<string>(), null);
                SendMsg(packet, out NetworkStream nwStream);
                _shutdownEvent.Set();
                isConnected = false;
                client.Close();
            }
        }

        /// <summary>
        /// Sends packet to the server and returns recieved feedback
        /// </summary>
        /// <param name="packet">packet to send</param>
        /// <returns></returns>
        public CustomPacket SendReceiveMessage(CustomPacket packet)
        {
            lock (sendLock)
            {
                SendMsg(packet, out NetworkStream nwStream);
                return ReadLong(nwStream);
            }
        }

        /// <summary>
        /// Thread sending pings to the server
        /// </summary>
        private void KeepAlive()
        {
            while (isConnected)
            {
                lock (sendLock)
                {
                    CustomPacket packet = new CustomPacket(FlagType.ping, HeaderTypes.basic, "ping", null, null);
                    SendMsg(packet, out NetworkStream nwStream);
                }
                if (_shutdownEvent.WaitOne(tickRate))
                    break;
            }
        }

        /// <summary>
        /// Read incoming message from the server
        /// </summary>
        /// <param name="nwStream"></param>
        /// <returns></returns>
        protected CustomPacket ReadLong(NetworkStream nwStream)
        {
            byte[] msgLenBytes = new byte[bytesOfData];
            nwStream.Read(msgLenBytes, 0, bytesOfData);

            int dataLen = BitConverter.ToInt32(msgLenBytes, 0);
            byte[] readMsgData = new byte[dataLen];

            int dataRead = 0;
            do
            {
                dataRead += nwStream.Read(readMsgData, dataRead, dataLen - dataRead);
            } while (dataRead < dataLen);

            return Deserialize<CustomPacket>(readMsgData);
        }
    }
}
