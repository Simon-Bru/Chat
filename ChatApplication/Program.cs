using System;
using System.Net;
using System.Net.Sockets;

namespace ChatApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp);
            
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4200);

            server.Bind(localEndPoint);
            // SUPPORT 100 USERS BEFORE SENDING
            server.Listen(100);

            Socket childSocket = server.Accept();
            Console.WriteLine("Waiting for a connection...");
            server.BeginAccept(new AsyncCallback(ar =>
            {
                Console.Write(ar.ToString()); 
            }), server);
            
//            server.BeginReceive(new AsyncCallback(ar =>
//            {
//                Console.Write(ar.ToString());
//            }), server)

        }
    }
}