using System;
using System.IO;
using System.Windows.Forms;

namespace RCPA.Gui.FileArgument
{
  public class OpenFileArgument : AbstractFileArgument
  {
    private readonly OpenFileDialog fileDialog;

    public OpenFileArgument(String fileDescription, String extension)
      : base(fileDescription, extension)
    {
      this.fileDialog = new OpenFileDialog();
      this.fileDialog.AutoUpgradeEnabled = true;
      this.fileDialog.Filter = fileFilter;
    }

    public OpenFileArgument(String fileDescription, String[] extensions)
      : base(fileDescription, extensions)
    {
      this.fileDialog = new OpenFileDialog();
      this.fileDialog.AutoUpgradeEnabled = true;
      this.fileDialog.Filter = fileFilter;
    }

    public override FileDialog GetFileDialog()
    {
      this.fileDialog.Title = GetBrowseDescription();
      return this.fileDialog;
    }

    public override bool IsValid(string filename)
    {
      return (filename != null) && (filename.Trim().Length > 0) && new FileInfo(filename.Trim()).Exists;
    }

    public override string GetBrowseDescription()
    {
      return "Browse " + GetFileDescription() + " File ...";
    }

    public override bool IsSaving
    {
      get { return false; }
    }
  }
}