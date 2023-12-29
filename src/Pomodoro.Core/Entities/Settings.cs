namespace Pomodoro.Core.Entities;

public class Settings
{
  public const int DefaultAmountOfTimers = 1;

  public bool AutoStartBreak { get; set; } = false;

  public bool AutoStartWork { get; set; } = false;

  public readonly static TimeSpan DefaultWorkTime = TimeSpan.FromMinutes(0.1);

  public readonly static TimeSpan DefaultBreakTime = TimeSpan.FromMinutes(0.1);
}
