using ChatCore;

namespace ChatTests;

// Fake connection for tests: stores everything sent instead of using the network
public class FakeClientConnection : IClientConnection
{
    public List<string> Received { get; } = new();

    public void Send(string text)
    {
        Received.Add(text);
    }
}