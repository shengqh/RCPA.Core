using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;

namespace RCPA.Gui
{
  public partial class AbstractFileProcessorUI : AbstractProcessorUI
  {
    protected IRcpaFileDirectoryComponent originalFile;

    public AbstractFileProcessorUI()
    {
      InitializeComponent();

      base.AfterLoadOption += MyAfterLoadOption;
    }

    public virtual void SetFileArgument(string key, IFileArgument fileArgument)
    {
      if (this.originalFile != null)
      {
        this.originalFile.RemoveClickEvent();
        RemoveComponent(this.originalFile);
      }

      this.originalFile = new RcpaFileField(this.btnOriginalFile, this.txtOriginalFile, key, fileArgument, true);
      AddComponent(this.originalFile);
    }

    public virtual void SetDirectoryArgument(string key, string description)
    {
      if (this.originalFile != null)
      {
        this.originalFile.RemoveClickEvent();
        RemoveComponent(this.originalFile);
      }

      this.originalFile = new RcpaDirectoryField(this.btnOriginalFile, this.txtOriginalFile, key, description, true);
      AddComponent(this.originalFile);
    }

    protected override string GetOriginFile()
    {
      if (originalFile != null)
      {
        return this.originalFile.FullName;
      }
      else
      {
        throw new Exception("GetOriginFile not implemented.");
      }
    }
  }
}