using System;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class InputDirectoryForm : Form
  {
    private readonly RcpaDirectoryField valueField;

    public InputDirectoryForm()
      : this("Browse directory", "Directory", "")
    {
    }

    public InputDirectoryForm(string caption, string description, string defaultValue)
    {
      InitializeComponent();

      Text = caption;
      this.valueField = new RcpaDirectoryField(this.btnBrowse, this.txtValue, "", description, true);
      this.valueField.FullName = defaultValue;
    }

    public string Value
    {
      get { return this.valueField.FullName; }
      set { this.valueField.FullName = value; }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      try
      {
        this.valueField.ValidateComponent();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }
  }
}