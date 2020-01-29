namespace DecryptIdeas
{
    partial class Form1
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
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtIdeasResponse = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblIdeasResponse = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(190, 109);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 0;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtIdeasResponse
            // 
            this.txtIdeasResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIdeasResponse.Location = new System.Drawing.Point(12, 22);
            this.txtIdeasResponse.Multiline = true;
            this.txtIdeasResponse.Name = "txtIdeasResponse";
            this.txtIdeasResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIdeasResponse.Size = new System.Drawing.Size(431, 81);
            this.txtIdeasResponse.TabIndex = 1;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(12, 157);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(431, 136);
            this.txtResult.TabIndex = 2;
            // 
            // lblIdeasResponse
            // 
            this.lblIdeasResponse.AutoSize = true;
            this.lblIdeasResponse.Location = new System.Drawing.Point(12, 6);
            this.lblIdeasResponse.Name = "lblIdeasResponse";
            this.lblIdeasResponse.Size = new System.Drawing.Size(166, 13);
            this.lblIdeasResponse.TabIndex = 3;
            this.lblIdeasResponse.Text = "Ideas Response (Encrypted XML)";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 141);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(37, 13);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Result";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 305);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblIdeasResponse);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtIdeasResponse);
            this.Controls.Add(this.btnDecrypt);
            this.Name = "Form1";
            this.Text = "Ideas Response Decryptor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtIdeasResponse;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblIdeasResponse;
        private System.Windows.Forms.Label lblResult;
    }
}

