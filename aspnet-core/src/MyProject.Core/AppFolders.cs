namespace MyProject
{
    using Abp.Dependency;

    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }

        public string DemoUploadFolder { get; set; }

        public string DemoFileDownloadFolder { get; set; }
    }
}