using RCPA.Utils;
using System.Threading;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class ProgressField : UserControl, IProgressCallback
  {
    private ProgressProxy proxy;

    private CancellationTokenSource cancellation = new CancellationTokenSource();

    public ProgressField()
    {
      InitializeComponent();

      proxy = new ProgressProxy(this, null, null, lblProgress1, progressBar);
    }

    public CancellationToken Token
    {
      get
      {
        return cancellation.Token;
      }
    }

    public void Cancel()
    {
      cancellation.Cancel();
    }

    public void SetMessage(string message)
    {
      proxy.SetMessage(message);
    }

    public void SetPosition(long position)
    {
      proxy.SetPosition(position);
    }

    #region IProgressCallback Members

    public bool IsCancellationPending()
    {
      return cancellation.IsCancellationRequested || proxy.IsCancellationPending();
    }

    public void Begin()
    {
      cancellation = new CancellationTokenSource();
      proxy.Begin();
    }

    public void SetRange(long minimum, long maximum)
    {
      proxy.SetRange(minimum, maximum);
    }

    public void SetRange(int progressBarIndex, long minimum, long maximum)
    {
      proxy.SetRange(progressBarIndex, minimum, maximum);
    }

    public void Increment(long value)
    {
      proxy.Increment(value);
    }

    public void Increment(int progressBarIndex, long value)
    {
      proxy.Increment(progressBarIndex, value);
    }

    public void SetPosition(int progressBarIndex, long position)
    {
      proxy.SetPosition(progressBarIndex, position);
    }

    public void SetMessage(string format, params object[] args)
    {
      proxy.SetMessage(format, args);
    }

    public void SetMessage(int labelIndex, string message)
    {
      proxy.SetMessage(labelIndex, message);
    }

    public void SetMessage(int labelIndex, string format, params object[] args)
    {
      proxy.SetMessage(labelIndex, format, args);
    }

    public void End()
    {
      proxy.End();
    }

    public bool IsConsole()
    {
      return false;
    }

    #endregion
  }
}
