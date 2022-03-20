using RCPA.Gui.FileArgument;
using System;

namespace RCPA.Gui
{
  public partial class AbstractFileProcessorUI : AbstractProcessorFileUI
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