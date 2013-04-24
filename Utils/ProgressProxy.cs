using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Utils
{
  public class ProgressProxy : AbstractProgressCallback
  {
    private Control rootControl;
    private Button cancelButton;
    private Button startButton;
    private Label label;
    private ProgressBar progressBar;

    private long _minimum;
    private long _maximum;
    private double _range;
    private long _position;
    private int _lastPercentage;

    public ProgressProxy(Control rootControl, Button startButton, Button cancelButton, Label label, ProgressBar progressBar)
    {
      this.rootControl = rootControl;
      this.startButton = startButton;
      this.cancelButton = cancelButton;
      this.label = label;
      this.progressBar = progressBar;
      _minimum = 1;
      _maximum = 100;
      InitRange();
      _position = 1;
      _lastPercentage = 0;
    }

    public delegate void SetTextInvoker(string text);
    public delegate void SetPositionInvoker(int val);
    public delegate void RangeInvoker(int minimum, int maximum);
    public delegate Object GetCancelButtonTag();
    public delegate void BeginInvoker();

    #region Implementation of IProgressCallback

    public override void Begin()
    {
      rootControl.Invoke(new MethodInvoker(DoBegin));
    }

    public override void SetRange(int progressBarIndex, long minimum, long maximum)
    {
      if (progressBarIndex == 0)
      {
        _minimum = minimum;
        _maximum = maximum;
        _position = _minimum;
        _lastPercentage = 0;
        InitRange();
        rootControl.Invoke(new RangeInvoker(DoSetRange), new object[] { 1, 100 });
      }
    }

    private void InitRange()
    {
      _range = (_maximum - _minimum + 1) / 100.0;
    }

    public override void SetMessage(int progressBarIndex, String text)
    {
      if (progressBarIndex == 0)
      {
        rootControl.Invoke(new SetTextInvoker(DoSetMessage), new object[] { text });
      }
    }

    public override void Increment(int progressBarIndex, long val)
    {
      if (progressBarIndex == 0)
      {
        SetPosition(0, _position + val);
      }
    }

    public override void SetPosition(int progressBarIndex, long val)
    {
      if (progressBarIndex == 0)
      {
        _position = val;
        
        int curPercentage = (int)((_position - _minimum) / _range);
        if (curPercentage > 100)
        {
          curPercentage = 100;
        }

        if (curPercentage != _lastPercentage)
        {
          _lastPercentage = curPercentage;
          rootControl.Invoke(new SetPositionInvoker(DoSetPosition), new object[] { curPercentage });
        }
      }
    }

    public override bool IsCancellationPending()
    {
      return (bool)rootControl.Invoke(new GetCancelButtonTag(DoGetCancelButtonTag));
    }

    public override void End()
    {
      rootControl.Invoke(new MethodInvoker(DoEnd));
    }
    #endregion

    #region Implementation members invoked on the owner thread

    private void DoSetMessage(String text)
    {
      if (null != label)
      {
        label.Text = text;
        label.Update();
      }
    }

    private void DoSetPosition(int val)
    {
      if (null != progressBar && progressBar.Value != val)
      {
        progressBar.Value = val;
        progressBar.Update();
      }
    }

    private void DoBegin()
    {
      if (null != startButton)
      {
        startButton.Enabled = false;
      }

      if (null != cancelButton)
      {
        cancelButton.Enabled = true;
        cancelButton.Tag = false;
      }
    }

    private void DoSetRange(int minimum, int maximum)
    {
      if (null != progressBar)
      {
        progressBar.Minimum = minimum;
        progressBar.Maximum = maximum;
        progressBar.Value = minimum;
        progressBar.Update();
      }
    }

    private void DoEnd()
    {
      if (null != startButton)
      {
        startButton.Enabled = true;
      }

      if (null != cancelButton)
      {
        cancelButton.Tag = false;
        cancelButton.Enabled = false;
      }
    }

    private Object DoGetCancelButtonTag()
    {
      if (null != cancelButton)
      {
        return cancelButton.Tag;
      }
      else
      {
        return false;
      }
    }

    #endregion
  }
}
