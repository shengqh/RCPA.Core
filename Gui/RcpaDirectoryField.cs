using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using RCPA.Utils;

namespace RCPA.Gui
{
  public class RcpaDirectoryField : RcpaTextField, IRcpaFileDirectoryComponent
  {
    private string dirDescription;

    private readonly Button btnDirectory;

    private readonly EventHandler myEvent;

    private FolderBrowserDialog dialog;
    
    public RcpaDirectoryField(Button btnDir, TextBox txtValue, String key, String dirDescription, bool required)
      : base(txtValue, key, dirDescription, string.Empty, required)
    {
      this.dirDescription = dirDescription;

      this.btnDirectory = btnDir;
      this.btnDirectory.Text = "Browse " + dirDescription + " Directory ...";

      this.myEvent = btnDirectoryClick;
      this.btnDirectory.Click += this.myEvent;

      base.ValidateFunc = DoValidate;
    }

    protected override string GetValidateError()
    {
      return "Input " + dirDescription + " directory";
    }

    private bool DoValidate(string text)
    {
      return Directory.Exists(text);
    }

    public bool Exists
    {
      get
      {
        return Directory.Exists(FullName);
      }
    }

    public string FullName
    {
      get { return Text.Trim(); }
      set { Text = value; }
    }

    public override bool Enabled
    {
      get
      {
        return btnDirectory.Enabled;
      }
      set
      {
        btnDirectory.Enabled = value;
        txtValue.Enabled = value;
      }
    }

    public void RemoveClickEvent()
    {
      this.btnDirectory.Click -= this.myEvent;
    }

    private void btnDirectoryClick(object sender, EventArgs e)
    {
      if (dialog == null)
      {
        dialog = new FolderBrowserDialog();
        dialog.Description = dirDescription;
      }

      if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
      {
        FullName = dialog.SelectedPath;
      }
    }
  }
}