using System.ComponentModel;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class MultipleProgressField : UserControl
  {
    private WorkerProgressChangedProxy proxy;

    public MultipleProgressField()
    {
      InitializeComponent();

      proxy = new WorkerProgressChangedProxy(new[] { lblProgress2, lblProgress1 }, new[] { progressBar });
    }

    [Localizable(true)]
    [Category("Message1"), DefaultValue("")]
    public string Message1
    {
      get
      {
        return lblProgress1.Text;
      }
      set
      {
        lblProgress1.Text = value;
      }
    }

    [Localizable(true)]
    [Category("Message2"), DefaultValue("")]
    public string Message2
    {
      get
      {
        return lblProgress2.Text;
      }
      set
      {
        lblProgress2.Text = value;
      }
    }

    [Localizable(true)]
    [Category("Position"), DefaultValue(1)]
    public int Position
    {
      get
      {
        return progressBar.Value;
      }
      set
      {
        progressBar.Value = value;
      }
    }

    public ProgressChangedEventHandler ProgressChangedHander
    {
      get
      {
        return proxy.ProgressChanged;
      }
    }
  }
}
