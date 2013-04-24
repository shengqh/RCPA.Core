using System;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class InputDoubleTextForm : Form
  {
    private readonly RcpaTextField value1Field;

    private readonly RcpaTextField value2Field;

    public InputDoubleTextForm()
    {
      InitializeComponent();
    }

    public InputDoubleTextForm(string caption, string description1, string defaultValue1, string description2,
                               string defaultValue2)
      : this()
    {
      Text = caption;
      this.lblDescription.Text = description1;
      this.value1Field = new RcpaTextField(this.txtValue1, "", description1, defaultValue1, true);
      this.label1.Text = description2;
      this.value2Field = new RcpaTextField(this.txtValue2, "", description2, defaultValue2, true);
    }

    public string Value1
    {
      get { return this.value1Field.Text; }
      set { this.value1Field.Text = value; }
    }

    public string Value2
    {
      get { return this.value2Field.Text; }
      set { this.value2Field.Text = value; }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      try
      {
        this.value1Field.ValidateComponent();
        this.value2Field.ValidateComponent();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }
  }
}