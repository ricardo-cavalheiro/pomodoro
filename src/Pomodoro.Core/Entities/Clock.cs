namespace Pomodoro.Core.Entities;

using Pomodoro.Core.Enums;
using Pomodoro.Core.ValueObjects;
using Pomodoro.Core.ValueObjects.ClockTimer;

public class Clock
{
  public readonly Guid Id = Guid.NewGuid();

  private readonly ClockTimer _clockTimer;

  public Settings Settings { get; private set; }

  public EClockTimerStates CurrentState = EClockTimerStates.Work;

  public WorkInterval WorkInterval { get; private set; }

  public BreakInterval BreakInterval { get; private set; }

  public bool IsCompleted =>
    WorkInterval.IsCompleted
    && BreakInterval.IsCompleted;

  public DateTime StartDate => WorkInterval.StartDate;

  public DateTime EndDate => BreakInterval.EndDate;

  public event Action OnClockEnd = () => { };

  public Clock(
    TimeSpan workInterval,
    TimeSpan breakInterval,
    Settings settings
  )
  {
    WorkInterval = new WorkInterval(workInterval);
    BreakInterval = new BreakInterval(breakInterval);
    Settings = settings;

    _clockTimer = new ClockTimer(this);
  }

  public void StartClock() => _clockTimer.StartTimer();

  public void StopClock() => _clockTimer.StopTimer();

  public TimeSpan ElapsedTime() => _clockTimer.ElapsedTime();

  public TimeSpan RemainingTime() => _clockTimer.RemainingTime();

  public TimeSpan TotalElapsedTime() =>
    IsCompleted
      ? EndDate - StartDate
      : DateTime.Now - StartDate;

  public void OnClockEnded() => OnClockEnd.Invoke();
}
