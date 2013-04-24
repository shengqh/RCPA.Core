using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;

namespace RCPA
{
  public interface IOptionFile
  {
    void RemoveFromXml(XElement option);

    void LoadFromXml(XElement option);

    void SaveToXml(XElement option);
  }
}
