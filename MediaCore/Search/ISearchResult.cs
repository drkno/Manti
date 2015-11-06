namespace MediaCore.Search
{
    // ReSharper disable UnusedMember.Global
    public interface ISearchResult
    {
        uint Type { get; }
        string Tag { get; }
        string Title { get; }
        string Description { get; }
        string Uploader { get; }
        SearchCategory Category { get; }
        ulong Size { get; }
        uint Downloads { get; }
        uint Age { get; }
        bool Trusted { get; }
        string PageLink { get; }
        object DownloadConfig { get; }
    }
}
