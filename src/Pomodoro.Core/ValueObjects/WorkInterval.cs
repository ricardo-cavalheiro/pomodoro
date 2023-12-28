namespace Pomodoro.Core.ValueObjects;

public class WorkInterval(TimeSpan duration)
{
  public bool IsCompleted { get; set; } = false;

  public bool IsActive { get; set; } = false;

  public TimeSpan Duration { get; private set; } = duration;

  public DateTime StartDate { get; set; } = DateTime.Now;

  public DateTime EndDate { get; set; }
}
