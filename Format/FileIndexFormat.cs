using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RCPA.Format
{
  public class FileIndexItem
  {
    public FileIndexItem()
    { }

    public FileIndexItem(long startPosition, long length, string fileScan)
    {
      this.StartPosition = startPosition;
      this.Length = length;
      this.Key = fileScan;
    }

    public long StartPosition { get; set; }

    public long Length { get; set; }

    public string Key { get; set; }

    public override string ToString()
    {
      return string.Format("{0},{1},{2}", StartPosition, Length, Key);
    }
  }

  public class FileIndexFormat : IFileFormat<List<FileIndexItem>>
  {
    #region IFileReader<List<IndexItem>> Members

    public List<FileIndexItem> ReadFromFile(string fileName)
    {
      List<FileIndexItem> result = new List<FileIndexItem>();

      using (StreamReader sr = new StreamReader(FileUtils.OpenReadFile(fileName)))
      {
        sr.ReadLine();

        string line;
        while (null != (line = sr.ReadLine()))
        {
          var parts = line.Split('\t');
          try
          {
            var item = new FileIndexItem();
            item.StartPosition = Convert.ToInt64(parts[0]);
            item.Length = Convert.ToInt64(parts[1]);
            item.Key = parts[2];
            result.Add(item);
          }
          catch (Exception ex)
          {
            throw new Exception(string.Format("Parsing index file error, line is invalid : {0}", line), ex);
          }
        }
      }

      return result;
    }

    #endregion

    #region IFileWriter<List<IndexItem>> Members

    public void WriteToFile(string fileName, List<FileIndexItem> t)
    {
      using (StreamWriter sw = new StreamWriter(fileName))
      {
        sw.WriteLine("Start\tLength\tKey");
        foreach (var item in t)
        {
          sw.WriteLine("{0}\t{1}\t{2}", item.StartPosition, item.Length, item.Key);
        }
      }
    }

    #endregion
  }
}
