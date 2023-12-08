using Pomodoro.Core.Constants;

namespace Pomodoro.Core.Entities;

public class Task
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  private readonly List<Clock> _clocks;

  public int AmountOfTasks { get; private set; } = TaskConstants.DefaultAmountOfClocks;

  public string Name { get; private set; }

  public bool IsCompleted { get; private set; } = false;

  public Clock CurrentActiveClock { get; private set; }

  public Task(string name, Clock clock, int amountOfTasks = TaskConstants.DefaultAmountOfClocks)
  {
    Name = name;
    AmountOfTasks = amountOfTasks;
    _clocks = [];

    if (amountOfTasks > TaskConstants.DefaultAmountOfClocks)
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

  public TimeSpan ClocksTotalTime()
  {
    var totalTime = TimeSpan.FromMinutes(0);

    foreach (Clock clock in _clocks)
    {
      totalTime += clock.WorkInterval.Duration;
    }

    return totalTime;
  }

  public void MarkTaskAsCompleted(bool force = false)
  {
    if (force)
    {
      IsCompleted = true;
      return;
    }

    foreach (var clock in _clocks)
    {
      if (!clock.IsCompleted) return;
    }

    IsCompleted = true;
  }

  public void AddClock(Clock newClock)
  {
    _clocks.Add(newClock);
  }

  public void AddRangeClocks(Clock clock)
  {
    for (var i = 0; i < AmountOfTasks; i++)
    {
      _clocks.Add(new Clock(clock.WorkInterval.Duration, clock.BreakInterval.Duration));
    }
  }
}
