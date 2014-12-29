using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class DoubleField : TextField
  {
    public DoubleField()
    {
      InitializeComponent();

      this.ValidateFunc = (m =>
      {
        double value;
        return double.TryParse(m, out value);
      });
    }

    public double Value
    {
      get
      {
        return double.Parse(Text);
      }
      set
      {
        Text = value.ToString();
      }
    }
  }
}
