namespace EntityGenerate
{
    partial class Main
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
            this.btnClient = new System.Windows.Forms.Button();
            this.btnService = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnEntity = new System.Windows.Forms.Button();
            this.clbTable = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbDb = new System.Windows.Forms.ComboBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServiceBaseName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnClient
            // 
            this.btnClient.Location = new System.Drawing.Point(600, 140);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(98, 40);
            this.btnClient.TabIndex = 20;
            this.btnClient.Text = "Client";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // btnService
            // 
            this.btnService.Location = new System.Drawing.Point(476, 140);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(93, 40);
            this.btnService.TabIndex = 19;
            this.btnService.Text = "Service";
            this.btnService.UseVisualStyleBackColor = true;
            this.btnService.Click += new System.EventHandler(this.btnService_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(206, 140);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(105, 40);
            this.btnCheckAll.TabIndex = 18;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "Path";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(206, 58);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(629, 24);
            this.txtPath.TabIndex = 16;
            this.txtPath.Text = "C:\\Users\\CoCaCoLaLa\\Desktop\\Test";
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged_1);
            // 
            // btnEntity
            // 
            this.btnEntity.Location = new System.Drawing.Point(342, 140);
            this.btnEntity.Name = "btnEntity";
            this.btnEntity.Size = new System.Drawing.Size(103, 40);
            this.btnEntity.TabIndex = 15;
            this.btnEntity.Text = "Entity";
            this.btnEntity.UseVisualStyleBackColor = true;
            this.btnEntity.Click += new System.EventHandler(this.btnEntity_Click);
            // 
            // clbTable
            // 
            this.clbTable.CheckOnClick = true;
            this.clbTable.FormattingEnabled = true;
            this.clbTable.Location = new System.Drawing.Point(206, 190);
            this.clbTable.Name = "clbTable";
            this.clbTable.Size = new System.Drawing.Size(629, 308);
            this.clbTable.TabIndex = 14;
            this.clbTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clbTable_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "Database";
            // 
            // cbbDb
            // 
            this.cbbDb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDb.FormattingEnabled = true;
            this.cbbDb.Location = new System.Drawing.Point(206, 9);
            this.cbbDb.Name = "cbbDb";
            this.cbbDb.Size = new System.Drawing.Size(629, 26);
            this.cbbDb.TabIndex = 12;
            this.cbbDb.SelectedIndexChanged += new System.EventHandler(this.cbbDb_SelectedIndexChanged);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(729, 140);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(106, 40);
            this.btnAll.TabIndex = 21;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "ServiceBase Name";
            // 
            // txtServiceBaseName
            // 
            this.txtServiceBaseName.Location = new System.Drawing.Point(206, 99);
            this.txtServiceBaseName.Name = "txtServiceBaseName";
            this.txtServiceBaseName.Size = new System.Drawing.Size(629, 24);
            this.txtServiceBaseName.TabIndex = 22;
            this.txtServiceBaseName.Text = "MyProject";
            this.txtServiceBaseName.TextChanged += new System.EventHandler(this.txtServiceBaseName_TextChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 508);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtServiceBaseName);
            this.Controls.Add(this.btnAll);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Button btnService;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnEntity;
        private System.Windows.Forms.CheckedListBox clbTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbDb;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServiceBaseName;
    }
}

