namespace RCPA.Gui
{
  partial class AbstractFileProcessorUI
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
      this.pnlFile = new System.Windows.Forms.Panel();
      this.txtOriginalFile = new System.Windows.Forms.TextBox();
      this.btnOriginalFile = new System.Windows.Forms.Button();
      this.pnlFile.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblProgress
      // 
      this.lblProgress.Location = new System.Drawing.Point(0, 36);
      this.lblProgress.Size = new System.Drawing.Size(955, 23);
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point(0, 59);
      this.progressBar.Size = new System.Drawing.Size(955, 23);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(525, 8);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(440, 8);
      // 
      // btnGo
      // 
      this.btnGo.Location = new System.Drawing.Point(355, 8);
      // 
      // pnlFile
      // 
      this.pnlFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlFile.Controls.Add(this.txtOriginalFile);
      this.pnlFile.Controls.Add(this.btnOriginalFile);
      this.pnlFile.Location = new System.Drawing.Point(0, 13);
      this.pnlFile.Name = "pnlFile";
      this.pnlFile.Size = new System.Drawing.Size(955, 24);
      this.pnlFile.TabIndex = 9;
      // 
      // txtOriginalFile
      // 
      this.txtOriginalFile.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtOriginalFile.Location = new System.Drawing.Point(246, 0);
      this.txtOriginalFile.Name = "txtOriginalFile";
      this.txtOriginalFile.Size = new System.Drawing.Size(709, 20);
      this.txtOriginalFile.TabIndex = 5;
      // 
      // btnOriginalFile
      // 
      this.btnOriginalFile.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnOriginalFile.Location = new System.Drawing.Point(0, 0);
      this.btnOriginalFile.Name = "btnOriginalFile";
      this.btnOriginalFile.Size = new System.Drawing.Size(246, 24);
      this.btnOriginalFile.TabIndex = 3;
      this.btnOriginalFile.Text = "btnOriginalFile";
      this.btnOriginalFile.UseVisualStyleBackColor = true;
      // 
      // AbstractFileProcessorUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.ClientSize = new System.Drawing.Size(955, 121);
      this.Controls.Add(this.pnlFile);
      this.Name = "AbstractFileProcessorUI";
      this.Controls.SetChildIndex(this.progressBar, 0);
      this.Controls.SetChildIndex(this.lblProgress, 0);
      this.Controls.SetChildIndex(this.pnlFile, 0);
      this.pnlFile.ResumeLayout(false);
      this.pnlFile.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.Panel pnlFile;
    protected System.Windows.Forms.TextBox txtOriginalFile;
    protected System.Windows.Forms.Button btnOriginalFile;

  }
}
