namespace Pomodoro.Core.ValueObjects.ClockTimer;

using System.Timers;
using Pomodoro.Core.Entities;

public abstract class ClockTimerState(Timer timer, Clock clock)
{
  protected Timer _timer = timer;

  protected Clock _clock = clock;

  public event EventHandler<ElapsedEventArgs> OnElapsed = (
    object? sender,
    ElapsedEventArgs eventArgs
  ) =>
  { };

  public abstract void Start();

  public abstract void Stop();

  public abstract TimeSpan ElapsedTime();

  public abstract TimeSpan RemainingTime();

  protected void OnTimerElapsed(
    object? sender,
    ElapsedEventArgs eventArgs
  ) => OnElapsed.Invoke(sender, eventArgs);
}

public class WorkState : ClockTimerState
{
  public WorkState(Clock clock) : base(
      new Timer(clock.WorkInterval.Duration),
      clock
    )
  {
    _timer.Elapsed += OnWorkPeriodEnded;
  }

  public override TimeSpan ElapsedTime() =>
    _clock.WorkInterval.IsActive
      ? DateTime.Now - _clock.WorkInterval.StartDate
      : TimeSpan.Zero;

  public override TimeSpan RemainingTime() =>
    _clock.WorkInterval.IsActive
      ? _clock.WorkInterval.StartDate.AddMinutes(_clock.WorkInterval.Duration.TotalMinutes) - DateTime.Now
      : TimeSpan.Zero;

  public override void Start()
  {
    _clock.WorkInterval.IsActive = true;

    _timer.Start();
  }

  public override void Stop()
  {
    _clock.WorkInterval.IsActive = false;

    _timer.Stop();
  }

  private void OnWorkPeriodEnded(object? source, ElapsedEventArgs eventArgs)
  {
    Stop();
    _timer.Dispose();

    _clock.WorkInterval.EndDate = eventArgs.SignalTime;
    _clock.WorkInterval.IsCompleted = true;
    _clock.WorkInterval.IsActive = false;

    base.OnTimerElapsed(source, eventArgs);
  }
}

public class BreakState : ClockTimerState
{
  public BreakState(Clock clock) :
    base(
      new Timer(clock.WorkInterval.Duration),
      clock
    )
  {
    _timer.Elapsed += OnBreakPeriodEnded;
  }

  public override TimeSpan ElapsedTime() =>
    _clock.BreakInterval.IsActive
      ? DateTime.Now - _clock.BreakInterval.StartDate
      : TimeSpan.Zero;

  public override TimeSpan RemainingTime() =>
    _clock.BreakInterval.IsActive
      ? _clock.BreakInterval.StartDate.AddMinutes(_clock.BreakInterval.Duration.TotalMinutes) - DateTime.Now
      : TimeSpan.Zero;

  public override void Start()
  {
    _clock.BreakInterval.IsActive = true;

    _timer.Start();
  }

  public override void Stop()
  {
    _clock.BreakInterval.IsActive = false;

    _timer.Stop();
  }

  private void OnBreakPeriodEnded(object? source, ElapsedEventArgs eventArgs)
  {
    Stop();
    _timer.Dispose();

    _clock.BreakInterval.EndDate = eventArgs.SignalTime;
    _clock.BreakInterval.IsCompleted = true;
    _clock.BreakInterval.IsActive = false;

    base.OnTimerElapsed(source, eventArgs);
  }
}
