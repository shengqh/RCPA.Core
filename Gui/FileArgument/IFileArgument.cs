using System;
using System.Windows.Forms;

namespace RCPA.Gui.FileArgument
{
  public interface IFileArgument
  {
    String GetFileDescription();

    String GetBrowseDescription();

    FileDialog GetFileDialog();

    bool IsValid(String filename);

    bool IsSaving { get; }
  }
}