using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;
using RCPA.Utils;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaFormState : IRcpaComponent
  {
    private Form form;

    public RcpaFormState(Form form)
    {
      this.form = form;
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    { }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(XElement option)
    {
      option.RemoveChild("FormState");
    }

    public void SaveToXml(XElement option)
    {
      option.Add(new XElement("FormState",
        new XElement("Width", form.Width),
        new XElement("Height", form.Height),
        new XElement("WindowState", form.WindowState.ToString())));
    }

    public void LoadFromXml(XElement option)
    {
      XElement state = option.Element("FormState");
      if (null != state)
      {
        form.WindowState = (FormWindowState)Enum.Parse(FormWindowState.Maximized.GetType(), state.GetChildValue("WindowState", form.WindowState.ToString()));
        if (form.WindowState == FormWindowState.Normal)
        {
          form.Width = state.GetChildValue("Width", form.Width);
          form.Height = state.GetChildValue("Height", form.Height);
        }
      }
    }

    #endregion
  }
}