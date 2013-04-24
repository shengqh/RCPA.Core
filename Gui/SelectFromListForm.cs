using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class SelectFromListForm : Form
  {
    public SelectFromListForm()
    {
      InitializeComponent();
    }

    public void Initialize(List<string> allItems, List<string> checkedItems)
    {
      lbItems.BeginUpdate();
      try
      {
        lbItems.Items.Clear();
        foreach (var item in allItems)
        {
          lbItems.Items.Add(item, checkedItems.Contains(item));
        }
      }
      finally
      {
        lbItems.EndUpdate();
      }
    }

    public List<string> GetCheckedItems()
    {
      List<string> result = new List<string> ();

      foreach (var item in lbItems.CheckedItems)
      {
        result.Add(MyConvert.Format(item));
      }

      return result;
    }
  }
}
