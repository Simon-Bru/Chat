using System;
using System.Net;
using System.Net.Sockets;

namespace ChatApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string message;
            do
            {
               message=Run();
               Console.WriteLine(message);
            } while (message != "/shutdown");
        }
        
        static string Run()
        {
            try{
            TcpListener server = new TcpListener(IPAddress.Any, 4200);
            server.Start();
                String responseData = String.Empty;
            
                do
                { Byte[] data = new Byte[256];
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                   
                    
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received : " + responseData);
                    data = new Byte[256];
                    data = System.Text.Encoding.ASCII.GetBytes("hello from server");
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();
                } while (responseData != "/shutdown");
           
                //server.EndAcceptTcpClient();
                
               
                
                
                return responseData;
                
            }
                catch (ArgumentNullException e) 
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            } 
            catch (SocketException e) 
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            return "running...";
//            server.BeginReceive(new AsyncCallback(ar =>
//            {
//                Console.Write(ar.ToString());
//            }), server)

        }
    }
}