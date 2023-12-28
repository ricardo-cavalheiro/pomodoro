namespace Pomodoro.Core.Entities;

public class Task
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  private readonly Queue<Clock> _clocks;

  public int AmountOfTimers { get; private set; }

  public string Name { get; private set; }

  public Clock CurrentActiveClock { get; private set; }

  public Settings Settings { get; private set; }

  public Task(string name, Settings? settings, int amountOfTimers = Settings.DefaultAmountOfTimers)
  {
    Name = name;
    AmountOfTimers = amountOfTimers;
    Settings = settings ?? new Settings();
    _clocks = [];

    if (amountOfTimers > Settings.DefaultAmountOfTimers)
    {
      for (var i = 0; i < AmountOfTimers; i++)
      {
        _clocks.Enqueue(new Clock(Settings.DefaultWorkTime, Settings.DefaultBreakTime));
      }
    }
    else
    {
      _clocks.Enqueue(new Clock(Settings.DefaultWorkTime, Settings.DefaultBreakTime));
    }

    CurrentActiveClock = _clocks.Peek();

    CurrentActiveClock.OnClockEnd += () =>
    {
      _clocks.Dequeue();
      CurrentActiveClock = _clocks.Peek();
    };
  }

  public void StartTask()
  {
    CurrentActiveClock.StartClock();
  }

  public void StopTask()
  {
    CurrentActiveClock.StopClock();
  }

  public bool IsCompleted()
  {
    return _clocks.All(clock => clock.IsCompleted);
  }

  public TimeSpan TaskTotalEstimatedTime()
  {
    return _clocks.Aggregate(
      TimeSpan.Zero,
      (totalTime, clock) => totalTime + clock.WorkInterval.Duration
    );
  }

  public TimeSpan TaskTotalElapsedTime()
  {
    return _clocks.Aggregate(
      TimeSpan.Zero,
      (totalTime, clock) => totalTime + clock.TotalElapsedTime()
    );
  }

  public void AddClock(Clock newClock)
  {
    _clocks.Enqueue(newClock);
  }
}
