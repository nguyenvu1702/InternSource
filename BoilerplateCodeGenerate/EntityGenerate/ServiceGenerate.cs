using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EntityGenerate
{
    public partial class fServiceGenerate : Form
    {
        public fServiceGenerate()
        {
            InitializeComponent();
        }

        public string serviceNames = string.Empty;

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            string serviceNames = txtServiceNames.Text.Trim();
            string path = txtPath.Text.Trim();
            string serviceBaseName = txtServiceBaseName.Text.Trim();

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(serviceNames) && !string.IsNullOrEmpty(path))
            {
                var nameSpace = path.Split('\\').Where(w => w != "").Last();
                var listServiceName = serviceNames.Split(',').Where(w => w != "").ToList();

                // Tạo thư mục chứa các service
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Tạo thư mục chứa service nhỏ
                foreach (var item in listServiceName)
                {
                    sb = new StringBuilder();
                    string servicePath = path + @"\" + item;
                    if (!Directory.Exists(servicePath))
                    {
                        Directory.CreateDirectory(servicePath);
                    }

                    if (!Directory.Exists(servicePath + @"\Dtos"))
                    {
                        Directory.CreateDirectory(servicePath + @"\Dtos");
                    }

                    string interfaceName = "I" + item + "AppService";
                    string itfaceFile = servicePath + @"\" + interfaceName + ".cs";

                    sb.AppendLine(string.Format("namespace {0}.{1}", serviceBaseName, item));
                    sb.AppendLine("{");
                    sb.AppendLine("    using System.Threading.Tasks;");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("    public interface {0}", interfaceName));
                    sb.AppendLine("    {");
                    sb.AppendLine("        Task GetBaoCao();");
                    sb.AppendLine("    }");
                    sb.AppendLine("}");

                    File.WriteAllText(itfaceFile, sb.ToString());

                    string serviceName = item + "AppService";
                    sb = new StringBuilder();

                    sb.AppendLine(string.Format("namespace {0}.{1}", serviceBaseName, item));
                    sb.AppendLine("{");
                    sb.AppendLine("    using System;");
                    sb.AppendLine("    using System.Threading.Tasks;");
                    sb.AppendLine(string.Empty);
                    sb.AppendLine(string.Format("    public class {0} : {1}, {2}", serviceName, serviceBaseName + "AppServiceBase", interfaceName));
                    sb.AppendLine("    {");
                    sb.AppendLine(string.Format("        public {0}()", serviceName));
                    sb.AppendLine("        {");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    sb.AppendLine("        public Task GetBaoCao()");
                    sb.AppendLine("        {");
                    sb.AppendLine("            throw new NotImplementedException();");
                    sb.AppendLine("        }");
                    sb.AppendLine("    }");
                    sb.AppendLine("}");

                    string serviceFile = servicePath + @"\" + item + "AppService.cs";
                    File.WriteAllText(serviceFile, sb.ToString());
                }
                MessageBox.Show("Thành công.");
            }
        }

        private void fServiceGenerate_Load(object sender, EventArgs e)
        {
            this.txtServiceNames.Text = serviceNames.Trim();
        }
    }
}
