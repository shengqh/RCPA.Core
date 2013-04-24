using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.IO
{
  public static class FileExtension
  {
    public static string ReadLine(this FileStream fs)
    {
      StringBuilder sb = new StringBuilder();
      int c;
      while (-1 != (c = fs.ReadByte()))
      {
        if ('\r' == c)
        {
          continue;
        }
        else if ('\n' == c)
        {
          return sb.ToString();
        }
        else
        {
          sb.Append((char)c);
        }
      }

      if (sb.Length > 0)
      {
        return sb.ToString();
      }
      else
      {
        return null;
      }
    }
  }
}
