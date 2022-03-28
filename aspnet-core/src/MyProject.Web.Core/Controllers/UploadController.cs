namespace MyProject.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MyProject.Global;

    [Route("api/[controller]/[action]")]
    public class UploadController : MyProjectControllerBase
    {
        private readonly IAppFolders appFolders;

        public UploadController(IAppFolders appFolders)
        {
            this.appFolders = appFolders;
        }

        [HttpPost]
        public async Task<List<string>> DemoUpload()
        {
            string fileFolderPath = Path.Combine(this.appFolders.DemoUploadFolder + @"\" + string.Format("{0:yyyyMMdd_hhmmss}", DateTime.Now));
            return await this.Upload(fileFolderPath);
        }

        private async Task<List<string>> Upload(string fileFolderPath)
        {
            List<string> result = new List<string>();

            // Nếu không có file
            if (this.Request.Form.Files == null || this.Request.Form.Files.Count <= 0)
            {
                return result;
            }

            foreach (var file in this.Request.Form.Files)
            {
                result.Add(GlobalFunction.SaveFile(fileFolderPath, file));
            }

            return await Task.FromResult(result);
        }
    }
}
