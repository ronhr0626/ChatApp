using System.Net.Sockets;
using System.Text;

Console.Write("Name: ");
var name = Console.ReadLine() ?? "Anon";

using var client = new TcpClient();
await client.ConnectAsync("127.0.0.1", 5000);
using var stream = client.GetStream();
var reader = new StreamReader(stream, Encoding.UTF8);
var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

// First line sent to the server is the username
await writer.WriteLineAsync(name);
Console.WriteLine("Connected. Type messages, /quit to exit.");

// Receive messages in the background
_ = Task.Run(async () =>
{
    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
    catch (IOException)
    {
        // Server closed the connection
    }
    Console.WriteLine("Disconnected from server.");
});

// Send what the user types
string? input;
while ((input = Console.ReadLine()) != null && input != "/quit")
    await writer.WriteLineAsync(input);