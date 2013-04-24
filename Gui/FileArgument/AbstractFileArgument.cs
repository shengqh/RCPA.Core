using System;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui.FileArgument
{
  public abstract class AbstractFileArgument : IFileArgument
  {
    private readonly String fileDescription;
    protected String fileFilter;

    public AbstractFileArgument(String fileDescription, String extension)
    {
      this.fileDescription = fileDescription;

      this.fileFilter = MyConvert.Format("{0}(*.{1})|*.{1}|All Files(*.*)|*.*", fileDescription, GetExtension(extension));
    }

    public AbstractFileArgument(String fileDescription, String[] extensions)
    {
      this.fileDescription = fileDescription;

      var sb = new StringBuilder();
      for (int i = 0; i < extensions.Length; i++)
      {
        if (0 != i)
        {
          sb.Append(";");
        }
        sb.Append("*." + GetExtension(extensions[i]));
      }

      this.fileFilter = fileDescription + "(" + sb + ")|" + sb + "|All Files(*.*)|*.*";
    }

    #region IFileArgument Members

    public string GetFileDescription()
    {
      return this.fileDescription;
    }

    public abstract string GetBrowseDescription();

    public abstract FileDialog GetFileDialog();

    public abstract bool IsValid(string filename);

    #endregion

    private static string GetExtension(String extension)
    {
      string ext;
      if (extension.StartsWith("."))
      {
        ext = extension.Substring(1);
      }
      else
      {
        ext = extension;
      }
      return ext;
    }

    #region IFileArgument Members


    public abstract bool IsSaving { get; }

    #endregion
  }
}