namespace RCPA.Gui
{
  partial class TextField
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
      this.TextEdit = new System.Windows.Forms.TextBox();
      this.lblCaption = new System.Windows.Forms.Label();
      this.lblDescription = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // TextEdit
      // 
      this.TextEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TextEdit.Location = new System.Drawing.Point(257, 0);
      this.TextEdit.Margin = new System.Windows.Forms.Padding(0);
      this.TextEdit.Name = "TextEdit";
      this.TextEdit.Size = new System.Drawing.Size(981, 22);
      this.TextEdit.TabIndex = 1;
      // 
      // lblCaption
      // 
      this.lblCaption.Dock = System.Windows.Forms.DockStyle.Left;
      this.lblCaption.Location = new System.Drawing.Point(0, 0);
      this.lblCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new System.Drawing.Size(257, 28);
      this.lblCaption.TabIndex = 2;
      this.lblCaption.Text = "Description";
      this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblDescription
      // 
      this.lblDescription.Dock = System.Windows.Forms.DockStyle.Right;
      this.lblDescription.Location = new System.Drawing.Point(1238, 0);
      this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(25, 28);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // TextField
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.TextEdit);
      this.Controls.Add(this.lblDescription);
      this.Controls.Add(this.lblCaption);
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "TextField";
      this.Size = new System.Drawing.Size(1263, 28);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.TextBox TextEdit;
    public System.Windows.Forms.Label lblCaption;
    public System.Windows.Forms.Label lblDescription;

  }
}
