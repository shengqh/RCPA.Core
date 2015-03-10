using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA
{
  public interface IRTemplateProcessorOptions
  {
    string InputFile { get; set; }

    string OutputFile { get; set; }

    string RExecute { get; set; }

    string RTemplate { get; set; }

    List<string> Parameters { get; set; }

    bool CreateNoWindow { get; set; }

    bool NoResultFile { get; set; }
  }

  public class RTemplateProcessorOptions : IRTemplateProcessorOptions
  {
    public RTemplateProcessorOptions()
    {
      this.Parameters = new List<string>();
      this.CreateNoWindow = true;
      this.NoResultFile = false;
    }

    public string InputFile { get; set; }

    public string OutputFile { get; set; }

    public string RExecute { get; set; }

    public string RTemplate { get; set; }

    public List<string> Parameters { get; set; }

    public bool CreateNoWindow { get; set; }

    public bool NoResultFile { get; set; }
  }
}
