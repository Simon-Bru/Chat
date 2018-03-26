using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApplication
{
    class Program
    {
        private List<TcpClient> alive_clients = new List<TcpClient>();
        
        static void Main(string[] args)
        {
            Program server = new Program();
            server.listen();
        }

        private void listen()
        {            
            TcpListener server = new TcpListener(IPAddress.Any, 4200);
            try {
                
                server.Start();

                while (true) {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();

                    new Thread((o) =>
                    {
                        AcceptCallback(client);
                    }).Start();
                }
            } catch (Exception e) {  
                Console.WriteLine(e.ToString());  
            }
        }
        
        private void AcceptCallback(TcpClient client)
        {
            ConnectedCallback(client);

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[256];

            while (client.Connected)
            {
                try
                {
                    stream.Read(data, 0, data.Length);
                    ReadCallback(data);
                    data = new byte[256];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            DisconnectedCallback(client);
        }

        private void ReadCallback(byte[] data)
        {
            NetworkStream stream;
            String msg = Encoding.UTF8.GetString(data);

            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("New message: "+msg);
                SendMsgToAll(data);   
            }
        }

        private void ConnectedCallback(TcpClient client)
        {
            alive_clients.Add(client);
            String socketIp = SocketIp(client);
            byte[] msg = Encoding.ASCII.GetBytes("Client with IP "+socketIp+" joined the chat");
            Console.WriteLine("New connection from IP: "+socketIp);

            SendMsgToAll(msg);
        }

        private void DisconnectedCallback(TcpClient client)
        {
            String socketIp = SocketIp(client);
            client.Close();
            alive_clients.Remove(client);
            byte[] msg = Encoding.ASCII.GetBytes("Client with IP "+socketIp+" left the chat");
            Console.WriteLine("Client with IP: "+socketIp+" just left.");

            SendMsgToAll(msg);
        }

        private void SendMsgToAll(byte[] msg)
        {
            NetworkStream stream;
            
            alive_clients.ForEach(c =>
            {
                if (c.Connected)
                {
                    try 
                    {
                        stream = c.GetStream();
                        stream.Write(msg, 0, msg.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: Trying to write through disconnected socket");
                    }
                }
            });
        }

        private Boolean isAlive(TcpClient client) {
//            Ping myPing = new Ping();
//            String socketIp = SocketIp(client);
//            if(!socketIp.Equals("127.0.0.1"))
//            {
//                try
//                {
//                    PingReply reply = myPing.Send(socketIp);
//                    Console.WriteLine("Statut du ping " +
//                                      reply.Status);
//                    return reply.Status.Equals(IPStatus.Success);
//                }
//                catch (PingException e)
//                {
//                    return false;
//                }
//            }
            return true;
        }

        private static string SocketIp(TcpClient client)
        {
            String ip = client.Client.RemoteEndPoint.ToString();
            return ip.Split(":")[0];
        }
    }
}