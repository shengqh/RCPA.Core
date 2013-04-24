namespace RCPA.Gui
{
  partial class MultipleProgressField
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblProgress1 = new System.Windows.Forms.Label();
      this.lblProgress2 = new System.Windows.Forms.Label();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // lblProgress1
      // 
      this.lblProgress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lblProgress1.Location = new System.Drawing.Point(3, 14);
      this.lblProgress1.Name = "lblProgress1";
      this.lblProgress1.Size = new System.Drawing.Size(791, 23);
      this.lblProgress1.TabIndex = 0;
      // 
      // lblProgress2
      // 
      this.lblProgress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lblProgress2.Location = new System.Drawing.Point(3, 36);
      this.lblProgress2.Name = "lblProgress2";
      this.lblProgress2.Size = new System.Drawing.Size(791, 23);
      this.lblProgress2.TabIndex = 1;
      // 
      // progressBar
      // 
      this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar.Location = new System.Drawing.Point(3, 62);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(791, 23);
      this.progressBar.TabIndex = 2;
      // 
      // MultipleProgressField
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add(this.progressBar);
      this.Controls.Add(this.lblProgress2);
      this.Controls.Add(this.lblProgress1);
      this.Name = "MultipleProgressField";
      this.Size = new System.Drawing.Size(799, 91);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblProgress1;
    private System.Windows.Forms.Label lblProgress2;
    private System.Windows.Forms.ProgressBar progressBar;
  }
}
