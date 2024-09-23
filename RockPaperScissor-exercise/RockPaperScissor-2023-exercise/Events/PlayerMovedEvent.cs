namespace Events;

public class PlayerMovedEvent
{
    public Dictionary<string, object> Header { get; set; } = new();
    public Guid GameId { get; set; }
    public string PlayerId { get; set; }
    public Move Move { get; set; }
}