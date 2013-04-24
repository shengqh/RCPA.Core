using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;

namespace RCPA.Gui
{
  public partial class AbstractTurboProcessorUI : AbstractMultipleProgressFileProcessorUI
  {
    protected RcpaDirectoryField batchMode;
    protected RcpaFileField singleMode;

    public AbstractTurboProcessorUI()
    {
      InitializeComponent();
      btnOriginalFile.Visible = false;
      txtOriginalFile.Visible = false;
      AfterLoadOption += tcBatchMode_SelectedIndexChanged;
    }

    public override void SetFileArgument(string key, IFileArgument fileArgument)
    {
      if (this.singleMode != null)
      {
        this.singleMode.RemoveClickEvent();
        RemoveComponent(this.singleMode);
      }

      this.singleMode = new RcpaFileField(this.btnSingle, this.txtSingle, key, fileArgument, true);
      AddComponent(this.singleMode);
    }

    public override void SetDirectoryArgument(string key, string description)
    {
      if (this.batchMode != null)
      {
        this.batchMode.RemoveClickEvent();
        RemoveComponent(this.batchMode);
      }

      this.batchMode = new RcpaDirectoryField(this.btnBatch, this.txtBatch, key, description, true);
      AddComponent(this.batchMode);
    }

    protected bool IsBatchMode()
    {
      return 1 == this.tcBatchMode.SelectedIndex;
    }

    private void tcBatchMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!IsDesignTime())
      {
        if (null == this.singleMode || null == this.batchMode)
        {
          MessageBox.Show(this, "Call SetFileArgument and SetDirectoryArgument in construction first!", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (IsBatchMode())
        {
          SetComponentEnabled(this.singleMode, false);
          SetComponentEnabled(this.batchMode, true);
          base.originalFile = this.batchMode;
          lblSecondProgress.Visible = true;
        }
        else if (this.singleMode != null)
        {
          SetComponentEnabled(this.singleMode, true);
          SetComponentEnabled(this.batchMode, false);
          base.originalFile = this.singleMode;
          lblSecondProgress.Visible = false;
        }
      }
    }

    private bool IsDesignTime()
    {
      object obj = GetService(typeof (IDesignerHost));
      return obj != null;
    }
  }
}