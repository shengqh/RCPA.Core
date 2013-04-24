using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Format
{
  public interface IFileDefinition
  {
    void ReadFromFile(string fileName);

    void WriteToFile(string fileName);

    void WriteSampleFile(string fileName);
  }
}
