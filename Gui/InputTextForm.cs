using System;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;
using System.Xml.Linq;
using System.IO;

namespace RCPA.Gui
{
  public partial class InputTextForm : Form
  {
    private readonly string configFileName;

    private readonly RcpaTextField valueField;

    public InputTextForm()
    {
      InitializeComponent();
    }

    public InputTextForm(string configFilename, string key, string caption, string description, string defaultValue,
                         bool needSkipButton)
      : this()
    {
      this.configFileName = configFilename;
      Text = caption;
      this.lblDescription.Text = description;

      this.valueField = new RcpaTextField(this.txtValue, key, description, defaultValue, false);

      this.btnSkip.Visible = needSkipButton;

      LoadOption();
    }

    public string Value
    {
      get { return this.valueField.Text; }
      set { this.valueField.Text = value; }
    }

    private XElement GetOption()
    {
      XElement option;
      if (File.Exists(configFileName))
      {
        option = XElement.Load(this.configFileName);
      }
      else
      {
        option = new XElement("configuration");
      }
      return option;
    }

    private void LoadOption()
    {
      if (this.configFileName == null)
      {
        return;
      }

      try
      {
        XElement option = GetOption();

        this.valueField.LoadFromXml(option);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("Load option error : " + ex.Message);
      }
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

      SaveOption();
      DialogResult = DialogResult.OK;
      Close();
    }

    private void SaveOption()
    {
      if (this.configFileName == null)
      {
        return;
      }

      try
      {
        XElement option = GetOption();

        this.valueField.RemoveFromXml(option);

        this.valueField.SaveToXml(option);

        option.Save(configFileName);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("Save option error : " + ex.Message);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void btnSkip_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Ignore;
    }

    private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 13)
      {
        btnOk.PerformClick();
      }
    }
  }
}