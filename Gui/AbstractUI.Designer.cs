namespace RCPA.Gui
{
  partial class AbstractUI
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
      this.pnlButton = new System.Windows.Forms.Panel();
      this.btnGo = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.pnlButton.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.pnlButton.Controls.Add(this.btnClose);
      this.pnlButton.Controls.Add(this.btnCancel);
      this.pnlButton.Controls.Add(this.btnGo);
      this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlButton.Location = new System.Drawing.Point(0, 283);
      this.pnlButton.Name = "panel1";
      this.pnlButton.Size = new System.Drawing.Size(893, 36);
      this.pnlButton.TabIndex = 7;
      // 
      // btnGo
      // 
      this.btnGo.Location = new System.Drawing.Point(328, 8);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = new System.Drawing.Size(75, 21);
      this.btnGo.TabIndex = 1;
      this.btnGo.Text = "&Go";
      this.btnGo.UseVisualStyleBackColor = true;
      this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Enabled = false;
      this.btnCancel.Location = new System.Drawing.Point(409, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 21);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "C&ancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(490, 8);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 21);
      this.btnClose.TabIndex = 8;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // AbstractUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(893, 319);
      this.Controls.Add(this.pnlButton);
      this.Name = "AbstractUI";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Load += new System.EventHandler(this.AbstractUI_Load);
      this.SizeChanged += new System.EventHandler(this.AbstractUI_SizeChanged);
      this.pnlButton.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel pnlButton;
    protected System.Windows.Forms.Button btnClose;
    protected System.Windows.Forms.Button btnCancel;
    protected System.Windows.Forms.Button btnGo;

  }
}