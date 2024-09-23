namespace Events;

public class GameFinishedEvent
{
    public Dictionary<string, object> Header { get; set; } = new();
    public Guid GameId { get; set; }
    public string? WinnerId { get; set; }
    public Dictionary<string, Move> Moves { get; set; } = new Dictionary<string, Move>();
}