namespace DotNetUtils.Net
{
    /// <summary>
    /// Invoked whenever the state or progress of a download changes.
    /// </summary>
    /// <param name="fileDownloadProgress"></param>
    public delegate void FileDownloadProgressChangedHandler(FileDownloadProgress fileDownloadProgress);
}