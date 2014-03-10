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
  public partial class RcpaSelectList : UserControl
  {
    public RcpaSelectList()
    {
      InitializeComponent();
    }

    [Localizable(true)]
    [Category("Buttons"), DescriptionAttribute("Gets or sets the Load Button visible"), DefaultValue(true)]
    public bool LoadButtonVisible
    {
      get { return btnLoad.Visible; }
      set { btnLoad.Visible = value; }
    }

    [Localizable(true)]
    [Category("Buttons"), DescriptionAttribute("Gets or sets the Save Button visible"), DefaultValue(true)]
    public bool SaveButtonVisible
    {
      get { return btnSave.Visible; }
      set { btnSave.Visible = value; }
    }

    [Localizable(true)]
    [Category("Buttons"), DescriptionAttribute("Gets or sets the Moveup Button visible"), DefaultValue(true)]
    public bool UpButtonVisible
    {
      get { return btnMoveUp.Visible; }
      set { btnMoveUp.Visible = value; }
    }

    [Localizable(true)]
    [Category("Buttons"), DescriptionAttribute("Gets or sets the Movedown Button visible"), DefaultValue(true)]
    public bool DownButtonVisible
    {
      get { return btnMoveDown.Visible; }
      set { btnMoveDown.Visible = value; }
    }

    [Localizable(true)]
    [Category("Description"), DescriptionAttribute("Gets or sets the Description value"), DefaultValue("Description")]
    public string Description
    {
      get { return lblTitle.Text; }
      set { lblTitle.Text = value; }
    }

    public void Initialize(List<string> allItems, List<string> checkedItems, List<string> oldItems)
    {

      List<string> newItems;
      if (oldItems == null || oldItems.Count == 0)
      {
        newItems = (from item in checkedItems
                    where allItems.Contains(item)
                    select item).Union(from item in allItems
                                       where !checkedItems.Contains(item)
                                       select item).ToList();
      }
      else
      {
        newItems = (from item in oldItems
                    where allItems.Contains(item)
                    select item).Union(from item in allItems
                                       where !oldItems.Contains(item)
                                       select item).ToList();
      }

      lbItems.BeginUpdate();
      try
      {
        lbItems.Items.Clear();
        foreach (var item in newItems)
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
      List<string> result = new List<string>();

      foreach (var item in lbItems.CheckedItems)
      {
        result.Add(MyConvert.Format(item));
      }

      return result;
    }

    public List<Tuple<string, bool>> GetItems()
    {
      var result = new List<Tuple<string, bool>>();

      for (int i = 0; i < lbItems.Items.Count; i++)
      {
        result.Add(new Tuple<string, bool>(MyConvert.Format(lbItems.Items[i]), lbItems.CheckedIndices.Contains(i)));
      }

      return result;
    }

    private void listUp(CheckedListBox listBox)
    {
      if (listBox.SelectedIndex == -1 || listBox.SelectedIndex == 0)
        return;

      object selected;
      object previous;
      object temp;

      bool selectedChecked;
      bool previousChecked;
      bool tempChecked;

      selected = listBox.Items[listBox.SelectedIndex];
      selectedChecked = listBox.GetItemChecked(listBox.SelectedIndex);
      previous = listBox.Items[listBox.SelectedIndex - 1];
      previousChecked = listBox.GetItemChecked(listBox.SelectedIndex - 1);

      temp = selected;
      tempChecked = selectedChecked;
      selected = previous;
      selectedChecked = previousChecked;
      previous = temp;
      previousChecked = tempChecked;

      listBox.Items[listBox.SelectedIndex] = selected;
      listBox.SetItemChecked(listBox.SelectedIndex, selectedChecked);
      listBox.Items[listBox.SelectedIndex - 1] = previous;
      listBox.SetItemChecked(listBox.SelectedIndex - 1, previousChecked);

      listBox.SelectedIndex--;
    }

    private void listDown(CheckedListBox listBox)
    {
      if (listBox.SelectedIndex == -1 || listBox.SelectedIndex == listBox.Items.Count - 1)
        return;

      object selected;
      object next;
      object temp;

      bool selectedChecked;
      bool nextChecked;
      bool tempChecked;

      selected = listBox.Items[listBox.SelectedIndex];
      selectedChecked = listBox.GetItemChecked(listBox.SelectedIndex);
      next = listBox.Items[listBox.SelectedIndex + 1];
      nextChecked = listBox.GetItemChecked(listBox.SelectedIndex + 1);

      temp = selected;
      tempChecked = selectedChecked;
      selected = next;
      selectedChecked = nextChecked;
      next = temp;
      nextChecked = tempChecked;

      listBox.Items[listBox.SelectedIndex] = selected;
      listBox.SetItemChecked(listBox.SelectedIndex, selectedChecked);
      listBox.Items[listBox.SelectedIndex + 1] = next;
      listBox.SetItemChecked(listBox.SelectedIndex + 1, nextChecked);

      listBox.SelectedIndex++;
    }

    private void btnMoveUp_Click(object sender, EventArgs e)
    {
      listUp(lbItems);
    }

    private void btnMoveDown_Click(object sender, EventArgs e)
    {
      listDown(lbItems);
    }
  }
}
