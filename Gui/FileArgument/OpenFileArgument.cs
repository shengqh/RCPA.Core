using System;
using System.IO;
using System.Windows.Forms;

namespace RCPA.Gui.FileArgument
{
  public class OpenFileArgument : AbstractFileArgument
  {
    private readonly OpenFileDialog fileDialog;

    public OpenFileArgument(String fileDescription, String extension, bool multiselect = false)
      : base(fileDescription, extension)
    {
      this.fileDialog = new OpenFileDialog();
      this.fileDialog.AutoUpgradeEnabled = true;
      this.fileDialog.Filter = fileFilter;
      this.fileDialog.Multiselect = multiselect;
    }

    public OpenFileArgument(String fileDescription, String[] extensions, bool multiselect = false)
      : base(fileDescription, extensions)
    {
      this.fileDialog = new OpenFileDialog();
      this.fileDialog.AutoUpgradeEnabled = true;
      this.fileDialog.Filter = fileFilter;
      this.fileDialog.Multiselect = multiselect;
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