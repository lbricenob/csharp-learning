namespace AsyncProgramming
{
  public interface IDriverInfo
  {
    public Task<bool> GetDriverResponse(int driverId);
  }
}
