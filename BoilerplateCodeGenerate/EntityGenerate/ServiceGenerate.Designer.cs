namespace EntityGenerate
{
    partial class fServiceGenerate
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtServiceBaseName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServiceNames = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "ServiceBase Name";
            // 
            // txtServiceBaseName
            // 
            this.txtServiceBaseName.Location = new System.Drawing.Point(158, 26);
            this.txtServiceBaseName.Name = "txtServiceBaseName";
            this.txtServiceBaseName.Size = new System.Drawing.Size(399, 24);
            this.txtServiceBaseName.TabIndex = 18;
            this.txtServiceBaseName.Text = "MyProject";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 18);
            this.label2.TabIndex = 21;
            this.label2.Text = "Service Names";
            // 
            // txtServiceNames
            // 
            this.txtServiceNames.Location = new System.Drawing.Point(158, 68);
            this.txtServiceNames.Name = "txtServiceNames";
            this.txtServiceNames.Size = new System.Drawing.Size(399, 24);
            this.txtServiceNames.TabIndex = 20;
            this.txtServiceNames.Text = "MyProjectAppServiceBase";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "Path";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(158, 107);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(399, 24);
            this.txtPath.TabIndex = 22;
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(158, 149);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(120, 37);
            this.btnCheckAll.TabIndex = 24;
            this.btnCheckAll.Text = "Generate";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // fServiceGenerate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 208);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServiceNames);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtServiceBaseName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fServiceGenerate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ServiceGenerate";
            this.Load += new System.EventHandler(this.fServiceGenerate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServiceBaseName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServiceNames;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnCheckAll;
    }
}