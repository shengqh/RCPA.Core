namespace RCPA
{
  public interface IFileReader<T>
  {
    T ReadFromFile(string fileName);
  }

  public interface IFileWriter<T>
  {
    void WriteToFile(string fileName, T t);
  }

  public interface IFileFormat<T> : IFileReader<T>, IFileWriter<T>
  { }

  public interface IFileReader2<T> : IFileReader<T>
  {
    string GetFormatFile();

    string GetName();
  }
}