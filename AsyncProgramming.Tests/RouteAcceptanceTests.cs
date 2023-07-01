using AsyncProgramming;
using Moq;
using System.Diagnostics;

namespace AsyncProgramming.Tests
{
  public class RouteAcceptanceTests
  {
    DriverTestFactory driverTestFactory = new DriverTestFactory();

    [Fact]
    public async Task BothAccept_And_DriverA_AcceptsFirst()
    {
      IDriverInfo driverInfo = driverTestFactory.GetDriverInfoInstance(
        new Driver() { DriverId = 1, AcceptanceResponse = true, AwaitedTime = 1500 },
        new Driver() { DriverId = 2, AcceptanceResponse = true, AwaitedTime = 3000 }
        );

      RouteAcceptance routeAcceptance = new RouteAcceptance(driverInfo);
      Func<Task<int?>> task = () => routeAcceptance.GetAcceptanceDriver(1, 2);

      await AssertTimeOut(task, 1, 1500);
    }

    [Fact]
    public async Task BothAccept_And_DriverB_AcceptsFirst()
    {
      IDriverInfo driverInfo = driverTestFactory.GetDriverInfoInstance(
        new Driver() { DriverId = 1, AcceptanceResponse = true, AwaitedTime = 3000 },
        new Driver() { DriverId = 2, AcceptanceResponse = true, AwaitedTime = 1500 }
        );

      RouteAcceptance routeAcceptance = new RouteAcceptance(driverInfo);
      Func<Task<int?>> task = () => routeAcceptance.GetAcceptanceDriver(1, 2);

      await AssertTimeOut(task, 2, 1500);
    }

    [Fact]
    public async Task NoDriver_Accepts()
    {
      IDriverInfo driverInfo = driverTestFactory.GetDriverInfoInstance(
        new Driver() { DriverId = 1, AcceptanceResponse = false, AwaitedTime = 3000 },
        new Driver() { DriverId = 2, AcceptanceResponse = false, AwaitedTime = 1500 }
        );

      RouteAcceptance routeAcceptance = new RouteAcceptance(driverInfo);
      Func<Task<int?>> task = () => routeAcceptance.GetAcceptanceDriver(1, 2);

      await AssertTimeOut(task, null, 3000);
    }

    private async Task AssertTimeOut(Func<Task<int?>> task, int? selectedDriverId, int timeOutInMiliseconds)
    {
      int extraMiliseconds = 100;
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      int? resultAux = await task();
      stopwatch.Stop();
      TimeSpan timeSpan = stopwatch.Elapsed;
      Assert.True(timeSpan.TotalMilliseconds <= timeOutInMiliseconds + extraMiliseconds, $"The application took longer than {timeOutInMiliseconds} to execute");
      Assert.Equal(selectedDriverId, resultAux);
    }
  }

  public class DriverTestFactory
  {
    public IDriverInfo GetDriverInfoInstance(Driver driverA, Driver driverB)
    {
      Mock<IDriverInfo> driverInfo = new Mock<IDriverInfo>();
      driverInfo.Setup(d => d.GetDriverResponse(driverA.DriverId)).ReturnsAsync(() =>
      {
        Thread.Sleep(driverA.AwaitedTime);
        return driverA.AcceptanceResponse;
      });
      driverInfo.Setup(d => d.GetDriverResponse(driverB.DriverId)).ReturnsAsync(() =>
      {
        Thread.Sleep(driverB.AwaitedTime);
        return driverB.AcceptanceResponse;
      });
      return driverInfo.Object;
    }
  }
}