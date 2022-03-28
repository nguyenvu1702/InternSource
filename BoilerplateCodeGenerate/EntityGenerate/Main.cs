namespace EntityGenerate
{
    using GetDatabaseInfor;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private List<string> tableList = new List<string>();

        #region Entity click
        private void btnEntity_Click(object sender, EventArgs e)
        {
            try
            {
                Entity_Click();
                MessageBox.Show("Thành công");
            }
            catch (Exception exx)
            {
                MessageBox.Show("Thất bại : " + exx.Message);
            }
        }

        private void Entity_Click()
        {
            // Tạo thư mục chứa các Entity
            string entityPath = txtPath.Text.Trim() + @"\Entity";
            CreateDirectory(entityPath);

            StringBuilder dbSetSb = new StringBuilder();
            foreach (var table in tableList)
            {
                GenerateEntity(table.Trim(), entityPath);
                dbSetSb.AppendLine("public DbSet<" + table + "> " + table + " { get; set; }");
            }
            string pathDbset = entityPath + @"\DbSet.cs";
            File.WriteAllText(pathDbset, dbSetSb.ToString());
        }

        /// <summary>
        /// Tạo từng file Entity
        /// </summary>
        /// <param name="TableName"></param>
        private void GenerateEntity(string TableName, string path)
        {
            StringBuilder sb = new StringBuilder();
            // Lấy hết các column trong table
            var columnList = Database.GetListColumnNames(cbbDb.Text.Trim(), TableName);
            var idColumn = columnList.FirstOrDefault(e => e.ItemArray[3].ToString() == "Id");
            sb.AppendLine("namespace DbEntities");
            sb.AppendLine("{");
            sb.AppendLine("    using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine("    using Abp.Domain.Entities;");
            sb.AppendLine("    using Abp.Domain.Entities.Auditing;");
            sb.AppendLine(string.Empty);
            sb.AppendLine("    [Table(\"" + TableName + "\")]");
            sb.AppendLine("    public class " + TableName + " : FullAuditedEntity" + (idColumn != null 
                && idColumn[7].ToString() == "bigint" ? "<long>" : "") + ", IMayHaveTenant");
            sb.AppendLine("    {");
            sb.AppendLine("        public virtual int? TenantId { get; set; }");
            sb.AppendLine(string.Empty);

            if (idColumn != null)
            {
                columnList.Remove(idColumn);
            }

            int dem = 1;
            int count = columnList.Count;

            foreach (var item in columnList)
            {
                sb.AppendLine(Database.GetProperty(item.ItemArray));
                if (dem < count)
                {
                    sb.AppendLine(string.Empty);
                    dem++;
                }
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");
            path += @"\" + TableName + ".cs";
            File.WriteAllText(path, sb.ToString());
        }
        #endregion

        #region Service click
        private void btnService_Click(object sender, EventArgs e)
        {
            try
            {
                Service_Click();
                MessageBox.Show("Thành công");
            }
            catch (Exception exx)
            {
                MessageBox.Show("Thất bại : " + exx.Message);
            }
        }

        private void Service_Click()
        {
            // Tạo thư mục chứa các Entity
            string parentPath = txtPath.Text.Trim() + @"\Service";
            CreateDirectory(parentPath);

            // Danh sách tất cả service
            string serviceNames = string.Join(",", tableList);

            // Thư mục cha
            string path = parentPath;
            string serviceBaseName = txtServiceBaseName.Text.Trim();
            StringBuilder sb = new StringBuilder();
            StringBuilder createInputDtoSb = new StringBuilder();
            StringBuilder dtoSb = new StringBuilder();
            StringBuilder forViewDtoSb = new StringBuilder();
            StringBuilder getAllInputDtoSb = new StringBuilder();

            if (!string.IsNullOrEmpty(serviceNames) && !string.IsNullOrEmpty(path))
            {
                var nameSpace = path.Split('\\').Where(w => w != "").Last();
                var listServiceName = serviceNames.Split(',').Where(w => w != "").ToList();

                // Tạo thư mục chứa các service
                CreateDirectory(path);

                // Tạo thư mục chứa service nhỏ
                foreach (var item in listServiceName)
                {
                    string servicePath = path + @"\" + item;
                    string dtoPath = servicePath + @"\Dtos\";
                    string namespaceName = "QuanLy" + item;
                    CreateDirectory(servicePath);
                    CreateDirectory(dtoPath);

                    #region Dtos
                    // CreateInputDto
                    createInputDtoSb = new StringBuilder();
                    var columnList = Database.GetListColumnNames(cbbDb.Text.Trim(), item);
                    var idColumn = columnList.FirstOrDefault(e => e.ItemArray[3].ToString() == "Id");
                    createInputDtoSb.AppendLine(string.Format("namespace {0}.{1}.Dtos", serviceBaseName, namespaceName));
                    createInputDtoSb.AppendLine("{");
                    createInputDtoSb.AppendLine("    using Abp.Application.Services.Dto;");
                    createInputDtoSb.AppendLine("    using Abp.AutoMapper;");
                    createInputDtoSb.AppendLine("    using DbEntities;");
                    createInputDtoSb.AppendLine(string.Empty);
                    createInputDtoSb.AppendLine(string.Format("    [AutoMap(typeof({0}))]", item));
                    createInputDtoSb.AppendLine(string.Format("    public class {0}CreateInputDto : EntityDto<{1}?>", item, (idColumn != null
                        && idColumn[7].ToString() == "bigint" ? "<long>" : "int")));
                    createInputDtoSb.AppendLine("    {");

                    // Dto
                    dtoSb = new StringBuilder();
                    dtoSb.AppendLine(string.Format("namespace {0}.{1}.Dtos", serviceBaseName, namespaceName));
                    dtoSb.AppendLine("{");
                    dtoSb.AppendLine("    using Abp.Application.Services.Dto;");
                    dtoSb.AppendLine("    using Abp.AutoMapper;");
                    dtoSb.AppendLine("    using DbEntities;");
                    dtoSb.AppendLine(string.Empty);
                    dtoSb.AppendLine(string.Format("    [AutoMap(typeof({0}))]", item));
                    dtoSb.AppendLine(string.Format("    public class {0}Dto : EntityDto<{1}>", item, (idColumn != null
                        && idColumn[7].ToString() == "bigint" ? "<long>" : "int")));
                    dtoSb.AppendLine("    {");

                    if (idColumn != null)
                    {
                        columnList.Remove(idColumn);
                    }

                    int index = 0;
                    foreach (var column in columnList)
                    {
                        createInputDtoSb.AppendLine(Database.GetProperty(column.ItemArray));
                        dtoSb.AppendLine(Database.GetProperty(column.ItemArray));
                        if (index < columnList.Count - 1)
                        {
                            createInputDtoSb.AppendLine(string.Empty);
                            dtoSb.AppendLine(string.Empty);
                        }
                        index++;
                    }

                    // CreateInputDto
                    string createInputDto = item + "CreateInputDto";
                    createInputDtoSb.AppendLine("    }");
                    createInputDtoSb.AppendLine("}");
                    createInputDtoSb.Replace("virtual ", string.Empty);
                    File.WriteAllText(dtoPath + createInputDto + ".cs", createInputDtoSb.ToString());

                    // Dto
                    string dto = item + "Dto";
                    dtoSb.AppendLine("    }");
                    dtoSb.AppendLine("}");
                    dtoSb.Replace("virtual ", string.Empty);
                    File.WriteAllText(dtoPath + dto + ".cs", dtoSb.ToString());

                    // ForViewDto
                    string forViewDto = item + "ForViewDto";
                    forViewDtoSb = new StringBuilder();
                    forViewDtoSb.AppendLine(string.Format("namespace {0}.{1}.Dtos", serviceBaseName, namespaceName));
                    forViewDtoSb.AppendLine("{");
                    forViewDtoSb.AppendLine("    using DbEntities;");
                    forViewDtoSb.AppendLine(string.Empty);
                    forViewDtoSb.AppendLine(string.Format("    public class {0}ForViewDto", item));
                    forViewDtoSb.AppendLine("    {");
                    forViewDtoSb.AppendLine("        public " + item + " " + item + " { get; set; }");
                    forViewDtoSb.AppendLine("    }");
                    forViewDtoSb.AppendLine("}");
                    File.WriteAllText(dtoPath + forViewDto + ".cs", forViewDtoSb.ToString());

                    // GetAllInputDto
                    string GetAllInputDto = item + "GetAllInputDto";
                    getAllInputDtoSb = new StringBuilder();
                    getAllInputDtoSb.AppendLine(string.Format("namespace {0}.{1}.Dtos", serviceBaseName, namespaceName));
                    getAllInputDtoSb.AppendLine("{");
                    getAllInputDtoSb.AppendLine("    using Abp.Application.Services.Dto;");
                    getAllInputDtoSb.AppendLine(string.Empty);
                    getAllInputDtoSb.AppendLine(string.Format("    public class {0}GetAllInputDto : PagedAndSortedResultRequestDto", item));
                    getAllInputDtoSb.AppendLine("    {");
                    getAllInputDtoSb.AppendLine("        public string Keyword { get; set; }");
                    getAllInputDtoSb.AppendLine("    }");
                    getAllInputDtoSb.AppendLine("}");
                    File.WriteAllText(dtoPath + GetAllInputDto + ".cs", getAllInputDtoSb.ToString());
                    #endregion

                    #region Interface
                    sb = new StringBuilder();
                    string interfaceName = "I" + item + "AppService";
                    string itfaceFile = servicePath + @"\" + interfaceName + ".cs";
                    sb.AppendLine(string.Format("namespace {0}.{1}", serviceBaseName, namespaceName));
                    sb.AppendLine("{");
                    sb.AppendLine("    using System.Threading.Tasks;");
                    sb.AppendLine("    using Abp.Application.Services.Dto;");
                    sb.AppendLine(string.Format("    using {0}.Data;", serviceBaseName));
                    sb.AppendLine(string.Format("    using {0}.{1}.Dtos;", serviceBaseName, namespaceName));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("    public interface {0}", interfaceName));
                    sb.AppendLine("    {");
                    sb.AppendLine(string.Format("        Task<PagedResultDto<{0}>> GetAllAsync({1} input);", forViewDto, GetAllInputDto));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        Task<int> CreateOrEdit({0} input);", createInputDto));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        Task<{0}> GetForEditAsync(EntityDto input);", createInputDto));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        Task DeleteAsync(EntityDto input);");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        Task<FileDto> ExportToExcel({0} input);", GetAllInputDto));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        Task<string> ImportFileExcel(string filePath);");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        Task<FileDto> DownloadFileMau();");
                    sb.AppendLine("    }");
                    sb.AppendLine("}");
                    File.WriteAllText(itfaceFile, sb.ToString());
                    #endregion

                    #region Service
                    string repository = GetFirstToLower(item) + "Repository";
                    string serviceName = item + "AppService";
                    sb = new StringBuilder();
                    sb.AppendLine(string.Format("namespace {0}.{1}", serviceBaseName, namespaceName));
                    sb.AppendLine("{");
                    sb.AppendLine("    using System;");
                    sb.AppendLine("    using System.Drawing;");
                    sb.AppendLine("    using System.IO;");
                    sb.AppendLine("    using System.Linq;");
                    sb.AppendLine("    using System.Linq.Dynamic.Core;");
                    sb.AppendLine("    using System.Text;");
                    sb.AppendLine("    using System.Threading.Tasks;");
                    sb.AppendLine("    using Abp.Application.Services.Dto;");
                    sb.AppendLine("    using Abp.Domain.Repositories;");
                    sb.AppendLine("    using Abp.Linq.Extensions;");
                    sb.AppendLine("    using Abp.UI;");
                    sb.AppendLine("    using DbEntities;");
                    sb.AppendLine("    using Microsoft.EntityFrameworkCore;");
                    sb.AppendLine("    using MyProject.Data;");
                    sb.AppendLine("    using MyProject.Data.Excel.Dtos;");
                    sb.AppendLine("    using MyProject.Global;");
                    sb.AppendLine("    using MyProject.Net.MimeTypes;");
                    sb.AppendLine(string.Format("    using MyProject.{0}.Dtos;", namespaceName));
                    sb.AppendLine("    using OfficeOpenXml;");
                    sb.AppendLine("    using OfficeOpenXml.Style;");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("    public class {0} : {1}, {2}", serviceName, serviceBaseName + "AppServiceBase", interfaceName));
                    sb.AppendLine("    {");
                    sb.AppendLine(string.Format("        private readonly IRepository<{0}> {1};", item, repository));
                    sb.AppendLine("        private readonly IAppFolders appFolders;");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        public {0}(IRepository<{1}> {2}, IAppFolders appFolders)", serviceName, item, repository));
                    sb.AppendLine("        {");
                    sb.AppendLine(string.Format("            this.{0} = {1};", repository, repository));
                    sb.AppendLine("            this.appFolders = appFolders;");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        public async Task<PagedResultDto<{0}>> GetAllAsync({1} input)", forViewDto, GetAllInputDto));
                    sb.AppendLine("        {");
                    sb.AppendLine("            #region Check null");
                    sb.AppendLine("            if (input == null)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                throw new UserFriendlyException(StringResources.NullParameter);");
                    sb.AppendLine("            }");
                    sb.AppendLine("            #endregion");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("            var filter = this.{0}.GetAll()", repository));
                    sb.AppendLine("                                .WhereIf(!string.IsNullOrEmpty(input.Keyword), e => e.Ma.Contains(GlobalFunction.RegexFormat(input.Keyword))");
                    sb.AppendLine("|| e.Ten.Contains(GlobalFunction.RegexFormat(input.Keyword)));");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            var query = from o in filter");
                    sb.AppendLine(string.Format("                        select new {0}()", forViewDto));
                    sb.AppendLine("                        {");
                    sb.AppendLine(string.Format("                            {0} = o,", item));
                    sb.AppendLine("                        };");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            int totalCount = await query.CountAsync();");
                    sb.AppendLine(string.Format("            var output = await query.OrderBy(input.Sorting ?? \"{0}.Id\")", item));
                    sb.AppendLine("                                    .PageBy(input)");
                    sb.AppendLine("                                    .ToListAsync();");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("            return new PagedResultDto<{0}>(totalCount, output);", forViewDto));
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        public async Task<{0}> GetForEditAsync(EntityDto input)", createInputDto));
                    sb.AppendLine("        {");
                    sb.AppendLine("            #region Check null");
                    sb.AppendLine("            if (input == null)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                throw new UserFriendlyException(StringResources.NullParameter);");
                    sb.AppendLine("            }");
                    sb.AppendLine("            #endregion");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("            var entity = await this.{0}.FirstOrDefaultAsync(input.Id);", repository));
                    sb.AppendLine(string.Format("            var edit = this.ObjectMapper.Map<{0}>(entity);", createInputDto));
                    sb.AppendLine("            return await Task.FromResult(edit);");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        public async Task<int> CreateOrEdit({0} input)", createInputDto));
                    sb.AppendLine("        {");
                    sb.AppendLine("            #region Check null");
                    sb.AppendLine("            if (input == null)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                throw new UserFriendlyException(StringResources.NullParameter);");
                    sb.AppendLine("            }");
                    sb.AppendLine("            #endregion");
                    sb.AppendLine(string.Empty);

                    foreach (var column in columnList)
                    {
                        string colName = column[3].ToString();
                        string colType = column[7].ToString();
                        if (colType.Contains("nvarchar") || colType.Contains("nchar"))
                        {

                            sb.AppendLine(string.Format("            input.{0} = GlobalFunction.RegexFormat(input.{1});", colName, colName));
                        }
                        else if (colType.Contains("date"))
                        {
                            sb.AppendLine(string.Format("            input.{0} = GlobalFunction.GetDateTime(input.{1});", colName, colName));
                        }
                    }

                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            if (this.CheckExist(input.Ma, input.Id))");
                    sb.AppendLine("            {");
                    sb.AppendLine("                return 1;");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            if (input.Id == null)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                await this.Create(input);");
                    sb.AppendLine("            }");
                    sb.AppendLine("            else");
                    sb.AppendLine("            {");
                    sb.AppendLine("                await this.Update(input);");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            return 0;");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        public async Task DeleteAsync(EntityDto input)");
                    sb.AppendLine("        {");
                    sb.AppendLine("            #region Check null");
                    sb.AppendLine("            if (input == null)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                throw new UserFriendlyException(StringResources.NullParameter);");
                    sb.AppendLine("            }");
                    sb.AppendLine("            #endregion");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("            await this.{0}.DeleteAsync((int)input.Id);", repository));
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        public async Task<FileDto> ExportToExcel({0} input)", GetAllInputDto));
                    sb.AppendLine("        {");
                    sb.AppendLine("            #region Check null");
                    sb.AppendLine("            if (input == null)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                throw new UserFriendlyException(StringResources.NullParameter);");
                    sb.AppendLine("            }");
                    sb.AppendLine("            #endregion");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Lấy danh sách cần xuất excel");
                    sb.AppendLine("            var list = await this.GetAllAsync(input);");
                    sb.AppendLine("            using var package = new ExcelPackage();");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Add sheet");
                    sb.AppendLine(string.Format("            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(\"{0}\");", item));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            var namedStyle = package.Workbook.Styles.CreateNamedStyle(\"HyperLink\");");
                    sb.AppendLine("            namedStyle.Style.Font.UnderLine = true;");
                    sb.AppendLine("            namedStyle.Style.Font.Color.SetColor(Color.Blue);");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // set header");

                    index = 0;
                    foreach (var column in columnList)
                    {
                        index++;
                        string colName = column[3].ToString();
                        sb.AppendLine(string.Format("            worksheet.Cells[1, {1}].Value = \"{0}\";", colName, index));
                    }

                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Bôi đậm header");
                    sb.AppendLine(string.Format("            using (ExcelRange r = worksheet.Cells[1, 1, 1, {0}])", index));
                    sb.AppendLine("            {");
                    sb.AppendLine("                using var f = new Font(\"Calibri\", 12, FontStyle.Bold);");
                    sb.AppendLine("                r.Style.Font.SetFromFont(f);");
                    sb.AppendLine("                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Gan gia tri");
                    sb.AppendLine("            var rowNumber = 2;");
                    sb.AppendLine("            list.Items.ToList().ForEach(item =>");
                    sb.AppendLine("            {");

                    index = 0;
                    foreach (var column in columnList)
                    {
                        index++;
                        string colName = column[3].ToString();
                        sb.AppendLine(string.Format("                worksheet.Cells[rowNumber, {0}].Value = item.{1}.{2};", index, item, colName));
                    }

                    sb.AppendLine("                rowNumber++;");
                    sb.AppendLine("            });");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Cho các ô rộng theo dữ liệu");
                    sb.AppendLine("            worksheet.Cells.AutoFitColumns(0);");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            worksheet.PrinterSettings.FitToHeight = 1;");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Tên file");
                    sb.AppendLine(string.Format("            var fileName = \"{0}.xlsx\";", item));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Lưu file vào server");
                    sb.AppendLine("            using (var stream = new MemoryStream())");
                    sb.AppendLine("            {");
                    sb.AppendLine("                package.SaveAs(stream);");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);");
                    sb.AppendLine("            var filePath = Path.Combine(this.appFolders.TempFileDownloadFolder, file.FileToken);");
                    sb.AppendLine("            package.SaveAs(new FileInfo(filePath));");
                    sb.AppendLine("            return file;");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine("        /// Import excel.");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine("        /// <param name=\"filePath\">Đường dẫn file trên server.</param>");
                    sb.AppendLine("        /// <returns>Danh sách đường dẫn.</returns>");
                    sb.AppendLine("        public async Task<string> ImportFileExcel(string filePath)");
                    sb.AppendLine("        {");
                    sb.AppendLine("            StringBuilder returnMessage = new StringBuilder();");
                    sb.AppendLine("            returnMessage.Append(\"Kết quả nhập file: \");");
                    sb.AppendLine(string.Format("            ReadFromExcelDto<{0}> readResult = new ReadFromExcelDto<{0}>();", createInputDto, createInputDto));
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Không tìm thấy file");
                    sb.AppendLine("            if (!File.Exists(filePath))");
                    sb.AppendLine("            {");
                    sb.AppendLine("                readResult.ResultCode = (int)GlobalConst.ReadExcelResultCodeConst.FileNotFound;");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Đọc hết file excel");
                    sb.AppendLine("            var data = await GlobalFunction.ReadFromExcel(filePath);");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Không có dữ liệu");
                    sb.AppendLine("            if (data.Count <= 0)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                readResult.ResultCode = (int)GlobalConst.ReadExcelResultCodeConst.CantReadData;");
                    sb.AppendLine("            }");
                    sb.AppendLine("            else");
                    sb.AppendLine("            {");
                    sb.AppendLine("                // Đọc lần lượt từng dòng");
                    sb.AppendLine("                for (int i = 0; i < data.Count; i++)");
                    sb.AppendLine("                {");
                    sb.AppendLine("                    try");
                    sb.AppendLine("                    {");

                    index = 0;
                    foreach (var column in columnList)
                    {
                        index++;
                        string colName = column[3].ToString();
                        string type = column[7].ToString();
                        if (type == "int" || type == "double")
                        {
                            sb.AppendLine(string.Format("                        {0} {1} = {0}.Parse(data[i][{2}]);", type, GetFirstToLower(colName), index));
                        }
                        else
                        {
                            sb.AppendLine(string.Format("                        string {0} = data[i][{1}];", GetFirstToLower(colName), index));
                        }
                    }

                    sb.AppendLine(string.Format("                        var create = new {0}", createInputDto));
                    sb.AppendLine("                        {");

                    foreach (var column in columnList)
                    {
                        index++;
                        string colName = column[3].ToString();
                        sb.AppendLine(string.Format("                            {0} = {1},", colName, GetFirstToLower(colName)));
                    }

                    sb.AppendLine("                        };");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("                        // Nếu không bị trùng");
                    sb.AppendLine("                        if (await this.CreateOrEdit(create) == 0)");
                    sb.AppendLine("                        {");
                    sb.AppendLine("                            // Đánh dấu các bản ghi thêm thành công");
                    sb.AppendLine("                            readResult.ListResult.Add(create);");
                    sb.AppendLine("                        }");
                    sb.AppendLine("                        else");
                    sb.AppendLine("                        {");
                    sb.AppendLine("                            // Đánh dấu các bản ghi lỗi");
                    sb.AppendLine("                            readResult.ListErrorRow.Add(data[i]);");
                    sb.AppendLine("                            readResult.ListErrorRowIndex.Add(i + 1);");
                    sb.AppendLine("                        }");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                    catch");
                    sb.AppendLine("                    {");
                    sb.AppendLine("                        // Đánh dấu các bản ghi lỗi");
                    sb.AppendLine("                        readResult.ListErrorRow.Add(data[i]);");
                    sb.AppendLine("                        readResult.ListErrorRowIndex.Add(i + 1);");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Thông tin import");
                    sb.AppendLine("            readResult.ErrorMessage = GlobalModel.ReadExcelResultCodeSorted[readResult.ResultCode];");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Nếu đọc file thất bại");
                    sb.AppendLine("            if (readResult.ResultCode != 200)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                return readResult.ErrorMessage;");
                    sb.AppendLine("            }");
                    sb.AppendLine("            else");
                    sb.AppendLine("            {");
                    sb.AppendLine("                // Đọc file thành công");
                    sb.AppendLine("                // Trả kết quả import");
                    sb.AppendLine("                returnMessage.Append(string.Format(\"\\r\\n\\u00A0-Tổng ghi: {0}\", readResult.ListResult.Count + readResult.ListErrorRow.Count));");
                    sb.AppendLine("                returnMessage.Append(string.Format(\"\\r\\n\\u00A0-Số bản ghi thành công: {0}\", readResult.ListResult.Count));");
                    sb.AppendLine("                returnMessage.Append(string.Format(\"\\r\\n\\u00A0-Số bản ghi thất bại: {0}\", readResult.ListErrorRow.Count));");
                    sb.AppendLine();
                    sb.AppendLine("                if (readResult.ListErrorRowIndex.Count > 0)");
                    sb.AppendLine("                {");
                    sb.AppendLine("                    returnMessage.Append(string.Format(\"\\r\\n\\u00A0-Các dòng thất bại: {0}\", string.Join(\", \", readResult.ListErrorRowIndex)));");
                    sb.AppendLine("                }");
                    sb.AppendLine("            }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            return returnMessage.ToString();");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine("        /// Tải file mẫu cho import.");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine("        /// <returns>File mẫu.</returns>");
                    sb.AppendLine("        public async Task<FileDto> DownloadFileMau()");
                    sb.AppendLine("        {");
                    sb.AppendLine(string.Format("            string fileName = \"{0}Import.xlsx\";", item));
                    sb.AppendLine("            try");
                    sb.AppendLine("            {");
                    sb.AppendLine("                // _appFolders.DemoFileDownloadFolder : Thư mục chưa file mẫu cần tải");
                    sb.AppendLine("                // _appFolders.TempFileDownloadFolder : Không được sửa");
                    sb.AppendLine(string.Format("                return await GlobalFunction.DownloadFileMau(fileName, this.appFolders.{0}FileDownloadFolder, this.appFolders.TempFileDownloadFolder);", item));
                    sb.AppendLine("            }");
                    sb.AppendLine("            catch (Exception ex)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                throw new UserFriendlyException(\"Có lỗi: \" + ex.Message);");
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("        private bool CheckExist(string ma, int? id)");
                    sb.AppendLine("        {");
                    sb.AppendLine("            ma = GlobalFunction.RegexFormat(ma);");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine("            // Nếu query > 0 thì là bị trùng mã => return true");
                    sb.AppendLine(string.Format("            var query = this.{0}.GetAll().Where(e => e.{1} == ma)", repository, columnList[0][3].ToString()));
                    sb.AppendLine("                .WhereIf(id != null, e => e.Id != id).Count();");
                    sb.AppendLine("            return query > 0;");
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        private async Task Create({0} input)", createInputDto));
                    sb.AppendLine("        {");
                    sb.AppendLine(string.Format("            var create = this.ObjectMapper.Map<{0}>(input);", item));
                    sb.AppendLine(string.Format("            await this.{0}.InsertAndGetIdAsync(create);", repository));
                    sb.AppendLine("        }");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("        private async Task Update({0} input)", createInputDto));
                    sb.AppendLine("        {");
                    sb.AppendLine(string.Format("            var update = await this.{0}.FirstOrDefaultAsync((int)input.Id);", repository));
                    sb.AppendLine("            this.ObjectMapper.Map(input, update);");
                    sb.AppendLine("        }");
                    sb.AppendLine("    }");
                    sb.AppendLine("}");

                    string serviceFile = servicePath + @"\" + item + "AppService.cs";
                    File.WriteAllText(serviceFile, sb.ToString());
                    #endregion
                }
            }
        }
        #endregion

        #region Client click
        private void btnClient_Click(object sender, EventArgs e)
        {
            try
            {
                Client_Click();
                MessageBox.Show("Thành công");
            }
            catch (Exception exx)
            {
                MessageBox.Show("Thất bại : " + exx.Message);
            }
        }

        private void Client_Click()
        {
            // Tạo thư mục chứa các Entity
            string parentPath = txtPath.Text.Trim() + @"\Client";
            CreateDirectory(parentPath);

            foreach (var item in tableList)
            {
                GenerateClient(item, parentPath);
            }
        }

        /// <summary>
        /// Tạo từng file Client
        /// </summary>
        /// <param name="TableName"></param>
        private void GenerateClient(string tableName, string path)
        {
            path = path + @"\" + tableName;
            CreateDirectory(path);

            StringBuilder sb = new StringBuilder();
            // Lấy hết các column trong table
            var columnList = Database.GetListColumnNames(cbbDb.Text.Trim(), tableName);
            var idColumn = columnList.FirstOrDefault(e => e.ItemArray[3].ToString() == "Id");
            if (idColumn != null)
            {
                columnList.Remove(idColumn);
            }

            #region danh sách ts
            string dtoName = GetFirstToLower(tableName); ;
            string serviceProxyName = "_" + dtoName;
            sb.AppendLine("import { HttpClient } from '@angular/common/http';");
            sb.AppendLine("import { Component, Injector, OnInit, ViewChild } from '@angular/core';");
            sb.AppendLine("import { appModuleAnimation } from '@shared/animations/routerTransition';");
            sb.AppendLine("import { AppComponentBase } from '@shared/app-component-base';");
            sb.AppendLine("import { AppConsts } from '@shared/AppConsts';");
            sb.AppendLine("import { ImportExcelDialogComponent } from '@shared/components/import-excel/import-excel-dialog.component';");
            sb.AppendLine("import { FileDownloadService } from '@shared/file-download.service';");
            sb.AppendLine("import { " + tableName + "ForViewDto,  " + tableName + "GetAllInputDto,  " + tableName + "ServiceProxy } from '@shared/service-proxies/service-proxies';");
            sb.AppendLine("import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';");
            sb.AppendLine("import { LazyLoadEvent } from 'primeng/api';");
            sb.AppendLine("import { Table } from 'primeng/table';");
            sb.AppendLine("import { finalize } from 'rxjs/operators';");
            sb.AppendLine("import { CreateOrEdit" + tableName + "Component } from './create-or-edit-" + GetClientFileName(tableName) +
                "/create-or-edit-" + GetClientFileName(tableName) + ".component';");
            sb.AppendLine(string.Empty);
            sb.AppendLine("const URL = AppConsts.remoteServiceBaseUrl + '/api/Upload/" + tableName + "Upload';");
            sb.AppendLine("@Component({");
            sb.AppendLine("  selector: 'app-" + GetClientFileName(tableName) + "',");
            sb.AppendLine("  templateUrl: './" + GetClientFileName(tableName) + ".component.html',");
            sb.AppendLine("  animations: [appModuleAnimation()],");
            sb.AppendLine("})");
            sb.AppendLine("export class " + tableName + "Component extends AppComponentBase implements OnInit {");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  @ViewChild('dt') table: Table;");
            sb.AppendLine("  loading = true;");
            sb.AppendLine("  exporting = false;");
            sb.AppendLine("  keyword = '';");
            sb.AppendLine("  totalCount = 0;");
            sb.AppendLine("  records: " + tableName + "ForViewDto[] = [];");
            sb.AppendLine("  input: " + tableName + "GetAllInputDto;");
            sb.AppendLine("  constructor(");
            sb.AppendLine("    injector: Injector,");
            sb.AppendLine("    private _http: HttpClient,");
            sb.AppendLine("    private _modalService: BsModalService,");
            sb.AppendLine("    private _fileDownloadService: FileDownloadService,");
            sb.AppendLine("    private " + serviceProxyName + "ServiceProxy: " + tableName + "ServiceProxy,");
            sb.AppendLine("  ) { super(injector); }");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Empty);
            sb.AppendLine("  ngOnInit(): void {");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  getDataPage(lazyLoad?: LazyLoadEvent) {");
            sb.AppendLine("    this.loading = true;");
            sb.AppendLine("    this." + serviceProxyName + "ServiceProxy.getAll(");
            sb.AppendLine("      this.keyword || undefined,");
            sb.AppendLine("      this.getSortField(this.table),");
            sb.AppendLine("      lazyLoad ? lazyLoad.first : this.table.first,");
            sb.AppendLine("      lazyLoad ? lazyLoad.rows : this.table.rows,");
            sb.AppendLine("    ).pipe(finalize(() => { this.loading = false; }))");
            sb.AppendLine("      .subscribe(result => {");
            sb.AppendLine("        this.records = result.items;");
            sb.AppendLine("        this.totalCount = result.totalCount;");
            sb.AppendLine("      });");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  create(id?: number) {");
            sb.AppendLine("    this._showCreateOrEdit" + tableName + "Dialog(id);");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  view(id?: number) {");
            sb.AppendLine("    this._showCreateOrEdit" + tableName + "Dialog(id, true);");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  delete(record: " + tableName + "ForViewDto) {");
            sb.AppendLine("    this.swal.fire({");
            sb.AppendLine("      title: 'Bạn có chắc chắn không?',");
            sb.AppendLine("      text: '" + tableName + " ' + record." + dtoName + ".ten" + tableName + " + ' sẽ bị xóa!',");
            sb.AppendLine("      icon: 'warning',");
            sb.AppendLine("      showCancelButton: true,");
            sb.AppendLine("      confirmButtonColor: this.confirmButtonColor,");
            sb.AppendLine("      cancelButtonColor: this.cancelButtonColor,");
            sb.AppendLine("      cancelButtonText: this.cancelButtonText,");
            sb.AppendLine("      confirmButtonText: this.confirmButtonText");
            sb.AppendLine("    }).then((result) => {");
            sb.AppendLine("      if (result.value) {");
            sb.AppendLine("        this." + serviceProxyName + "ServiceProxy.delete(record." + dtoName + ".id).subscribe(() => {");
            sb.AppendLine("          this.showDeleteMessage();");
            sb.AppendLine("          this.getDataPage();");
            sb.AppendLine("        });");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  importExcel() {");
            sb.AppendLine("    this._showImport" + tableName + "Dialog();");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  exportToExcel() {");
            sb.AppendLine("    this.exporting = true;");
            sb.AppendLine("    this.input = new " + tableName + "GetAllInputDto();");
            sb.AppendLine("    this.input.skipCount = 0;");
            sb.AppendLine("    this.input.maxResultCount = 10000000;");
            sb.AppendLine("    this." + serviceProxyName + "ServiceProxy.exportToExcel(this.input).subscribe((result) => {");
            sb.AppendLine("      this._fileDownloadService.downloadTempFile(result);");
            sb.AppendLine("      this.exporting = false;");
            sb.AppendLine("    }, () => {");
            sb.AppendLine("      this.exporting = false;");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  private _showCreateOrEdit" + tableName + "Dialog(id?: number, isView = false): void {");
            sb.AppendLine("    // copy");
            sb.AppendLine("    let createOrEditDialog: BsModalRef;");
            sb.AppendLine("    createOrEditDialog = this._modalService.show(");
            sb.AppendLine("      CreateOrEdit" + tableName + "Component,");
            sb.AppendLine("      {");
            sb.AppendLine("        class: 'modal-xl',");
            sb.AppendLine("        ignoreBackdropClick: true,");
            sb.AppendLine("        initialState: {");
            sb.AppendLine("          id,");
            sb.AppendLine("          isView,");
            sb.AppendLine("        },");
            sb.AppendLine("      }");
            sb.AppendLine("    );");
            sb.AppendLine(string.Empty);
            sb.AppendLine("    // ouput emit");
            sb.AppendLine("    createOrEditDialog.content.onSave.subscribe(() => {");
            sb.AppendLine("      this.getDataPage();");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  private _showImport" + tableName + "Dialog(): void {");
            sb.AppendLine("    let importExcelDialog: BsModalRef;");
            sb.AppendLine(string.Empty);
            sb.AppendLine("    importExcelDialog = this._modalService.show(");
            sb.AppendLine("      ImportExcelDialogComponent,");
            sb.AppendLine("      {");
            sb.AppendLine("        class: 'modal-lg',");
            sb.AppendLine("        ignoreBackdropClick: true,");
            sb.AppendLine("        initialState: {");
            sb.AppendLine("          maxFile: 1,");
            sb.AppendLine("          excelAcceptTypes: this.excelAcceptTypes");
            sb.AppendLine("        }");
            sb.AppendLine("      }");
            sb.AppendLine("    );");
            sb.AppendLine(string.Empty);
            sb.AppendLine("    // Tải file mẫu");
            sb.AppendLine("    importExcelDialog.content.onDownload.subscribe(() => {");
            sb.AppendLine("      this." + serviceProxyName + "ServiceProxy.downloadFileMau().subscribe(result => {");
            sb.AppendLine("        importExcelDialog.content.downLoading = false;");
            sb.AppendLine("        this._fileDownloadService.downloadTempFile(result);");
            sb.AppendLine("      });");
            sb.AppendLine("    });");
            sb.AppendLine(string.Empty);
            sb.AppendLine("    // Upload");
            sb.AppendLine("    importExcelDialog.content.onSave.subscribe((ouput) => {");
            sb.AppendLine("      importExcelDialog.content.returnMessage = 'Đang upload file....';");
            sb.AppendLine("      const formdata = new FormData();");
            sb.AppendLine("      for (let i = 0; i < ouput.length; i++) {");
            sb.AppendLine("        formdata.append((i + 1) + '', ouput[i]);");
            sb.AppendLine("      }");
            sb.AppendLine("      this._http.post(URL, formdata).subscribe((res) => {");
            sb.AppendLine("        this." + serviceProxyName + "ServiceProxy.importFileExcel(res['result'][0]).subscribe((message) => {");
            sb.AppendLine("          importExcelDialog.content.returnMessage = message;");
            sb.AppendLine("          importExcelDialog.content.uploading = false;");
            sb.AppendLine("          importExcelDialog.content.uploadDone();");
            sb.AppendLine("          this.showUploadMessage();");
            sb.AppendLine("        });");
            sb.AppendLine("      });");
            sb.AppendLine("    });");
            sb.AppendLine(string.Empty);
            sb.AppendLine("    // Close");
            sb.AppendLine("    importExcelDialog.content.onClose.subscribe(() => {");
            sb.AppendLine("      this.getDataPage();");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");
            File.WriteAllText(path + @"\" + GetClientFileName(tableName) + ".component.ts", sb.ToString());
            #endregion

            #region danh sách html
            sb = new StringBuilder();
            sb.AppendLine("<div [@routerTransition]>");
            sb.AppendLine("    <section class=\"content-header\" id=\"tesst\">");
            sb.AppendLine("        <div class=\"container-fluid\">");
            sb.AppendLine("            <div class=\"row\">");
            sb.AppendLine("                <div class=\"col-6\">");
            sb.AppendLine("                    <h1>" + tableName + "</h1>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class=\"col-6 text-right\">");
            sb.AppendLine("                    <button type=\"button\" class=\"btn btn-success m-r-5\" [buttonBusy]=\"exporting\"");
            sb.AppendLine("                        (click)=\"exportToExcel()\"><i class=\"fa fa-download\"></i>");
            sb.AppendLine("                        <span style=\"margin-left: 5px;\">Xuất Excel</span></button>");
            sb.AppendLine(string.Empty);
            sb.AppendLine("                    <button type=\"button\" class=\"btn btn-success m-r-5\" (click)=\"importExcel()\">");
            sb.AppendLine("                        <i class=\"fa fa-upload\"></i>");
            sb.AppendLine("                        <span style=\"margin-left: 5px;\">Nhập Excel</span></button>");
            sb.AppendLine(string.Empty);
            sb.AppendLine("                    <button type=\"button\" class=\"btn btn-primary m-r-5\" (click)=\"create()\">");
            sb.AppendLine("                        <i class=\"fa fa-plus\"></i>");
            sb.AppendLine("                        <span style=\"margin-left: 5px;\">Thêm mới</span></button>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </section>");
            sb.AppendLine("    <section class=\"content px-2\">");
            sb.AppendLine("        <div class=\"container-fluid\">");
            sb.AppendLine("            <div class=\"card m-0\">");
            sb.AppendLine("                <div class=\"card-header\">");
            sb.AppendLine("                    <div class=\"input-group\">");
            sb.AppendLine("                        <div class=\"input-group-prepend\">");
            sb.AppendLine("                            <button type=\"button\" class=\"btn bg-blue\" (click)=\"getDataPage()\">");
            sb.AppendLine("                                <i class=\"fas fa-search\"></i>");
            sb.AppendLine("                            </button>");
            sb.AppendLine("                        </div>");
            sb.AppendLine("                        <input type=\"text\" class=\"form-control\" name=\"keyword\" [placeholder]=\"'Nhập từ khóa'\"");
            sb.AppendLine("                            [(ngModel)]=\"keyword\" (keyup.enter)=\"getDataPage()\" />");
            sb.AppendLine("                    </div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div>");
            sb.AppendLine("                    <p-table #dt [value]=\"records\" [lazy]=\"true\" (onLazyLoad)=\"getDataPage($event)\"");
            sb.AppendLine("                        [paginator]=\"paginator\" [loading]=\"loading\" [showCurrentPageReport]=\"showCurrentPageReport\"");
            sb.AppendLine("                        currentPageReportTemplate=\"{{totalCount==0?'':' Hiển thị: {first}-{last}/{totalRecords}'}} \"");
            sb.AppendLine("                        [rows]=\"paginatorRows\" [totalRecords]=\"totalCount\" [rowsPerPageOptions]=\"rowsPerPageOptions\"");
            sb.AppendLine("                        [scrollable]=\"scrollable\" scrollHeight=\"{{scrollHeight}}\">");
            sb.AppendLine("                        <ng-template pTemplate=\"header\">");
            sb.AppendLine("                            <tr>");
            sb.AppendLine("                                <th class=\"width-30\"></th>");

            foreach (var column in columnList)
            {
                string colName = column[3].ToString();
                string pSortableColumn = dtoName + "." + GetFirstToLower(colName);
                sb.AppendLine("                                <th class=\"width-150\" pSortableColumn=\"" + pSortableColumn + "\">" + colName);
                sb.AppendLine("                                    <p-sortIcon field=\"" + pSortableColumn + "\"></p-sortIcon>");
                sb.AppendLine("                                </th>");
            }

            sb.AppendLine("                            </tr>");
            sb.AppendLine("                        </ng-template>");
            sb.AppendLine("                        <ng-template pTemplate=\"body\" let-record>");
            sb.AppendLine("                            <tr class=\"ui-selectable-row\">");
            sb.AppendLine("                                <td class=\"width-30\">");
            sb.AppendLine("                                    <div class=\"dropdown\">");
            sb.AppendLine("                                        <button class=\"dropdown-toggle btn btn-sm btn-transparent btn-action\"");
            sb.AppendLine("                                            type=\"button\" id=\"dropdownMenuButton\" data-toggle=\"dropdown\"");
            sb.AppendLine("                                            aria-haspopup=\"true\" aria-expanded=\"false\">");
            sb.AppendLine("                                            <i class=\"fas fa-ellipsis-v\"></i>");
            sb.AppendLine("                                        </button>");
            sb.AppendLine("                                        <div class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuButton\">");
            sb.AppendLine("                                            <a class=\"dropdown-item\" (click)=\"view(record." + dtoName + ".id)\">Xem</a>");
            sb.AppendLine("                                            <a class=\"dropdown-item\" (click)=\"create(record." + dtoName + ".id)\">Cập");
            sb.AppendLine("                                                nhật</a>");
            sb.AppendLine("                                            <a class=\"dropdown-item\" (click)=\"delete(record)\">Xóa</a>");
            sb.AppendLine("                                        </div>");
            sb.AppendLine("                                    </div>");
            sb.AppendLine("                                </td>");

            foreach (var column in columnList)
            {
                string colName = column[3].ToString();
                string pSortableColumn = dtoName + "." + GetFirstToLower(colName);
                sb.AppendLine("                                <td class=\"width-150\">");
                sb.AppendLine("                                    {{ record." + pSortableColumn + " | truncate }}");
                sb.AppendLine("                                </td>");
            }

            sb.AppendLine("                            </tr>");
            sb.AppendLine("                        </ng-template>");
            sb.AppendLine("                        <ng-template pTemplate=\"emptymessage\">");
            sb.AppendLine("                            <tr>");
            sb.AppendLine("                                <td class=\"demo\" colspan=\"8\" class=\"text-left\">{{khongCoDuLieu}}</td>");
            sb.AppendLine("                            </tr>");
            sb.AppendLine("                        </ng-template>");
            sb.AppendLine("                    </p-table>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </section>");
            sb.AppendLine("</div>");
            File.WriteAllText(path + @"\" + GetClientFileName(tableName) + ".component.html", sb.ToString());
            #endregion

            #region create-or-edit.component.ts
            sb = new StringBuilder();
            sb.AppendLine("import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';");
            sb.AppendLine("import { FormBuilder, FormGroup, Validators } from '@angular/forms';");
            sb.AppendLine("import { AppComponentBase } from '@shared/app-component-base';");
            sb.AppendLine("import { CommonComponent } from '@shared/dft/components/common.component';");
            sb.AppendLine("import { " + tableName + "CreateInputDto, " + tableName + "ServiceProxy } from '@shared/service-proxies/service-proxies';");
            sb.AppendLine("import { BsModalRef } from 'ngx-bootstrap/modal';");
            sb.AppendLine("import { finalize } from 'rxjs/operators';");
            sb.AppendLine("@Component({");
            sb.AppendLine("  selector: 'app-create-or-edit-" + GetClientFileName(tableName) + "',");
            sb.AppendLine("  templateUrl: './create-or-edit-" + GetClientFileName(tableName) + ".component.html',");
            sb.AppendLine("})");
            sb.AppendLine("export class CreateOrEdit" + tableName + "Component extends AppComponentBase implements OnInit {");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  @Output() onSave = new EventEmitter<any>();");
            sb.AppendLine("  form: FormGroup;");
            sb.AppendLine("  saving = false;");
            sb.AppendLine("  isEdit = false;");
            sb.AppendLine("  id: number;");
            sb.AppendLine("  isView = false;");
            sb.AppendLine("  createInputDto: " + tableName + "CreateInputDto = new " + tableName + "CreateInputDto();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  constructor(");
            sb.AppendLine("    injector: Injector,");
            sb.AppendLine("    private _fb: FormBuilder,");
            sb.AppendLine("    public bsModalRef: BsModalRef,");
            sb.AppendLine("    private " + serviceProxyName + "ServiceProxy: " + tableName + "ServiceProxy,");
            sb.AppendLine("  ) {");
            sb.AppendLine("    super(injector);");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  ngOnInit(): void {");
            sb.AppendLine("    this.khoiTaoForm();");
            sb.AppendLine("    if (!this.id) {");
            sb.AppendLine("      // Thêm mới");
            sb.AppendLine("      this.createInputDto = new " + tableName + "CreateInputDto();");
            sb.AppendLine("      this.isEdit = false;");
            sb.AppendLine("    } else {");
            sb.AppendLine("      this.isEdit = true;");
            sb.AppendLine("      // Sửa");
            sb.AppendLine("      this." + serviceProxyName + "ServiceProxy.getForEdit(this.id).subscribe(item => {");
            sb.AppendLine("        this.createInputDto = item;");
            sb.AppendLine("        this._setValueForEdit();");
            sb.AppendLine("      });");
            sb.AppendLine("    }");
            sb.AppendLine("    if (this.isView) {");
            sb.AppendLine("      this.form.disable();");
            sb.AppendLine("    } else {");
            sb.AppendLine("      this.form.enable();");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  khoiTaoForm() {");
            sb.AppendLine("    this.form = this._fb.group({");

            foreach (var column in columnList)
            {
                string colName = column[3].ToString();
                string controlName = GetFirstToLower(colName);
                sb.AppendLine("      " + controlName + ": [''],");
            }

            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  save(): void {");
            sb.AppendLine("    if (CommonComponent.getControlErr(this.form) === '') {");
            sb.AppendLine("      this.saving = true;");
            sb.AppendLine("      this._getValueForSave();");
            sb.AppendLine("      this." + serviceProxyName + "ServiceProxy.createOrEdit(this.createInputDto).pipe(");
            sb.AppendLine("        finalize(() => {");
            sb.AppendLine("          this.saving = false;");
            sb.AppendLine("        })");
            sb.AppendLine("      ).subscribe((result) => {");
            sb.AppendLine("        if (result === 1) {");
            sb.AppendLine("          this.showExistMessage('Exits!');");
            sb.AppendLine("        } else if (!this.id) {");
            sb.AppendLine("          this.showCreateMessage();");
            sb.AppendLine("          this.bsModalRef.hide();");
            sb.AppendLine("          this.onSave.emit();");
            sb.AppendLine("        } else {");
            sb.AppendLine("          this.showUpdateMessage();");
            sb.AppendLine("          this.bsModalRef.hide();");
            sb.AppendLine("          this.onSave.emit();");
            sb.AppendLine("        }");
            sb.AppendLine("      });");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  close() {");
            sb.AppendLine("    this.bsModalRef.hide();");
            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  private _getValueForSave() {");

            foreach (var column in columnList)
            {
                string colName = column[3].ToString();
                string controlName = GetFirstToLower(colName);
                sb.AppendLine("    this.createInputDto." + controlName + " = this.form.controls." + controlName + ".value;");
            }

            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("  private _setValueForEdit() {");

            foreach (var column in columnList)
            {
                string colName = column[3].ToString();
                string controlName = GetFirstToLower(colName);
                sb.AppendLine("    this.form.controls." + controlName + ".setValue(this.createInputDto." + controlName + ");");
            }

            sb.AppendLine("  }");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");
            string createOrEditPath = @"\create-or-edit-" + GetClientFileName(tableName);
            CreateDirectory(path + createOrEditPath);
            File.WriteAllText(path + createOrEditPath + @"\" + createOrEditPath + ".component.ts", sb.ToString());
            #endregion

            #region create-or-edit.component.html
            sb = new StringBuilder();
            sb.AppendLine("<form tabindex=\"-1\" [formGroup]=\"form\" class=\"form-horizontal\" autocomplete=\"off\" (ngSubmit)=\"save()\">");
            sb.AppendLine("    <abp-modal-header");
            sb.AppendLine("        [title]=\"id ? (isView ? 'Xem chi tiết: ' + createInputDto." + tableName + " : 'Cập nhật: ' + createInputDto." + tableName + ") : '" + tableName + "'\"");
            sb.AppendLine("        (onCloseClick)=\"bsModalRef.hide()\"></abp-modal-header>");
            sb.AppendLine("    <div class=\"modal-body fixed-modal-height\">");

            int index = 1;
            foreach (var item in columnList)
            {
                string colName = item[3].ToString();
                colName = GetFirstToLower(colName);

                if (index % 2 != 0)
                {
                    sb.AppendLine("    <div class=\"row ui-fluid\">");
                }

                sb.AppendLine("        <div class=\"col-md-6\">");
                sb.AppendLine("            <div class=\"form-group\">");
                sb.AppendLine("                <dft-label-validation [control]=\"form.get('" + colName + "')\"");
                sb.AppendLine("                    [title]=\"'" + colName + "'\">");
                sb.AppendLine("                </dft-label-validation>");
                sb.AppendLine("                <input maxlength=\"50\" type=\"text\" id=\"" + colName + "\" name=\"" + colName + "\"");
                sb.AppendLine("                    formControlName='" + colName + "' pInputText />");
                sb.AppendLine("                <dft-validation [control]=\"form.get('" + colName + "')\">");
                sb.AppendLine("                </dft-validation>");
                sb.AppendLine("            </div>");
                sb.AppendLine("        </div>");

                if (index % 2 == 0 || index == columnList.Count)
                {
                    sb.AppendLine("    </div>");
                }

                index++;
            }

            sb.AppendLine("    </div>");
            sb.AppendLine("    <abp-modal-footer *ngIf=\"!isView\" [cancelDisabled]=\"saving\" (onCancelClick)=\"close()\">");
            sb.AppendLine("    </abp-modal-footer>");
            sb.AppendLine("</form>");
            CreateDirectory(path + createOrEditPath);
            File.WriteAllText(path + createOrEditPath + @"\" + createOrEditPath + ".component.html", sb.ToString());
            #endregion
        }
        #endregion

        #region Control Event
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
            GetButtonEnable();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                GetButtonEnable();
                cbbDb.DataSource = Database.GetDatabaseList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbbDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tableList = Database.ListTables(cbbDb.Text.Trim()).ToList();
            clbTable.Items.Clear();
            foreach (var item in tableList)
            {
                clbTable.Items.Add(item);
            }
        }

        private void clbTable_MouseUp(object sender, MouseEventArgs e)
        {
            GetButtonEnable();
            tableList = new List<string>();
            foreach (var item in clbTable.CheckedItems)
            {
                tableList.Add(item.ToString());
            }
        }

        private void txtPath_TextChanged_1(object sender, EventArgs e)
        {
            GetButtonEnable();
        }

        private void txtServiceBaseName_TextChanged(object sender, EventArgs e)
        {
            GetButtonEnable();
        }
        #endregion

        #region Button Enable
        private void GetButtonEnable()
        {
            GetGenerateEnable();
            GetServiceEnable();
        }

        private void GetGenerateEnable()
        {
            btnEntity.Enabled = btnClient.Enabled = clbTable.CheckedItems.Count > 0 && !string.IsNullOrEmpty(txtPath.Text.Trim());
        }

        private void GetServiceEnable()
        {
            btnService.Enabled = btnAll.Enabled = clbTable.CheckedItems.Count > 0
                && !string.IsNullOrEmpty(txtServiceBaseName.Text.Trim())
                && !string.IsNullOrEmpty(txtPath.Text.Trim());
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            Client_Click();
            Entity_Click();
            Service_Click();
            MessageBox.Show("Thành công");
        }
        #endregion

        private string GetFirstToLower(string keyWord)
        {
            return char.ToLower(keyWord[0]) + keyWord.Substring(1);
        }

        private string GetClientFileName(string tableName)
        {
            var split = Regex.Split(tableName, @"(?<!^)(?=[A-Z])").Select(e => e.ToLower()).ToList();
            return string.Join("-", split);
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
