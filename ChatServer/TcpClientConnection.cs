using ChatCore;

namespace ChatServer;

// Real connection: sends text over the network to one client
public class TcpClientConnection : IClientConnection
{
    private readonly StreamWriter _writer;
    private readonly object _lock = new();

    public TcpClientConnection(StreamWriter writer)
    {
        _writer = writer;
    }

    public void Send(string text)
    {
        try
        {
            // Lock so two messages are never written at the same time
            lock (_lock) _writer.WriteLine(text);
        }
        catch (Exception)
        {
            // Client has disconnected; the server removes it when its read loop ends
        }
    }
}