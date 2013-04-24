using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;
using RCPA.Utils;

namespace RCPA.Gui
{
  public class RcpaFileField : RcpaTextField, IRcpaFileDirectoryComponent
  {
    private readonly Button btnFile;

    private readonly IFileArgument fileArgument;

    private readonly EventHandler myEvent;

    public RcpaFileField(Button btnFile, TextBox txtValue, String key, IFileArgument fileArgument, bool required)
      : this(btnFile, txtValue, key, fileArgument, string.Empty, required)
    { }

    public RcpaFileField(Button btnFile, TextBox txtValue, String key, IFileArgument fileArgument, string initFileName, bool required)
      : base(txtValue, key, fileArgument.GetFileDescription(), initFileName, required)
    {
      this.fileArgument = fileArgument;

      this.btnFile = btnFile;
      this.btnFile.Text = fileArgument.GetBrowseDescription();
      this.myEvent = btnFileClick;
      this.btnFile.Click += this.myEvent;
      base.ValidateFunc = DoValidate;
    }

    public override bool Enabled
    {
      get
      {
        return btnFile.Enabled;
      }
      set
      {
        btnFile.Enabled = value;
        txtValue.Enabled = value;
      }
    }

    protected override string GetValidateError()
    {
      return "Input " + fileArgument.GetFileDescription() + " file";
    }

    private bool DoValidate(string text)
    {
      if (this.fileArgument.IsSaving)
      {
        return text != null && text.Length != 0;
      }

      return File.Exists(text);
    }

    public EventHandler AfterBrowseFileEvent { get; set; }

    public string FullName
    {
      get { return Text.Trim(); }
      set { Text = value; }
    }

    public bool Exists
    {
      get
      {
        string value = FullName;
        return value.Length > 0 && new FileInfo(value).Exists;
      }
    }

    public void RemoveClickEvent()
    {
      this.btnFile.Click -= this.myEvent;
    }

    public FileDialog GetDialog()
    {
      return this.fileArgument.GetFileDialog();
    }

    private void btnFileClick(object sender, EventArgs e)
    {
      FileDialog dialog = this.fileArgument.GetFileDialog();
      if (System.IO.File.Exists(FullName))
      {
        dialog.FileName = FullName;
      }

      if (dialog.ShowDialog(this.btnFile) == DialogResult.OK)
      {
        FullName = dialog.FileName;

        if (AfterBrowseFileEvent != null)
        {
          AfterBrowseFileEvent(FullName, e);
        }
      }
    }
  }
}