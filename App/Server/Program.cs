using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Server;

class Program
{
    private const int Port = 5000; // Port on which the server will listen

    static List<Markets> markets = new List<Markets>()
    {
        new Markets("Apple", 10.0),
        new Markets("Samsung", 10.0),
        new Markets("HP", 10.0),
        new Markets("Huawai", 10.0),
        new Markets("Google", 10.0),
        new Markets("Lenovo", 10.0),
        new Markets("Thinkpad", 10.0),
        new Markets("ASUS", 10.0),
        new Markets("Dell", 10.0)
    };

    static void Main(string[] args)
    {
        StartServer();
    }


    public static void StartServer()
    {
        TcpListener server = null;
        try
        {
            // Create a listener that listens on the given port
            server = new TcpListener(IPAddress.Any, Port);
            server.Start();

            Console.WriteLine("Server started. Waiting for a connection...");

            // Accept client connection
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                // Handle client communication in a separate method
                HandleClient(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
        finally
        {
            server?.Stop();
        }
    }

    private static void HandleClient(TcpClient client)
    {
        // Buffer for reading data
        byte[] bytes = new byte[1024];
        string data = null;

        NetworkStream stream = client.GetStream();

        try
        {
            // Read client data from the stream
            int bytesRead;
            while ((bytesRead = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Convert bytes to a string
                data = Encoding.ASCII.GetString(bytes, 0, bytesRead);

                Console.WriteLine($"Received: {data}");
                if (data == "GETMarkt")
                {
                    foreach (Markets market in markets)
                    {
                        byte[] response = Encoding.ASCII.GetBytes($"{market.Marketname},{market.CurrontPrice}");
                        stream.Write(response, 0, response.Length);
                        Thread.Sleep(200);
                    }
                    // Respond to client
                }

                Console.WriteLine("Sent back a response to the client.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
        finally
        {
            stream.Close();
            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}