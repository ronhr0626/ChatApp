using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatCore;
using ChatServer;

const int Port = 5000;
var room = new ChatRoom();

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Server listening on port {Port}...");

while (true)
{
    // Wait for a new client, then handle it without blocking the next one
    var client = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(client);
}

async Task HandleClientAsync(TcpClient client)
{
    using var stream = client.GetStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

    // First line from the client is the username
    var name = await reader.ReadLineAsync() ?? "Anon";
    var user = room.Join(name, new TcpClientConnection(writer));
    Console.WriteLine($"{name} connected");

    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            room.SendMessage(user, line);
    }
    catch (IOException)
    {
        // Client dropped the connection
    }
    finally
    {
        room.Leave(user);
        client.Dispose();
        Console.WriteLine($"{name} disconnected");
    }
}