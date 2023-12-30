namespace Pomodoro.Core.ValueObjects.ClockTimer;

using System.Timers;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Entities;

public class ClockTimer
{
  private readonly Clock _clock;

  private ClockTimerState _currentTimer;

  public ClockTimer(Clock clock)
  {
    _clock = clock;

    var workState = new WorkState(_clock);
    _currentTimer = workState;

    _currentTimer.OnElapsed += OnWorkElapsed;
  }

  public void StartTimer() => _currentTimer.Start();

  public void StopTimer() => _currentTimer.Stop();

  public TimeSpan ElapsedTime() => _currentTimer.ElapsedTime();

  public TimeSpan RemainingTime() => _currentTimer.RemainingTime();

  private void OnWorkElapsed(object? sender, ElapsedEventArgs e)
  {
    _currentTimer = new BreakState(_clock);
    _currentTimer.OnElapsed += OnBreakElapsed;

    _clock.CurrentState = EClockTimerStates.Interval;
    _clock.BreakInterval.StartDate = e.SignalTime;

    if (_clock.Settings.AutoStartBreak) _currentTimer.Start();
  }

  private void OnBreakElapsed(
    object? sender,
    ElapsedEventArgs e
  ) => _clock.OnClockEnded();
}
