namespace ChatCore;

// Delivers text to one connected user (a real socket, or a fake in tests)
public interface IClientConnection
{
    void Send(string text);
}