namespace AsyncProgramming
{
  public class RouteAcceptance
  {
    private IDriverInfo _driverInfo;

    public RouteAcceptance(IDriverInfo driverInfo)
    {
      _driverInfo = driverInfo;
    }

    public async Task<int?> GetAcceptanceDriver(int driverAId, int driverBId)
    {
      //bool resultA = await _driverInfo.GetDriverResponse(driverAId);
      //bool resultB = await _driverInfo.GetDriverResponse(driverBId);
      ////bool resultB = false;

      //if (resultA && resultB)
      //  return driverAId;
      //if(!resultA && !resultB)
      //  return null;
      //return driverBId;
      throw new NotImplementedException();
    }
  }
}
