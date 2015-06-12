using System;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using DigitalRune.Windows.Docking;
using System.ComponentModel;

namespace RCPA.Gui
{
  public partial class ComponentUI : DockableForm
  {
    private readonly RcpaComponentList componentList = new RcpaComponentList();

    public ComponentUI()
    {
      InitializeComponent();

      if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
      {
        ConfigFileName = FileUtils.GetConfigFile(GetType());
      }

      AddComponent(new RcpaFormState(this));
    }

    protected void RegisterRcpaComponent()
    {
      DoRegisterRcpaComponent(this);
    }

    protected void DoRegisterRcpaComponent(Control parent)
    {
      if(parent.HasChildren)
      {
        foreach (Control comp in parent.Controls)
        {
          if (comp is IRcpaComponent)
          {
            AddComponent(comp as IRcpaComponent);
          }
          else
          {
            DoRegisterRcpaComponent(comp);
          }
        }
      }
    }

    public string ConfigFileName { get; set; }

    public event EventHandler AfterLoadOption;

    protected void AddComponent(IRcpaComponent comp)
    {
      if (comp == null)
      {
        throw new ArgumentException("Argument cannot be null.");
      }

      this.componentList[comp] = true;
    }

    protected void RemoveComponent(IRcpaComponent comp)
    {
      this.componentList.Remove(comp);
    }

    protected void SetComponentEnabled(IRcpaComponent comp, bool enabled)
    {
      this.componentList[comp] = enabled;
    }

    protected virtual void DoBeforeValidate()
    {
    }

    protected virtual void ValidateComponents()
    {
      DoBeforeValidate();
      this.componentList.ValidateComponent();
    }

    public virtual void LoadOption()
    {
      try
      {
        XElement option = GetOption();

        RegisterRcpaComponent();

        this.componentList.LoadFromXml(option);

        RcpaOptionUtils.LoadFromXml(this, option);

        this.componentList.ResetEnabledByPrecondition();
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("Load option error : " + ex.Message);
      }
      OnAfterLoadOption(new EventArgs());
    }

    protected virtual void OnAfterLoadOption(EventArgs e)
    {
      if (null != AfterLoadOption)
      {
        AfterLoadOption(this, e);
      }
    }

    public virtual void SaveOption()
    {
      try
      {
        XElement option = GetOption();

        XElement appSetting = option.Element("appSettings");
        if (appSetting != null)
        {
          appSetting.Remove();
        }

        this.componentList.RemoveFromXml(option);
        this.componentList.SaveToXml(option);

        RcpaOptionUtils.SaveToXml(this, option);

        option.Save(ConfigFileName);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("Save option error : " + ex.Message);
      }
    }

    protected XElement GetOption()
    {
      XElement option;
      if (File.Exists(ConfigFileName))
      {
        option = XElement.Load(this.ConfigFileName, LoadOptions.SetBaseUri);
      }
      else
      {
        option = new XElement("configuration");
      }
      return option;
    }

    public DialogResult MyShowDialog()
    {
      LoadOption();
      StartPosition = FormStartPosition.CenterScreen;
      return ShowDialog();
    }

    public DialogResult MyShowDialog(IWin32Window owner)
    {
      LoadOption();
      StartPosition = FormStartPosition.CenterScreen;
      return ShowDialog(owner);
    }

    public void MyShow()
    {
      LoadOption();
      StartPosition = FormStartPosition.CenterScreen;
      Show();
    }
  }
}