namespace GenerateEntity
{
    partial class fGenerateEntity
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
            this.cbbDb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clbTable = new System.Windows.Forms.CheckedListBox();
            this.btnEntity = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnService = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbbDb
            // 
            this.cbbDb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDb.FormattingEnabled = true;
            this.cbbDb.Location = new System.Drawing.Point(150, 16);
            this.cbbDb.Name = "cbbDb";
            this.cbbDb.Size = new System.Drawing.Size(631, 41);
            this.cbbDb.TabIndex = 3;
            this.cbbDb.SelectedIndexChanged += new System.EventHandler(this.cbbDb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 36);
            this.label2.TabIndex = 4;
            this.label2.Text = "Database";
            // 
            // clbTable
            // 
            this.clbTable.CheckOnClick = true;
            this.clbTable.FormattingEnabled = true;
            this.clbTable.Location = new System.Drawing.Point(150, 162);
            this.clbTable.Name = "clbTable";
            this.clbTable.Size = new System.Drawing.Size(631, 308);
            this.clbTable.TabIndex = 5;
            this.clbTable.SelectedIndexChanged += new System.EventHandler(this.clbTable_SelectedIndexChanged);
            // 
            // btnEntity
            // 
            this.btnEntity.Location = new System.Drawing.Point(324, 112);
            this.btnEntity.Name = "btnEntity";
            this.btnEntity.Size = new System.Drawing.Size(161, 40);
            this.btnEntity.TabIndex = 6;
            this.btnEntity.Text = "Entity";
            this.btnEntity.UseVisualStyleBackColor = true;
            this.btnEntity.Click += new System.EventHandler(this.btnGenerateCode_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(150, 65);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(631, 41);
            this.txtPath.TabIndex = 7;
            this.txtPath.Text = "C:\\Users\\HungSe\'o\\Desktop\\Test";
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 36);
            this.label1.TabIndex = 8;
            this.label1.Text = "Path";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(150, 112);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(162, 40);
            this.btnCheckAll.TabIndex = 9;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnService
            // 
            this.btnService.Location = new System.Drawing.Point(497, 112);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(142, 40);
            this.btnService.TabIndex = 10;
            this.btnService.Text = "Service";
            this.btnService.UseVisualStyleBackColor = true;
            // 
            // btnClient
            // 
            this.btnClient.Location = new System.Drawing.Point(651, 112);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(130, 40);
            this.btnClient.TabIndex = 11;
            this.btnClient.Text = "Client";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // fGenerateEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 490);
            this.Controls.Add(this.btnClient);
            this.Controls.Add(this.btnService);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnEntity);
            this.Controls.Add(this.clbTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbDb);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "fGenerateEntity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenerateEntity";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbbDb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clbTable;
        private System.Windows.Forms.Button btnEntity;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnService;
        private System.Windows.Forms.Button btnClient;
    }
}

