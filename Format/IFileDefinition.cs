namespace RCPA.Format
{
  public interface IFileDefinition
  {
    void ReadFromFile(string fileName);

    void WriteToFile(string fileName);

    void WriteSampleFile(string fileName);
  }
}
