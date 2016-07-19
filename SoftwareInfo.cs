namespace RCPA
{
  public static class SoftwareInfo
  {
    public static string SoftwareName { get; set; }
    public static string SoftwareVersion { get; set; }
    static SoftwareInfo()
    {
      SoftwareName = string.Empty;
      SoftwareVersion = string.Empty;
    }
  }
}
