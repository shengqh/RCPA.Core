using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public abstract class AbstractRcpaComponent : IDependentRcpaComponent
  {
    public IOptionFile Adaptor { get; set; }

    private CheckBox _preCondition;

    private List<Control> _childrens = new List<Control>();

    public List<Control> Childrens { get { return _childrens; } }

    protected virtual void DoPreConditionChanged(object sender, EventArgs e)
    {
      if (_preCondition != null)
      {
        this.Enabled = _preCondition.Checked;
      }
    }

    public CheckBox PreCondition
    {
      get
      {
        return _preCondition;
      }
      set
      {
        if (_preCondition != null)
        {
          _preCondition.CheckedChanged -= DoPreConditionChanged;
        }

        _preCondition = value;

        if (_preCondition != null)
        {
          _preCondition.CheckedChanged += DoPreConditionChanged;
        }
      }
    }

    protected bool HasPrecondition
    {
      get
      {
        return PreCondition != null;
      }
    }

    protected bool PreconditionPassed
    {
      get
      {
        return (PreCondition == null) || PreCondition.Checked;
      }
    }

    #region IRcpaComponent Members

    public virtual void ValidateComponent()
    {
      Error.Clear();
    }

    public virtual bool Enabled
    {
      get
      {
        if (Childrens.Count == 0)
        {
          return true;
        }
        else
        {
          return Childrens[0].Enabled;
        }
      }
      set
      {
        foreach (var child in Childrens)
        {
          child.Enabled = value;
        }
      }
    }

    #endregion

    private ErrorProvider error = new ErrorProvider();

    protected ErrorProvider Error { get { return error; } }

    #region IOptionFile Members

    //public void LoadFromFile(System.Configuration.Configuration option)
    //{
    //  Adaptor.LoadFromFile(option);
    //}

    //public void RemoveFromFile(System.Configuration.Configuration option)
    //{
    //  Adaptor.RemoveFromFile(option);
    //}

    //public void SaveToFile(System.Configuration.Configuration option)
    //{
    //  Adaptor.SaveToFile(option);
    //}

    public void RemoveFromXml(System.Xml.Linq.XElement option)
    {
      Adaptor.RemoveFromXml(option);
    }

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      Adaptor.LoadFromXml(option);
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      Adaptor.SaveToXml(option);
    }

    #endregion
  }
}
