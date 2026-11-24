using System.Collections.Concurrent;

namespace ChatCore;

// Keeps track of connected users and delivers messages to everyone
public class ChatRoom
{
    // Concurrent because the server handles many clients at the same time
    private readonly ConcurrentDictionary<Guid, (User User, IClientConnection Connection)> _members = new();

    public int Count => _members.Count;

    public User Join(string name, IClientConnection connection)
    {
        var user = new User(Guid.NewGuid(), name);
        _members[user.Id] = (user, connection);
        Broadcast($"*** {user.Name} joined ***");
        return user;
    }

    public void SendMessage(User sender, string text)
    {
        var message = new ChatMessage(sender.Name, text, DateTime.Now);
        Broadcast(message.ToString());
    }

    public void Leave(User user)
    {
        if (_members.TryRemove(user.Id, out _))
            Broadcast($"*** {user.Name} left ***");
    }

    // Sends the text to every connected user
    private void Broadcast(string text)
    {
        foreach (var member in _members.Values)
            member.Connection.Send(text);
    }
}