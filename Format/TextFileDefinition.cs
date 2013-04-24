using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace RCPA.Format
{
  public class FileDefinitionItem
  {
    public string AnnotationName { get; set; }
    public string PropertyName { get; set; }
    public bool Required { get; set; }
    public string ValueType { get; set; }
    public string Format { get; set; }
    public string Example { get; set; }

    public override string ToString()
    {
      return PropertyName;
    }

    public bool IsVisible()
    {
      return !string.IsNullOrEmpty(AnnotationName) && !string.IsNullOrEmpty(PropertyName) && !string.IsNullOrEmpty(ValueType);
    }
  }

  public class DefaultValue
  {
    public string PropertyName { get; set; }
    public string Value { get; set; }
  }

  public class TextFileDefinition : List<FileDefinitionItem>, IFileDefinition
  {
    public string Description { get; set; }

    public char Delimiter { get; set; }

    public string DefinitionFile { get; private set; }

    public List<DefaultValue> DefaultValues { get; set; }

    public string DelimiterString
    {
      get
      {
        if (Delimiter == '\t')
        {
          return "tab";
        }
        else
        {
          return Delimiter.ToString();
        }
      }
      set
      {
        if (string.IsNullOrEmpty(value) || value.ToLower().Equals("tab"))
        {
          Delimiter = '\t';
        }
        else
        {
          Delimiter = value[0];
        }
      }
    }

    public TextFileDefinition()
    {
      Delimiter = ',';
      Description = "Text File Definition Description";
      DefinitionFile = string.Empty;
      DefaultValues = new List<DefaultValue>();
    }

    #region IFileDefinition<T> Members

    public void ReadFromFile(string fileName)
    {
      XElement root = XElement.Load(fileName);

      DelimiterString = root.Element("Delimiter").Value;
      Description = root.Element("Description").Value;

      Clear();

      AddRange(from c in root.Elements("Item")
               select new FileDefinitionItem()
               {
                 AnnotationName = c.Element("Header").Value,
                 PropertyName = c.Element("PropertyName").Value,
                 Required = Convert.ToBoolean(c.Element("Required").Value),
                 ValueType = c.Element("ValueType").Value,
                 Format = c.Element("Format").Value,
                 Example = c.Element("Example").Value
               });

      DefaultValues = (from c in root.Elements("DefaultValue")
                       select new DefaultValue()
                       {
                         PropertyName = c.Element("PropertyName").Value,
                         Value = c.Element("Value").Value
                       }).ToList();

      DefinitionFile = fileName;
    }

    public void WriteToFile(string fileName)
    {
      XElement root = new XElement("FileDefinition",
        new XElement("Delimiter", this.DelimiterString),
        new XElement("Description", this.Description),
        from item in this
        select new XElement("Item",
          new XElement("Header", item.AnnotationName),
          new XElement("PropertyName", item.PropertyName),
          new XElement("Required", item.Required.ToString()),
          new XElement("ValueType", item.ValueType),
          new XElement("Format", item.Format),
          new XElement("Example", item.Example)),
        from dv in this.DefaultValues
        select new XElement("DefaultValue",
          new XElement("PropertyName", dv.PropertyName),
          new XElement("Value", dv.Value))
              );
      root.Save(fileName);

      DefinitionFile = fileName;
    }

    public void WriteSampleFile(string fileName)
    {
      var line = (from item in this
                  select item.AnnotationName).Merge(this.Delimiter);
      File.WriteAllText(fileName, line);
    }

    #endregion
  }
}
