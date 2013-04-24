namespace RCPA.Gui
{
  partial class AbstractTurboProcessorUI
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
      this.tcBatchMode = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.txtSingle = new System.Windows.Forms.TextBox();
      this.btnSingle = new System.Windows.Forms.Button();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.txtBatch = new System.Windows.Forms.TextBox();
      this.btnBatch = new System.Windows.Forms.Button();
      this.pnlFile.SuspendLayout();
      this.tcBatchMode.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblSecondProgress
      // 
      this.lblSecondProgress.Location = new System.Drawing.Point(0, 178);
      this.lblSecondProgress.Size = new System.Drawing.Size(897, 23);
      // 
      // pnlFile
      // 
      this.pnlFile.Location = new System.Drawing.Point(31, 133);
      this.pnlFile.Size = new System.Drawing.Size(831, 24);
      // 
      // txtOriginalFile
      // 
      this.txtOriginalFile.Size = new System.Drawing.Size(585, 20);
      // 
      // lblProgress
      // 
      this.lblProgress.Location = new System.Drawing.Point(0, 201);
      this.lblProgress.Size = new System.Drawing.Size(897, 23);
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point(0, 224);
      this.progressBar.Size = new System.Drawing.Size(897, 23);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(496, 9);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(411, 9);
      // 
      // btnGo
      // 
      this.btnGo.Location = new System.Drawing.Point(326, 9);
      // 
      // tcBatchMode
      // 
      this.tcBatchMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tcBatchMode.Controls.Add(this.tabPage1);
      this.tcBatchMode.Controls.Add(this.tabPage2);
      this.tcBatchMode.Location = new System.Drawing.Point(31, 27);
      this.tcBatchMode.Name = "tcBatchMode";
      this.tcBatchMode.SelectedIndex = 0;
      this.tcBatchMode.Size = new System.Drawing.Size(831, 100);
      this.tcBatchMode.TabIndex = 27;
      this.tcBatchMode.SelectedIndexChanged += new System.EventHandler(this.tcBatchMode_SelectedIndexChanged);
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.txtSingle);
      this.tabPage1.Controls.Add(this.btnSingle);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(823, 74);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Single Mode";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // txtSingle
      // 
      this.txtSingle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSingle.Location = new System.Drawing.Point(208, 33);
      this.txtSingle.Name = "txtSingle";
      this.txtSingle.Size = new System.Drawing.Size(595, 20);
      this.txtSingle.TabIndex = 1;
      // 
      // btnSingle
      // 
      this.btnSingle.Location = new System.Drawing.Point(17, 31);
      this.btnSingle.Name = "btnSingle";
      this.btnSingle.Size = new System.Drawing.Size(172, 23);
      this.btnSingle.TabIndex = 0;
      this.btnSingle.Text = "button1";
      this.btnSingle.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.txtBatch);
      this.tabPage2.Controls.Add(this.btnBatch);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(823, 74);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Batch Mode";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // txtBatch
      // 
      this.txtBatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBatch.Location = new System.Drawing.Point(208, 33);
      this.txtBatch.Name = "txtBatch";
      this.txtBatch.Size = new System.Drawing.Size(595, 20);
      this.txtBatch.TabIndex = 3;
      // 
      // btnBatch
      // 
      this.btnBatch.Location = new System.Drawing.Point(17, 31);
      this.btnBatch.Name = "btnBatch";
      this.btnBatch.Size = new System.Drawing.Size(172, 23);
      this.btnBatch.TabIndex = 2;
      this.btnBatch.Text = "button2";
      this.btnBatch.UseVisualStyleBackColor = true;
      // 
      // AbstractTurboProcessorUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(897, 286);
      this.Controls.Add(this.tcBatchMode);
      this.Name = "AbstractTurboProcessorUI";
      this.Controls.SetChildIndex(this.pnlFile, 0);
      this.Controls.SetChildIndex(this.progressBar, 0);
      this.Controls.SetChildIndex(this.lblProgress, 0);
      this.Controls.SetChildIndex(this.tcBatchMode, 0);
      this.Controls.SetChildIndex(this.lblSecondProgress, 0);
      this.pnlFile.ResumeLayout(false);
      this.pnlFile.PerformLayout();
      this.tcBatchMode.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.TabControl tcBatchMode;
    protected System.Windows.Forms.TabPage tabPage1;
    protected System.Windows.Forms.TabPage tabPage2;
    protected System.Windows.Forms.TextBox txtSingle;
    protected System.Windows.Forms.Button btnSingle;
    protected System.Windows.Forms.TextBox txtBatch;
    protected System.Windows.Forms.Button btnBatch;
  }
}