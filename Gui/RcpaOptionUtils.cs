using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public static class RcpaOptionUtils
  {
    public static void SaveToXml(object source, string optionFile)
    {
      XElement root = new XElement("Options");
      SaveToXml(source, root);
      root.Save(optionFile);
    }

    public static void LoadFromXml(object target, string optionFile)
    {
      if (File.Exists(optionFile))
      {
        XElement root = XElement.Load(optionFile);
        LoadFromXml(target, root);
      }
    }

    public static void SaveToXml(object source, XElement target)
    {
      Type type = source.GetType();

      List<MemberInfo> members = GetMemberInfos(type);

      foreach (var item in members)
      {
        object[] attributes = item.GetCustomAttributes(typeof(RcpaOption), false);
        foreach (RcpaOption fieldAttribute in attributes)
        {
          //获得相应属性的值
          object value = type.InvokeMember(item.Name, BindingFlags.GetProperty | BindingFlags.GetField, null, source, null);
          target.RemoveChild(fieldAttribute.Name);

          if (fieldAttribute.ValueType == RcpaOptionType.StringArray)
          {
            string[] values = (string[])value;
            target.Add(new XElement(fieldAttribute.Name,
              from v in values
              select new XElement("item", v)));
          }
          else if (fieldAttribute.ValueType == RcpaOptionType.StringList)
          {
            List<string> values = (List<string>)value;
            target.Add(new XElement(fieldAttribute.Name,
              from v in values
              select new XElement("item", v)));
          }
          else if (fieldAttribute.ValueType == RcpaOptionType.IXml)
          {
            IXml obj = value as IXml;
            var parent = new XElement(fieldAttribute.Name);
            obj.Save(parent);
            target.Add(parent);
          }
          else
            target.Add(new XElement(fieldAttribute.Name, value));
        }
      }
    }

    public static void LoadFromXml(object target, XElement source)
    {
      Type type = target.GetType();

      List<MemberInfo> members = GetMemberInfos(type);

      foreach (var item in members)
      {
        object[] attributes = item.GetCustomAttributes(typeof(RcpaOption), true);
        foreach (RcpaOption fieldAttribute in attributes)
        {
          var xmlElement = source.Element(fieldAttribute.Name);
          if (xmlElement != null)
          {
            object oldValue = type.InvokeMember(item.Name, BindingFlags.GetProperty | BindingFlags.GetField, null, target, null);
            switch (fieldAttribute.ValueType)
            {
              case RcpaOptionType.String:
                {
                  var xmlValue = xmlElement.Value;
                  type.InvokeMember(item.Name, BindingFlags.SetProperty | BindingFlags.SetField, null, target, new object[] { xmlValue });
                  break;
                }
              case RcpaOptionType.Int32:
                {
                  var xmlValue = Convert.ToInt32(xmlElement.Value);
                  type.InvokeMember(item.Name, BindingFlags.SetProperty | BindingFlags.SetField, null, target, new object[] { xmlValue });
                  break;
                }
              case RcpaOptionType.Double:
                {
                  var xmlValue = MyConvert.ToDouble(xmlElement.Value);
                  type.InvokeMember(item.Name, BindingFlags.SetProperty | BindingFlags.SetField, null, target, new object[] { xmlValue });
                  break;
                }
              case RcpaOptionType.Boolean:
                {
                  var xmlValue = Convert.ToBoolean(xmlElement.Value);
                  type.InvokeMember(item.Name, BindingFlags.SetProperty | BindingFlags.SetField, null, target, new object[] { xmlValue });
                  break;
                }
              case RcpaOptionType.StringArray:
                {
                  var xmlValue = (from n in xmlElement.Elements()
                                  select n.Value).ToArray();
                  type.InvokeMember(item.Name, BindingFlags.SetProperty | BindingFlags.SetField, null, target, new object[] { xmlValue });
                  break;
                }
              case RcpaOptionType.StringList:
                {
                  var xmlValue = (from n in xmlElement.Elements()
                                  select n.Value).ToList();
                  type.InvokeMember(item.Name, BindingFlags.SetProperty | BindingFlags.SetField, null, target, new object[] { xmlValue });
                  break;
                }
              case RcpaOptionType.IXml:
                {
                  IXml xmlValue = oldValue as IXml;
                  xmlValue.Load(xmlElement);
                  break;
                }
              default:
                throw new ArgumentException(MyConvert.Format("Unsupported RcpaOptionType {0} of field/property {1} in type {2}, only String/Int32/Double supported", fieldAttribute.ValueType, item.Name, type.Name));
            }
          }
        }
      }
    }

    private static List<MemberInfo> GetMemberInfos(Type type)
    {
      List<MemberInfo> members = new List<MemberInfo>();

      //foreach (var field in type.GetFields())
      //{
      //  members.Add(field);
      //}

      foreach (var property in type.GetProperties())
      {
        members.Add(property);
      }

      return members;
    }
  }
}
