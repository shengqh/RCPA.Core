using System.ComponentModel;

namespace RCPA.Gui
{
  public partial class AbstractMultipleProgressFileProcessorUI : AbstractFileProcessorUI
  {
    public AbstractMultipleProgressFileProcessorUI()
    {
      InitializeComponent();
    }

    protected override ProgressChangedEventHandler GetProgressChanged()
    {
      return new WorkerProgressChangedProxy(new[] { lblProgress, this.lblSecondProgress }, new[] { progressBar }).ProgressChanged;
    }
  }
}