namespace RCPA.Utils
{
  public class MultipleLinesExtractorOptions
  {
    public MultipleLinesExtractorOptions()
    {
      //this.FromHead = true;
      this.LineCount = 10;
    }

    public int LineCount { get; set; }

    //public bool FromHead { get; set; }

    public string SourceFile { get; set; }

    public string TargetFile { get; set; }
  }
}
