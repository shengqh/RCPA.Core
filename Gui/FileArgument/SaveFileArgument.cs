using System;
using System.IO;
using System.Windows.Forms;

namespace RCPA.Gui.FileArgument
{
  public class SaveFileArgument : AbstractFileArgument
  {
    private readonly SaveFileDialog fileDialog;

    public SaveFileArgument(String fileDescription, String extension)
      : base(fileDescription, extension)
    {
      this.fileDialog = new SaveFileDialog();
      this.fileDialog.AutoUpgradeEnabled = true;
      this.fileDialog.Filter = fileFilter;
    }

    public SaveFileArgument(String fileDescription, String[] extensions)
      : base(fileDescription, extensions)
    {
      this.fileDialog = new SaveFileDialog();
      this.fileDialog.AutoUpgradeEnabled = true;
      this.fileDialog.Filter = fileFilter;
    }

    public override FileDialog GetFileDialog()
    {
      return this.fileDialog;
    }

    public override bool IsValid(string filename)
    {
      return (filename != null) &&
             (filename.Trim().Length != 0) &&
             !(new DirectoryInfo(filename.Trim()).Exists);
    }

    public override string GetBrowseDescription()
    {
      return "Save " + GetFileDescription() + " File ...";
    }

    public override bool IsSaving
    {
      get { return true; }
    }
  }
}