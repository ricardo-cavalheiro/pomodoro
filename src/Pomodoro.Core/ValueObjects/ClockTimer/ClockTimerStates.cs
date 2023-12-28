namespace Pomodoro.Core.ValueObjects.ClockTimer;

using System.Timers;
using Pomodoro.Core.Entities;

public abstract class CustomTimer(Timer timer)
{
  protected Timer _timer = timer;

  public event EventHandler<ElapsedEventArgs> OnElapsed;

  public abstract void Start();

  public abstract void Stop();

  public abstract TimeSpan ElapsedTime();

  public abstract TimeSpan RemainingTime();

  protected virtual void OnTimerElapsed(object? sender, ElapsedEventArgs eventArgs)
  {
    OnElapsed?.Invoke(sender, eventArgs);
  }
}

public class WorkState : CustomTimer
{
  private readonly Clock _clock;

  public WorkState(Clock clock) : base(new Timer(clock.WorkInterval.Duration))
  {
    _clock = clock;
    _timer = new Timer(_clock.WorkInterval.Duration);
    _timer.Elapsed += OnWorkPeriodEnded;
  }

  public override TimeSpan ElapsedTime()
  {
    return
      !_clock.WorkInterval.IsCompleted
        ? DateTime.Now - _clock.WorkInterval.StartDate
        : TimeSpan.Zero;
  }

  public override TimeSpan RemainingTime()
  {
    return
      !_clock.WorkInterval.IsCompleted
        ? _clock.WorkInterval.StartDate.AddMinutes(_clock.WorkInterval.Duration.TotalMinutes) - DateTime.Now
        : TimeSpan.Zero;
  }

  public override void Start()
  {
    _timer.Start();
  }

  public override void Stop()
  {
    _timer.Stop();
  }

  private void OnWorkPeriodEnded(object? source, ElapsedEventArgs eventArgs)
  {
    Stop();
    _timer.Dispose();

    _clock.WorkInterval.EndDate = eventArgs.SignalTime;
    _clock.WorkInterval.IsCompleted = true;

    base.OnTimerElapsed(source, eventArgs);
  }
}

public class BreakState : CustomTimer
{
  private readonly Clock _clock;

  public BreakState(Clock clock) : base(new Timer(clock.WorkInterval.Duration))
  {
    _clock = clock;
    _timer = new Timer(_clock.BreakInterval.Duration);
    _timer.Elapsed += OnBreakPeriodEnded;
  }

  public override TimeSpan ElapsedTime()
  {
    return
      !_clock.BreakInterval.IsCompleted
        ? DateTime.Now - _clock.BreakInterval.StartDate
        : TimeSpan.Zero;
  }

  public override TimeSpan RemainingTime()
  {
    return
      !_clock.BreakInterval.IsCompleted
        ? _clock.BreakInterval.StartDate.AddMinutes(_clock.BreakInterval.Duration.TotalMinutes) - DateTime.Now
        : TimeSpan.Zero;
  }

  public override void Start()
  {
    _timer.Start();
  }

  public override void Stop()
  {
    _timer.Stop();
  }

  private void OnBreakPeriodEnded(object? source, ElapsedEventArgs eventArgs)
  {
    Stop();
    _timer.Dispose();

    _clock.BreakInterval.EndDate = eventArgs.SignalTime;
    _clock.BreakInterval.IsCompleted = true;

    base.OnTimerElapsed(source, eventArgs);
  }
}
