using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaComponentList : Dictionary<IRcpaComponent, bool>, IRcpaComponent
  {
    public void ResetEnabledByPrecondition()
    {
      HashSet<object> rc = new HashSet<object>();
      foreach (var comp in this.Keys)
      {
        if (!(comp is IDependentRcpaComponent))
        {
          rc.Add(comp);
          continue;
        }

        var cp = comp as IDependentRcpaComponent;
        if (cp.PreCondition == null)
        {
          rc.Add(comp);
        }
      }

      while (rc.Count != this.Count)
      {
        foreach (IRcpaComponent comp in Keys)
        {
          if (rc.Contains(comp))
          {
            continue;
          }

          var cp = comp as IDependentRcpaComponent;
          if (cp.PreCondition is IRcpaComponent)
          {
            if ((!this.ContainsKey(cp.PreCondition as IRcpaComponent)) || rc.Contains(cp.PreCondition))
            {
              cp.Enabled = cp.PreCondition.Checked;
              rc.Add(comp);
            }
          }
          else
          {
            cp.Enabled = cp.PreCondition.Checked;
            rc.Add(comp);
          }
        }
      }
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    {
      foreach (IRcpaComponent comp in Keys)
      {
        if (this[comp])
        {
          comp.ValidateComponent();
        }
      }
    }

    public void LoadFromXml(XElement option)
    {
      foreach (IRcpaComponent comp in Keys)
      {
        comp.LoadFromXml(option);
      }
    }

    public void RemoveFromXml(XElement option)
    {
      foreach (IRcpaComponent comp in Keys)
      {
        comp.RemoveFromXml(option);
      }
    }

    public void SaveToXml(XElement option)
    {
      foreach (IRcpaComponent comp in Keys)
      {
        comp.SaveToXml(option);
      }
    }

    #endregion
  }
}