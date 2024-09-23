using System.Diagnostics;
using Events;
using Helpers;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Serilog;

namespace Monolith;

public class RandomPlayer : IPlayer
{
    private const string PlayerId = "Mr. Random";

    public PlayerMovedEvent MakeMove(GameStartedEvent e)
    {
        var propagator = new TraceContextPropagator();
        var parentContext = propagator.Extract(default, e, (r, key) =>
        {
            return new List<string>(new[] { r.Header.ContainsKey(key) ? r.Header[key].ToString() : String.Empty });
        });
        Baggage.Current = parentContext.Baggage;
        using var activity = Monitoring.ActivitySource.StartActivity("Message received | RandomPlayer MakeMove", ActivityKind.Consumer, parentContext.ActivityContext);
        var random = new Random();
        var next = random.Next(3);
        var move = next switch
        {
            0 => Move.Rock,
            1 => Move.Paper,
            _ => Move.Scissor
        };
        Log.Logger.Debug("Player {PlayerId} has decided to perform the move {Move}", PlayerId, move);
        var moveEvent = new PlayerMovedEvent
        {
            GameId = e.GameId,
            PlayerId = PlayerId,
            Move = move
        };
        var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
        var propagationContext = new PropagationContext(activityContext, Baggage.Current);
        var propagatorRndPlayer = new TraceContextPropagator();
        propagatorRndPlayer.Inject(propagationContext, moveEvent, (r, key, value) =>
        { 
            r.Header.Add(key, value);
        });
        return moveEvent;
    }
    public void ReceiveResult(GameFinishedEvent e)
    {
        var propagator = new TraceContextPropagator();
        var parentContext = propagator.Extract(default, e, (r, key) =>
        {
            return new List<string>(new[] { r.Header.ContainsKey(key) ? r.Header[key].ToString() : String.Empty });
        });
        Baggage.Current = parentContext.Baggage;
        using var activity = Monitoring.ActivitySource.StartActivity("Received Result | Random Player", ActivityKind.Consumer, parentContext.ActivityContext);
    }

    public string GetPlayerId()
    {
        return PlayerId;
    }
}

