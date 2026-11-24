using ChatCore;

namespace ChatTests;

public class ChatRoomTests
{
    [Fact]
    public void Join_AddsUserToRoom()
    {
        // Arrange
        var room = new ChatRoom();
        var alice = new FakeClientConnection();

        // Act
        room.Join("Alice", alice);

        // Assert
        Assert.Equal(1, room.Count);
    }

    [Fact]
    public void SendMessage_ReachesOtherUsers()
    {
        var room = new ChatRoom();
        var alice = new FakeClientConnection();
        var bob = new FakeClientConnection();
        var aliceUser = room.Join("Alice", alice);
        room.Join("Bob", bob);

        room.SendMessage(aliceUser, "hei Bob");

        Assert.Contains(bob.Received, m => m.Contains("Alice: hei Bob"));
    }

    [Fact]
    public void Leave_NotifiesOtherUsers()
    {
        var room = new ChatRoom();
        var alice = new FakeClientConnection();
        var bob = new FakeClientConnection();
        room.Join("Alice", alice);
        var bobUser = room.Join("Bob", bob);

        room.Leave(bobUser);

        Assert.Equal(1, room.Count);
        Assert.Contains(alice.Received, m => m.Contains("Bob left"));
    }
}