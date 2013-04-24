using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA
{
  public class FileChangedEventArgs : EventArgs
  {
    public string[] Files { get; set; }
  }

  public delegate void FileChangedEventHandler(object sender, FileChangedEventArgs e);
}
