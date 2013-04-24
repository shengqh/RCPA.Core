using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RCPA.Converter;
using System.Windows.Forms;

namespace RCPA.Format
{
  public class TextFileReader<T> : IFileReader2<List<T>> where T : new()
  {
    private TextFileDefinition def;

    private List<AnnotationToPropertyConverter<T>> converters;

    private HashSet<string> annoNames;

    public TextFileReader(string defFileName)
    {
      this.def = new TextFileDefinition();
      this.def.ReadFromFile(defFileName);
      InitializeByDefinition();
    }

    private void InitializeByDefinition()
    {
      converters = new List<AnnotationToPropertyConverter<T>>();
      annoNames = new HashSet<string>();
      foreach (var item in def)
      {
        if (item.IsVisible())
        {
          converters.Add(new AnnotationToPropertyConverter<T>(item));
          annoNames.Add(item.AnnotationName);
        }
      }
    }

    #region IFileReader<List<T>> Members

    public List<T> ReadFromFile(string fileName)
    {
      List<T> result = new List<T>();
      using (StreamReader sr = new StreamReader(fileName))
      {
        string line = sr.ReadLine();
        var parts = line.Split(this.def.Delimiter);

        var annCount = (from p in parts
                        where annoNames.Contains(p)
                        select p).Count();

        if (annCount == 0)
        {
          throw new Exception("Cannot find header information in file " + fileName + "\nIs file wrong?");
        }

        var reader = new AnnotationConverterFactory().GetConverters(line, this.def.Delimiter);
        Annotation ann = new Annotation();
        while ((line = sr.ReadLine()) != null)
        {
          if (line.Trim() == string.Empty)
          {
            continue;
          }

          ann.Annotations.Clear();
          reader.SetProperty(ann, line);

          T t = new T();
          bool bError = false;
          foreach (var conv in converters)
          {
            if (!conv.AnnotationToProperty(ann, t))
            {
              Console.WriteLine("Required entry {0} missed in line {1}, ignored.", conv.Item.AnnotationName, line);
              bError = true;
              break;
            }
          }
          if (!bError)
          {
            result.Add(t);
          }
        }
      }

      return result;
    }

    #endregion

    #region IFileReader2<List<T>> Members

    public string GetName()
    {
      return Path.GetFileNameWithoutExtension( def.DefinitionFile);
    }

    public string GetFormatFile()
    {
      return def.DefinitionFile;
    }

    #endregion

    public override string ToString()
    {
      if (string.IsNullOrEmpty(def.Description))
      {
        return GetName();
      }
      else
      {
        return GetName() + " : " + def.Description;
      }
    }
  }
}
