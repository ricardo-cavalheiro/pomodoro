using Pomodoro.Core.Entities;
using Pomodoro.Core.ValueObjects.ClockTimer;
using Task = System.Threading.Tasks.Task;

namespace Pomodoro.UnitTests.ValueObjects.ClockTimer;

public class ClockTimerStateTests
{
  public class Setup
  {
    public readonly Clock Clock;

    public readonly Settings Settings;

    public Setup()
    {
      Settings = new Settings();
      Clock = new Clock(Settings.DefaultWorkTime, Settings.DefaultBreakTime, Settings);
    }
  }

  public class WorkStateTests : IClassFixture<Setup>
  {
    private readonly Setup Setup;

    private readonly WorkState WorkState;

    public WorkStateTests(Setup setup)
    {
      Setup = setup;
      WorkState = new WorkState(Setup.Clock);
    }

    [Fact]
    public void Start_WorkState()
    {
      var clock = Setup.Clock;
      var clockTimerWorkState = WorkState;

      clockTimerWorkState.Start();

      Assert.True(clock.WorkInterval.IsActive);
      Assert.False(clock.WorkInterval.IsCompleted);
      Assert.False(clock.BreakInterval.IsActive);
      Assert.False(clock.BreakInterval.IsCompleted);
    }

    [Fact]
    public void Stop_WorkState()
    {
      var clock = Setup.Clock;
      var clockTimerWorkState = WorkState;

      clockTimerWorkState.Start();

      Assert.True(clock.WorkInterval.IsActive);
      Assert.False(clock.WorkInterval.IsCompleted);
      Assert.False(clock.BreakInterval.IsActive);
      Assert.False(clock.BreakInterval.IsCompleted);

      clockTimerWorkState.Stop();

      Assert.False(clock.WorkInterval.IsActive);
      Assert.False(clock.WorkInterval.IsCompleted);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async void Elapsed_Time_Should_Always_Be_Lesser_Than_Work_Duration(int seconds)
    {
      var clock = new Clock(
        Settings.DefaultWorkTime,
        Settings.DefaultBreakTime,
        Setup.Settings
      );
      var clockTimerWorkState = new WorkState(clock);

      clockTimerWorkState.Start();

      await Task.Delay(TimeSpan.FromSeconds(seconds));

      var elapsedTime = clockTimerWorkState.ElapsedTime();

      Assert.True(elapsedTime <= clock.WorkInterval.Duration);
    }

    [Theory]
    [InlineData(6)]
    public async void Elapsed_Time_Should_Be_Set_To_Zero_When_It_is_Greater_Than_Work_Duration(int seconds)
    {
      var clockTimerWorkState = WorkState;

      clockTimerWorkState.Start();

      await Task.Delay(TimeSpan.FromSeconds(seconds));

      var elapsedTime = clockTimerWorkState.ElapsedTime();

      Assert.Equal(TimeSpan.Zero, elapsedTime);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async void Remaining_Time_Should_Be_Always_Less_Than_Work_Duration(int seconds)
    {
      var clockTimerWorkState = WorkState;

      clockTimerWorkState.Start();

      await Task.Delay(TimeSpan.FromSeconds(seconds));

      var remainingTime = clockTimerWorkState.RemainingTime();

      Assert.True(remainingTime <= Setup.Clock.WorkInterval.Duration);
    }
  }

  public class BreakStateTests : IClassFixture<Setup>
  {
    private readonly Setup Setup;

    private readonly BreakState BreakState;

    public BreakStateTests(Setup setup)
    {
      Setup = setup;
      BreakState = new BreakState(Setup.Clock);
    }

    [Fact]
    public void Start_BreakState()
    {
      var clock = Setup.Clock;
      var clockTimerBreakState = BreakState;

      clockTimerBreakState.Start();

      Assert.True(clock.BreakInterval.IsActive);
      Assert.False(clock.BreakInterval.IsCompleted);
      Assert.False(clock.WorkInterval.IsCompleted);
      Assert.False(clock.WorkInterval.IsActive);
    }

    [Fact]
    public void Stop_BreakState()
    {
      var clock = Setup.Clock;
      var clockTimerBreakState = BreakState;

      clockTimerBreakState.Start();

      Assert.True(clock.BreakInterval.IsActive);
      Assert.False(clock.BreakInterval.IsCompleted);
      Assert.False(clock.WorkInterval.IsActive);
      Assert.False(clock.WorkInterval.IsCompleted);

      clockTimerBreakState.Stop();

      Assert.False(clock.BreakInterval.IsActive);
      Assert.False(clock.BreakInterval.IsCompleted);
    }
  }
}
