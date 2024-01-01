namespace Pomodoro.Core.Entities;

public class Task
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  private int CreatedTimersCount = 0;

  private readonly List<Clock> _clocks = [];

  public int AmountOfTimersToBeExecuted { get; private set; }

  public string Name { get; private set; }

  public Clock CurrentActiveTimer { get; private set; }

  public Settings Settings { get; private set; }

  public Task(
    string name,
    Settings? settings,
    int amountOfTimersToBeExecuted = Settings.DefaultAmountOfTimers
  )
  {
    Name = name;
    Settings = settings ?? new Settings();
    AmountOfTimersToBeExecuted = amountOfTimersToBeExecuted;

    var clock = new Clock(
      Settings.DefaultWorkTime,
      Settings.DefaultBreakTime,
      Settings
    );
    AddClock(clock);

    CurrentActiveTimer = clock;

    CurrentActiveTimer.OnClockEnd += OnClockEnded;
  }

  private void OnClockEnded()
  {
    if (CreatedTimersCount < AmountOfTimersToBeExecuted)
    {
      var clock = new Clock(
        Settings.DefaultWorkTime,
        Settings.DefaultBreakTime,
        Settings
      );
      AddClock(clock);

      CurrentActiveTimer = clock;

      if (Settings.AutoStartWork) StartTask();
    }
  }

  public void StartTask() => CurrentActiveTimer.StartClock();

  public void StopTask() => CurrentActiveTimer.StopClock();

  public bool IsCompleted() => _clocks.All(clock => clock.IsCompleted);

  public TimeSpan TaskTotalEstimatedTime() =>
    _clocks.Aggregate(
      TimeSpan.Zero,
      (totalTime, clock) => totalTime + clock.WorkInterval.Duration
    );

  public TimeSpan TaskTotalElapsedTime() =>
    _clocks.Aggregate(
      TimeSpan.Zero,
      (totalTime, clock) => totalTime + clock.TotalElapsedTime()
    );

  public void RemoveClock(Clock clockToBeRemoved) => _clocks.Remove(clockToBeRemoved);

  public void AddClock(Clock newClock)
  {
    _clocks.Add(newClock);

    AmountOfTimersToBeExecuted++;
    CreatedTimersCount++;
  }
}
