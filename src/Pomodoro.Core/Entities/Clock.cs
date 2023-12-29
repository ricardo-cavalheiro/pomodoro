namespace Pomodoro.Core.Entities;

using Pomodoro.Core.Enums;
using Pomodoro.Core.ValueObjects;
using Pomodoro.Core.ValueObjects.ClockTimer;

public class Clock
{
  public readonly Guid Id = Guid.NewGuid();

  private readonly ClockTimer _clockTimer;

  public EClockTimerStates CurrentState = EClockTimerStates.Work;

  public bool AutoStartBreak { get; set; } = true;

  public WorkInterval WorkInterval { get; private set; }

  public BreakInterval BreakInterval { get; private set; }

  public bool IsCompleted => WorkInterval.IsCompleted && BreakInterval.IsCompleted;

  public DateTime StartDate => WorkInterval.StartDate;

  public DateTime EndDate => BreakInterval.EndDate;

  public event Action OnClockEnd;

  public Clock(TimeSpan workInterval, TimeSpan breakInterval)
  {
    WorkInterval = new WorkInterval(workInterval);
    BreakInterval = new BreakInterval(breakInterval);

    _clockTimer = new ClockTimer(this);
  }

  public void StartClock() => _clockTimer.StartTimer();

  public void StopClock() => _clockTimer.StopTimer();

  public TimeSpan ElapsedTime() => _clockTimer.ElapsedTime();

  public TimeSpan RemainingTime() => _clockTimer.RemainingTime();

  public TimeSpan TotalElapsedTime() => IsCompleted ? EndDate - StartDate : DateTime.Now - StartDate;

  public void OnClockEnded() => OnClockEnd.Invoke();
}
