namespace MyProject
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }

        string DemoUploadFolder { get; }

        string DemoFileDownloadFolder { get; }
    }
}