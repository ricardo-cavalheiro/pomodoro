using Pomodoro.Core.Constants;

namespace Pomodoro.Core.Entities;

public class Task
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  private readonly Queue<Clock> _clocks;

  public int AmountOfTimers { get; private set; }

  public string Name { get; private set; }

  public Clock CurrentActiveClock { get; private set; }

  public Task(string name, Clock clock, int amountOfTimers = TaskConstants.DefaultAmountOfTimers)
  {
    Name = name;
    AmountOfTimers = amountOfTimers;
    _clocks = [];

    if (amountOfTimers > TaskConstants.DefaultAmountOfTimers)
    {
      AddRangeClocks(clock);
    }
    else
    {
      AddClock(clock);
    }

    CurrentActiveClock = clock;
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

  public void AddRangeClocks(Clock clock)
  {
    for (var i = 0; i < AmountOfTimers; i++)
    {
      _clocks.Enqueue(new Clock(clock.WorkInterval.Duration, clock.BreakInterval.Duration));
    }
  }
}
