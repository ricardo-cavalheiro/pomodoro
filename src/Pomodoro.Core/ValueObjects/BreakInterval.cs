namespace Pomodoro.Core.ValueObjects;

public class BreakInterval(TimeSpan duration)
{
    public bool IsCompleted { get; set; } = false;

    public TimeSpan Duration { get; private set; } = duration;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
