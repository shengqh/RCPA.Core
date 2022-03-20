using System;
using System.Linq;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class RcpaListSelection : UserControl
  {
    public RcpaListSelection()
    {
      InitializeComponent();
    }

    public void AssignItems(object[] itemsLeft, object[] itemsRight)
    {
      lbLeft.Items.Clear();
      lbLeft.Items.AddRange(itemsLeft);
      lbRight.Items.Clear();
      lbRight.Items.AddRange(itemsRight);
    }

    private void RcpaListSelection_SizeChanged(object sender, EventArgs e)
    {
      lbLeft.Width = (this.Width - 100) / 2;
      lbRight.Width = lbLeft.Width;
    }

    public object[] GetSelectedItems()
    {
      return lbRight.Items.Cast<object>().ToArray(); ;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      foreach (var item in lbLeft.SelectedItems)
      {
        if (!lbRight.Items.Contains(item))
        {
          lbRight.Items.Add(item);
        }
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      var deleteItems = lbRight.SelectedItems.Cast<object>().ToList();
      foreach (var item in deleteItems)
      {
        lbRight.Items.Remove(item);
      }
    }
  }
}
