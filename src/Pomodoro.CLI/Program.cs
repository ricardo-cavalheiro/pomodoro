namespace Pomodoro.CLI;

using System.Timers;
using Pomodoro.Core.Entities;

public class Program
{
  public static void Main()
  {
    var workInterval = TimeSpan.FromMinutes(0.1);
    var breakInterval = TimeSpan.FromMinutes(0.1);
    var settings = new Settings();
    var task = new Task("Work", settings, 2);
    var taskCompleted = new ManualResetEvent(false);

    Console.WriteLine($"Starting task `{task.Name}`\n");

    task.StartTask();

    var timer = new Timer(1000);

    timer.Elapsed += (sender, e) =>
    {
      Console.Clear();
      Console.WriteLine($"Clock ID: {task.CurrentActiveTimer.Id}");
      Console.WriteLine($"Start date {task.CurrentActiveTimer.StartDate}");
      Console.WriteLine($"End date {task.CurrentActiveTimer.EndDate}");
      Console.WriteLine($"Remaining time: {task.CurrentActiveTimer.RemainingTime()}");
      Console.WriteLine($"Elapsed time: {task.CurrentActiveTimer.ElapsedTime()}");
      Console.WriteLine($"Total elapsed time: {task.CurrentActiveTimer.TotalElapsedTime()}");
      Console.WriteLine($"Actual time: {DateTime.Now}");
      Console.WriteLine($"Is Work time completed: {task.CurrentActiveTimer.WorkInterval.IsCompleted}");
      Console.WriteLine($"Is Break time completed: {task.CurrentActiveTimer.BreakInterval.IsCompleted}");
      Console.WriteLine($"Is Break Auto Start on: {task.CurrentActiveTimer.AutoStartBreak}");

      if (task.IsCompleted() || task.CurrentActiveTimer.IsCompleted)
      {
        Console.WriteLine($"Task total elapsed time: {task.TaskTotalElapsedTime()}");
        Console.WriteLine($"Task `{task.Name}` finished!");
        timer.Stop();
        taskCompleted.Set();
        Environment.Exit(0);
      }
    };

    timer.AutoReset = true;
    timer.Enabled = true;

    taskCompleted.WaitOne();
  }
}

