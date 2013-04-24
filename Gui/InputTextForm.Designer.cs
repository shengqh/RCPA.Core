namespace RCPA.Gui
{
  partial class InputTextForm
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
      this.btnOk = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblDescription = new System.Windows.Forms.Label();
      this.txtValue = new System.Windows.Forms.TextBox();
      this.btnSkip = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btnOk
      // 
      this.btnOk.Location = new System.Drawing.Point(222, 92);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "&Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(384, 92);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // lblDescription
      // 
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new System.Drawing.Point(47, 29);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(41, 12);
      this.lblDescription.TabIndex = 2;
      this.lblDescription.Text = "label1";
      // 
      // txtValue
      // 
      this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtValue.HideSelection = false;
      this.txtValue.Location = new System.Drawing.Point(49, 56);
      this.txtValue.Name = "txtValue";
      this.txtValue.Size = new System.Drawing.Size(588, 21);
      this.txtValue.TabIndex = 0;
      this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValue_KeyPress);
      // 
      // btnSkip
      // 
      this.btnSkip.Location = new System.Drawing.Point(303, 92);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.Size = new System.Drawing.Size(75, 23);
      this.btnSkip.TabIndex = 4;
      this.btnSkip.Text = "&Skip";
      this.btnSkip.UseVisualStyleBackColor = true;
      this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
      // 
      // InputTextForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(686, 129);
      this.Controls.Add(this.btnSkip);
      this.Controls.Add(this.txtValue);
      this.Controls.Add(this.lblDescription);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOk);
      this.Name = "InputTextForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "InputTextForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtValue;
    private System.Windows.Forms.Button btnSkip;
  }
}