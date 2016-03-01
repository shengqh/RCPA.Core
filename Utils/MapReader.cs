using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RCPA.Utils
{
  public class MapReader
  {
    private string key = string.Empty;
    private string value = string.Empty;
    private char delimiter;

    private List<int> keyIndecies = null;
    private int valueIndex = -1;
    private bool hasHeader = true;

    public MapReader(string key, string value, char delimiter = '\t')
    {
      this.key = key;
      this.value = value;
      this.delimiter = delimiter;
    }

    public MapReader(int keyIndex, int valueIndex, char delimiter = '\t', bool hasHeader = true)
      : this(new[] { keyIndex }, valueIndex, delimiter, hasHeader)
    { }

    public MapReader(int[] keyIndecies, int valueIndex, char delimiter = '\t', bool hasHeader = true)
    {
      this.keyIndecies = keyIndecies.ToList();
      this.valueIndex = valueIndex;
      this.delimiter = delimiter;
      this.hasHeader = hasHeader;
    }

    public Dictionary<string, string> ReadFromFile(string fileName)
    {
      Dictionary<string, string> result = new Dictionary<string, string>();
      using (StreamReader sr = new StreamReader(fileName))
      {
        string line;
        if (keyIndecies == null)
        {
          line = sr.ReadLine();
          var parts = line.Split(this.delimiter);
          keyIndecies = new List<int>();

          for (int i = 0; i < parts.Length; i++)
          {
            if (parts[i].Equals(key) || parts[i].Equals("\"" + key + "\""))
            {
              keyIndecies.Add(i);
            }
          }

          if (keyIndecies.Count == 0)
          {
            throw new ArgumentException(string.Format("Cannot find key column {0} in file {1}", key, fileName));
          }

          valueIndex = Array.IndexOf(parts, value);
          if (valueIndex == -1)
          {
            valueIndex = Array.IndexOf(parts, "\"" + value + "\"");
            if (valueIndex == -1)
            {
              throw new ArgumentException(string.Format("Cannot find value column {0} in file {1}", value, fileName));
            }
          }
        }
        else if (hasHeader)
        {
          line = sr.ReadLine();
        }

        while ((line = sr.ReadLine()) != null)
        {
          if (string.IsNullOrWhiteSpace(line))
          {
            continue;
          }

          var curParts = line.Split(this.delimiter);
          var curValue = curParts[valueIndex];

          foreach (var index in keyIndecies)
          {
            var key = GetValue(curParts[index]);

            result[key] = GetValue(curValue);
          }
        }
      }

      return result;
    }

    private string GetValue(string v)
    {
      if (v.StartsWith("\"") && v.EndsWith("\""))
      {
        return v.Substring(1, v.Length - 2);
      }
      return v;
    }
  }
}
