using System;
using System.Net.Sockets;
using System.Text;

namespace App.Interaction;

public class Client
{
    
    private const string ServerIp = "127.0.0.1"; // Localhost
    private const int Port = 5000; // Must match the server port

    public static void StartClient()
    {
        try
        {
            // Create a TcpClient and connect to the server
            TcpClient client = new TcpClient(ServerIp, Port);
            Console.WriteLine("Connected to server.");

            NetworkStream stream = client.GetStream();

            // Send data to the server
            string message = "GETMarkt";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            // Receive data from the server
            do
            {
                byte[] responseData = new byte[1024];
                int bytes = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.ASCII.GetString(responseData, 0, bytes);
                
                Program.markets.Add(response.Split(',').First(), new Markets(response.Split(',').First(), Convert.ToDouble(response.Split(',').Last())));
                
            }while (stream.DataAvailable);

            // Close everything
            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }
}
