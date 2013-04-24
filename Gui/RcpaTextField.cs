using System;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaTextField : AbstractRcpaComponent
  {
    private readonly string defaultValue;

    private readonly string title;

    protected TextBox txtValue;

    public RcpaTextField(TextBox txtValue, string key, string title, string defaultValue, bool required)
    {
      this.txtValue = txtValue;
      this.title = title;
      this.defaultValue = defaultValue;
      this.Required = required;
      this.txtValue.Text = defaultValue;

      Adaptor = new OptionFileTextBoxAdaptor(txtValue, key, defaultValue);
    }

    protected override void DoPreConditionChanged(object sender, EventArgs e)
    { 
      if (sender is CheckBox)
      {
        var cb = sender as CheckBox;
        txtValue.Enabled = cb.Enabled && cb.Checked;
      }
    }

    public bool Required { get; set; }

    public Func<string, bool> ValidateFunc { get; set; }

    public override bool Enabled
    {
      get
      {
        return txtValue.Enabled;
      }
      set
      {
        txtValue.Enabled = value;
      }
    }

    public string Text
    {
      get { return this.txtValue.Text.Trim(); }
      set { this.txtValue.Text = value; }
    }

    protected virtual string GetValidateError()
    {
      var result = "Input " + title;
      
      if (defaultValue != string.Empty)
      {
        result = result + ", default value = " + defaultValue;
      }

      return result;
    }

    protected virtual bool Validate(string text)
    {
      if (ValidateFunc == null)
      {
        return true;
      }

      var result = ValidateFunc(text);

      if (!result)
      {
        Error.SetError(txtValue, GetValidateError());
      }
      else
      {
        Error.SetError(txtValue, string.Empty);
      }

      return result;
    }

    #region IRcpaComponent Members

    public override void ValidateComponent()
    {
      if (HasPrecondition)
      {
        if (!PreconditionPassed && Text.Length == 0)
        {
          return;
        }
      }
      else
      {
        if (!this.Required && Text.Length == 0)
        {
          return;
        }
      }

      if (Text.Length == 0)
      {
        this.txtValue.Focus();
        throw new InvalidOperationException("Input " + this.title + " first");
      }

      if (!Validate(Text))
      {
        this.txtValue.Focus();
        throw new InvalidOperationException(MyConvert.Format("{0} is not a valid input for {1}. DefaultValue = {2}", Text, this.title, this.defaultValue));
      }
    }

    #endregion
  }
}