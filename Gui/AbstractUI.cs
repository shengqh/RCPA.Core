using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class AbstractUI : ComponentUI
  {
    private readonly List<Button> buttonList;

    public AbstractUI()
    {
      InitializeComponent();

      this.buttonList = new List<Button>();

      AddButton(this.btnGo);
      AddButton(this.btnCancel);
      AddButton(this.btnClose);
    }

    protected void AddButton(Button btn)
    {
      AddButtonToPanel(btn);

      if (!this.buttonList.Contains(btn))
      {
        this.buttonList.Add(btn);
      }
    }

    private void AddButtonToPanel(Button btn)
    {
      if (!pnlButton.Contains(btn))
      {
        pnlButton.Controls.Add(btn);
      }
    }

    protected void InsertButton(int index, Button btn)
    {
      AddButtonToPanel(btn);

      if (!this.buttonList.Contains(btn))
      {
        this.buttonList.Insert(index, btn);
      }
    }

    protected void RemoveButton(Button btn)
    {
      if (this.buttonList.Contains(btn))
      {
        this.buttonList.Remove(btn);
      }
    }

    public void UpdateButtons()
    {
      if (this.buttonList != null)
      {
        var validButtons = new List<Button>();
        int totalWidth = -10;
        foreach (Button btn in this.buttonList)
        {
          if (btn.Visible)
          {
            validButtons.Add(btn);
            totalWidth += 10 + btn.Width;
          }
        }

        int left = ClientSize.Width / 2 - totalWidth / 2;
        for (int i = 0; i < validButtons.Count; i++)
        {
          validButtons[i].Left = left;
          left += validButtons[i].Width + 10;
          validButtons[i].Top = (pnlButton.Height - validButtons[i].Height) / 2;
        }
      }
    }

    private void AbstractUI_Load(object sender, EventArgs e)
    {
      UpdateButtons();
    }

    private void btnGo_Click(object sender, EventArgs e)
    {
      try
      {
        ValidateComponents();
      }
      catch (Exception ex)
      {
        if (!string.IsNullOrEmpty(ex.Message))
        {
          MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return;
      }

      SaveOption();

      try
      {
        DoRealGo();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, "Exception : " + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
      }
    }

    protected virtual void DoRealGo()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      SaveOption();
      Close();
    }

    private void AbstractUI_SizeChanged(object sender, EventArgs e)
    {
      UpdateButtons();
    }
  }
}