using System;
using System.Collections.Generic;
using System.Net;
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
            IPEndPoint localEndPoint = new IPEndPoint(Dns.Resolve(Dns.GetHostName()).AddressList[0], 4200);
            
            TcpListener server = new TcpListener(localEndPoint);
            try {
                
                server.Start();

                while (true) {

                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();

                    new Thread((o) =>
                    {
                        AcceptCallback(client);
                    });
                }
            } catch (Exception e) {  
                Console.WriteLine(e.ToString());  
            }
        }
        
        private void AcceptCallback(TcpClient client)
        {
            alive_clients.Add(client);
            Console.WriteLine("New connection from bitch");

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[1024];

            while (client.Available > 0)
            {
                stream.Read(data, 0, data.Length);
                ReadCallback(data);
            }

            alive_clients.Remove(client);
            client.Close();
        }

        private void ReadCallback(byte[] data)
        {
            NetworkStream stream;
            String msg = Encoding.UTF8.GetString(data);
            Console.WriteLine("New message: "+msg);

            alive_clients.ForEach(c =>
            {
                stream = c.GetStream();
                stream.Write(data, 0, data.Length);
            });
        }
    }
}