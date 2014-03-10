namespace RCPA.Gui
{
  partial class RcpaSelectList
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblTitle = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lbItems = new System.Windows.Forms.CheckedListBox();
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnMoveUp = new System.Windows.Forms.Button();
      this.btnMoveDown = new System.Windows.Forms.Button();
      this.btnLoad = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblTitle
      // 
      this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
      this.lblTitle.Location = new System.Drawing.Point(0, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(397, 23);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Description";
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.lbItems);
      this.panel1.Controls.Add(this.panel2);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 23);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(397, 592);
      this.panel1.TabIndex = 1;
      // 
      // lbItems
      // 
      this.lbItems.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbItems.FormattingEnabled = true;
      this.lbItems.Location = new System.Drawing.Point(0, 0);
      this.lbItems.Name = "lbItems";
      this.lbItems.Size = new System.Drawing.Size(310, 592);
      this.lbItems.TabIndex = 2;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnSave);
      this.panel2.Controls.Add(this.btnLoad);
      this.panel2.Controls.Add(this.btnMoveDown);
      this.panel2.Controls.Add(this.btnMoveUp);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel2.Location = new System.Drawing.Point(310, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(87, 592);
      this.panel2.TabIndex = 3;
      // 
      // btnMoveUp
      // 
      this.btnMoveUp.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnMoveUp.Location = new System.Drawing.Point(0, 0);
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new System.Drawing.Size(87, 23);
      this.btnMoveUp.TabIndex = 0;
      this.btnMoveUp.Text = "Move up";
      this.btnMoveUp.UseVisualStyleBackColor = true;
      this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
      // 
      // btnMoveDown
      // 
      this.btnMoveDown.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnMoveDown.Location = new System.Drawing.Point(0, 23);
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new System.Drawing.Size(87, 23);
      this.btnMoveDown.TabIndex = 1;
      this.btnMoveDown.Text = "Move down";
      this.btnMoveDown.UseVisualStyleBackColor = true;
      this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
      // 
      // btnLoad
      // 
      this.btnLoad.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnLoad.Location = new System.Drawing.Point(0, 46);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(87, 23);
      this.btnLoad.TabIndex = 2;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      // 
      // btnSave
      // 
      this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnSave.Location = new System.Drawing.Point(0, 69);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(87, 23);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      // 
      // SelectFromListForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(397, 615);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.lblTitle);
      this.Name = "SelectFromListForm";
      this.Text = "Select Items";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckedListBox lbItems;
    private System.Windows.Forms.Panel panel2;
    public System.Windows.Forms.Button btnSave;
    public System.Windows.Forms.Button btnLoad;
    public System.Windows.Forms.Button btnMoveDown;
    public System.Windows.Forms.Button btnMoveUp;

  }
}