namespace RCPA
{
  public interface IMultipleFileSelector
  {
    string[] FileNames { get; set; }

    string[] SelectedFileNames { get; set; }
  }
}
