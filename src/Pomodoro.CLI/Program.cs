namespace Pomodoro.CLI;

using Pomodoro.Core.Enums;
using Pomodoro.Core.Entities;

public class Program
{
  public static void Main()
  {
    // fazer com que o código inicie o próximo timer quando o anterior acabar
    var workInterval = TimeSpan.FromMinutes(0.1);
    var breakInterval = TimeSpan.FromMinutes(0.1);
    var settings = new Settings();
    var task = new Task("Work", settings, 2);
    var taskCompleted = new ManualResetEvent(false);

    Console.WriteLine($"Starting task `{task.Name}`\n");

    task.StartTask();

    var systemTimer = new System.Timers.Timer(1000);

    systemTimer.Elapsed += (sender, e) =>
    {
      Console.Clear();
      Console.WriteLine($"Start date {task.CurrentActiveClock.StartDate}");
      Console.WriteLine($"End date {task.CurrentActiveClock.EndDate}");
      Console.WriteLine($"Remaining time: {task.CurrentActiveClock.RemainingTime()}");
      Console.WriteLine($"Elapsed time: {task.CurrentActiveClock.ElapsedTime()}");
      Console.WriteLine($"Total elapsed time: {task.CurrentActiveClock.TotalElapsedTime()}");
      Console.WriteLine($"Actual time: {DateTime.Now}");
      Console.WriteLine($"Is Work time completed: {task.CurrentActiveClock.WorkInterval.IsCompleted}");
      Console.WriteLine($"Is Break time completed: {task.CurrentActiveClock.BreakInterval.IsCompleted}");
      Console.WriteLine($"Is Break Auto Start on: {task.CurrentActiveClock.AutoStartBreak}");

      if (!task.CurrentActiveClock.AutoStartBreak && task.CurrentActiveClock.WorkInterval.IsCompleted && !task.CurrentActiveClock.BreakInterval.IsCompleted)
      {
        Console.WriteLine("Press enter to start break");

        var pressedKey = Console.ReadKey();

        if (pressedKey.Key == ConsoleKey.Enter)
        {
          task.CurrentActiveClock.StartClock();
        }
      }

      if (
        task.CurrentActiveClock.CurrentState == EClockTimerStates.Interval
          && task.CurrentActiveClock.WorkInterval.IsCompleted
          && !task.CurrentActiveClock.BreakInterval.IsCompleted
          && !task.CurrentActiveClock.AutoStartBreak
      )
      {
        task.CurrentActiveClock.StartClock();
      }

      if (task.IsCompleted())
      {
        Console.WriteLine($"Task total elapsed time: {task.TaskTotalElapsedTime()}");
        Console.WriteLine($"Task `{task.Name}` finished!");
        systemTimer.Stop();
        taskCompleted.Set();
        Environment.Exit(0);
      }
    };

    systemTimer.AutoReset = true;
    systemTimer.Enabled = true;

    taskCompleted.WaitOne();
  }
}

