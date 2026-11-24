namespace ChatCore;

// Represents a single chat message
public record ChatMessage(string Sender, string Text, DateTime Time)
{
    // Formats the message for display, e.g. "[10:41] Ronald: hei"
    public override string ToString() => $"[{Time:HH:mm}] {Sender}: {Text}";
}