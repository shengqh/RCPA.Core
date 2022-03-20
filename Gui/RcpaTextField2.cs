using System;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaTextField2<T> : AbstractRcpaComponent where T : Control
  {
    private readonly string defaultValue;

    private readonly string title;

    protected T txtValue;

    protected Func<T, string> getFunc;

    protected Action<T, string> setFunc;

    public RcpaTextField2(T txtValue, string key, string title, string defaultValue, bool required, Func<T, string> getFunc, Action<T, string> setFunc)
    {
      this.txtValue = txtValue;
      this.title = title;
      this.defaultValue = defaultValue;
      this.Required = required;
      this.getFunc = getFunc;
      this.setFunc = setFunc;
      setFunc(txtValue, defaultValue);

      Adaptor = new OptionFileTextAdaptor<T>(txtValue, key, defaultValue, getFunc, setFunc);

      Childrens.Add(txtValue);
    }

    public bool Required { get; set; }

    public Func<string, bool> ValidateFunc { get; set; }

    public string Text
    {
      get { return getFunc(this.txtValue).Trim(); }
      set { setFunc(this.txtValue, value); }
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
      if (!this.Required && Text.Length == 0)
      {
        return;
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