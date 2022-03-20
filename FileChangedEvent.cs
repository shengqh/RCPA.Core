using System;

namespace RCPA
{
  public class FileChangedEventArgs : EventArgs
  {
    public string[] Files { get; set; }
  }

  public delegate void FileChangedEventHandler(object sender, FileChangedEventArgs e);
}
