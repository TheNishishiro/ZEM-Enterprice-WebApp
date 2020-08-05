using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EnterpriseZEM_Common
{
    public class ServerBase : CommonBase
    {
        private IPAddress localAdd;
        private TcpListener listener;

        private Thread ListenForConThread;
        private Thread clientThread;

        private object _lock;

        public ServerBase(int port = 5000, string ip = "127.0.0.1") : base()
        {
            PORT = port;
            SERVER_IP = ip;

            _lock = new object();

            localAdd = IPAddress.Parse(SERVER_IP);
            listener = new TcpListener(localAdd, PORT);
        }

        public void StartListening()
        {
            if (ListenForConThread == null)
            {
                ListenForConThread = new Thread(ListenForConnections);
                ListenForConThread.Start();
            }
        }

        private void ServeClient(TcpClient client)
        {
            while (true)
            {
                try
                {
                    NetworkStream nwStream = client.GetStream();
                    nwStream.ReadTimeout = 120000;

                    byte[] readMsgLen = new byte[bytesOfData];
                    nwStream.Read(readMsgLen, 0, bytesOfData);
                    int dataLen = BitConverter.ToInt32(readMsgLen, 0);

                    byte[] buffer = new byte[dataLen];

                    CustomPacket receivedPacket = ReadLong(buffer, dataLen, nwStream);

                    if (receivedPacket.Flag == FlagType.ping)
                        continue;
                    else if (receivedPacket.Flag == FlagType.endConnection)
                    {
                        onConnectionClose(client.Client.RemoteEndPoint.ToString());
                        client.Close();
                        return;
                    }
                    //else if (dataReceived == "")
                    //    break;

                    CustomPacket packetToSend = Commands(receivedPacket, client);

                    byte[] bytesToSend = Serialize<CustomPacket>(packetToSend, out byte[] dataToSendLen);

                    nwStream.Write(dataToSendLen, 0, bytesOfData);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch (System.IO.IOException ex)
                {
                    onConnectionLost(client.Client.RemoteEndPoint.ToString(), ex);
                    client.Close();
                    return;
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    onConnectionLost(client.Client.RemoteEndPoint.ToString(), ex);
                    client.Close();
                    return;
                }
                catch (Exception ex)
                {
                    onConnectionLost(client.Client.RemoteEndPoint.ToString(), ex);
                    client.Close();
                    return;
                }
            }
        }

        protected CustomPacket ReadLong(byte[] buffer, int length, NetworkStream nwStream)
        {
            int dataRead = 0;
            do
            {
                dataRead += nwStream.Read(buffer, dataRead, length - dataRead);
            } while (dataRead < length);
            
            return Deserialize<CustomPacket>(buffer);
        }

        public virtual CustomPacket Commands(CustomPacket packet, TcpClient client)
        {
            throw new Exception("Commands override not implemented");
        }

        private void ListenForConnections()
        {
            onServerListen(SERVER_IP, PORT);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                clientThread = new Thread(() => ServeClient(client));
                clientThread.Start();
                onClientConnect(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            }
        }

        public virtual void onClientConnect(string clientAdress) 
        {
            Console.WriteLine($"New connection {clientAdress}");
        }

        public virtual void onServerListen(string serverIP, int serverPort) 
        {
            Console.WriteLine($"Listening for connections on {serverIP}:{serverPort}");
        }

        public virtual void onConnectionClose(string clientAdress)
        {
            Console.WriteLine($"{clientAdress}: Connection closed");
        }

        public virtual void onConnectionLost(string clientAdress, Exception exception)
        {
            Console.WriteLine($"Client {clientAdress} forcefully disconnected. Reason: {exception.Message}, {exception.InnerException}");
        }
    }
}
