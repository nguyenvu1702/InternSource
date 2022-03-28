﻿using GetDatabaseInfor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenerateEntity
{
    public partial class fGenerateEntity : Form
    {
        public fGenerateEntity()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                btnEntity.Enabled = btnService.Enabled = btnClient.Enabled = false;
                cbbDb.DataSource = Database.GetDatabaseList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Chọn db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tableList = Database.ListTables(cbbDb.Text.Trim()).ToList();
            clbTable.Items.Clear();
            foreach (var item in tableList)
            {
                clbTable.Items.Add(item);
            }
        }

        /// <summary>
        /// Chọn bảng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEntity.Enabled = btnService.Enabled = btnClient.Enabled = GetEnableGenerateCode();
        }

        /// <summary>
        /// Bấm tạo entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateCode_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in clbTable.CheckedItems)
                {
                    GenerateEntity(item.ToString().Trim());
                    sb.AppendLine("public DbSet<" + item.ToString() + "> " + item.ToString() + " { get; set; }");
                }
                string pathDbset = txtPath.Text.Trim() + @"\DbSet.cs";
                File.WriteAllText(pathDbset, sb.ToString());
                MessageBox.Show("Thành công");
            }
            catch (Exception exx)
            {
                MessageBox.Show("Thất bại : " + exx.Message);
            }
        }

        /// <summary>
        /// Tạo từng file Entity
        /// </summary>
        /// <param name="TableName"></param>
        private void GenerateEntity(string TableName)
        {
            StringBuilder sb = new StringBuilder();
            // Lấy hết các column trong table
            var columnList = Database.GetListColumnNames(cbbDb.Text.Trim(), TableName);
            var idColumn = columnList.FirstOrDefault(e => e.ItemArray[3].ToString() == "Id");
            sb.AppendLine("// This file is not generated, but this comment is necessary to exclude it from StyleCop analysis");
            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("using Abp.Domain.Entities;");
            sb.AppendLine("using Abp.Domain.Entities.Auditing;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine();
            sb.AppendLine("namespace DbEntities");
            sb.AppendLine("{");
            sb.AppendLine("    [Table(\"" + TableName + "\")]");
            sb.AppendLine("    public class " + TableName + " : FullAuditedEntity" + (idColumn != null && idColumn[7].ToString() == "bigint" ? "<long>" : "") + ", IMayHaveTenant");
            sb.AppendLine("    {");
            sb.AppendLine("        public virtual int? TenantId { get; set; }");

            if (idColumn != null)
            {
                columnList.Remove(idColumn);
            }
            foreach (var item in columnList)
            {
                sb.AppendLine(Database.GetProperty(item.ItemArray));
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            string path = txtPath.Text.Trim() + @"\" + TableName + ".cs";
            File.WriteAllText(path, sb.ToString());
        }

        /// <summary>
        /// Tạo file client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClient_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in clbTable.CheckedItems)
                {
                    GenerateClient(item.ToString().Trim());
                }
                MessageBox.Show("Thành công");
            }
            catch (Exception exx)
            {
                MessageBox.Show("Thất bại : " + exx.Message);
            }
        }

        /// <summary>
        /// Tạo từng file Client
        /// </summary>
        /// <param name="TableName"></param>
        private void GenerateClient(string TableName)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            // Lấy hết các column trong table
            var columnList = Database.GetListColumnNames(cbbDb.Text.Trim(), TableName);
            int index = 1;
            sb1.AppendLine("form: FormGroup;");
            sb1.AppendLine("constructor(");
            sb1.AppendLine("injector: Injector,");
            sb1.AppendLine(" private fb: FormBuilder");
            sb1.AppendLine(") {");
            sb1.AppendLine("super(injector);");
            sb1.AppendLine("}");
            sb1.AppendLine("");
            sb1.AppendLine("ngOnInit(): void {");
            sb1.AppendLine("this.khoiTaoForm();");
            sb1.AppendLine("}");
            sb1.AppendLine("");
            sb1.AppendLine("khoiTaoForm() {");
            sb1.AppendLine("this.form = this.fb.group({");

            foreach (var item in columnList)
            {
                string colName = item[3].ToString();
                colName = colName.Substring(0, 1).ToLower() + colName.Substring(1);

                sb1.AppendLine(colName + " : [''],");

                if (index % 2 != 0)
                {
                    sb.AppendLine("<div class=\"row ui-fluid\">");
                }
                sb.AppendLine("    <div class=\"col-md-6\">");
                sb.AppendLine("        <div class=\"form-group\">");
                sb.AppendLine("            <dft-label-validation [control]=\"form.get('" + colName + "')\"");
                sb.AppendLine("                [title]=\"'" + colName + " * '\">");
                sb.AppendLine("            </dft-label-validation>");
                sb.AppendLine("            <input required type=\"text\" id=\"" + colName + "\" name=\"" + colName + "\"");
                sb.AppendLine("                formControlName='" + colName + "' pInputText />");
                sb.AppendLine("            <dft-validation [control]=\"form.get('" + colName + "')\">");
                sb.AppendLine("            </dft-validation>");
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
                if (index % 2 == 0 || index == columnList.Count)
                {
                    sb.AppendLine("</div>");
                }
                index++;
            }
            sb1.AppendLine("});");
            sb1.AppendLine("}");
            sb1.AppendLine(sb.ToString());
            string path = txtPath.Text.Trim() + @"\" + TableName + ".ts";
            File.WriteAllText(path, sb1.ToString());
        }

        bool checkAll = false;
        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            checkAll = !checkAll;
            if (checkAll)
            {
                btnCheckAll.Text = "UnCheck All";
                for (int i = 0; i < clbTable.Items.Count; i++)
                {
                    clbTable.SetItemChecked(i, true);
                }
            }
            else
            {
                btnCheckAll.Text = "Check All";
                for (int i = 0; i < clbTable.Items.Count; i++)
                {
                    clbTable.SetItemChecked(i, false);
                }
            }
            btnEntity.Enabled = btnService.Enabled = btnClient.Enabled = checkAll && !string.IsNullOrEmpty(txtPath.Text.Trim());
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            btnEntity.Enabled = btnService.Enabled = btnClient.Enabled = GetEnableGenerateCode();
        }

        private bool GetEnableGenerateCode()
        {
            return clbTable.CheckedItems.Count > 0 && !string.IsNullOrEmpty(txtPath.Text.Trim());
        }


    }
}