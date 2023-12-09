namespace Pomodoro.CLI;

using Pomodoro.Core.Entities;

public class Program
{
  public static void Main()
  {
    // fazer com que o código inicie o próximo timer quando o anterior acabar
    var workInterval = TimeSpan.FromMinutes(0.1);
    var breakInterval = TimeSpan.FromMinutes(0.1);
    var clock = new Clock(workInterval, breakInterval) { AutoStartBreak = true };
    var task = new Task("Work", clock, 2);

    Console.WriteLine($"Starting task `{task.Name}`\n");

    task.StartTask();

    var systemTimer = new System.Timers.Timer(1000);

    systemTimer.Elapsed += (sender, e) =>
    {
      Console.Clear();
      Console.WriteLine($"Start date {task.CurrentActiveClock.StartDate}");
      Console.WriteLine($"Remaining time: {task.CurrentActiveClock.RemainingTime()}");
      Console.WriteLine($"Elapsed time: {task.CurrentActiveClock.ElapsedTime()}");
      Console.WriteLine($"Actual time: {DateTime.Now}");
      Console.WriteLine($"Is Work time completed: {task.CurrentActiveClock.WorkInterval.IsCompleted}");
      Console.WriteLine($"Is Break time completed: {task.CurrentActiveClock.BreakInterval.IsCompleted}");
      Console.WriteLine($"Is Break Auto Start on: {task.CurrentActiveClock.AutoStartBreak}");

      if (task.IsCompleted())
      {
        Console.WriteLine($"Task total elapsed time: {task.TaskTotalElapsedTime()}");
        Console.WriteLine($"Task `{task.Name}` finished!");
        systemTimer.Stop();
        Environment.Exit(0);
      }
    };

    systemTimer.AutoReset = true;
    systemTimer.Enabled = true;

    Console.ReadLine();
  }
}

