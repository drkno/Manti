namespace _1337x
{
    public class X1337XTorrent
    {
        public uint Type { get; } = 0x1;
        public string Tag { get; }
        public string Title { get; }
        public string Uploader { get; }
        public ulong Size { get; }
        public uint Downloads { get; }
        public uint Age { get; }
        public string PageLink { get; }
        public int Category { get; }
        public bool Trusted { get; }
        public string Description { get; }
        public dynamic DownloadConfig { get; }

        public X1337XTorrent(string title, string url, uint seed, uint leech, ulong size, string uploader, string magnetLink, uint downloads, int category, uint age, string torrentFile, string id)
        {
            Title = title;
            Size = size;
            Uploader = uploader;
            Downloads = downloads;
            Category = category;
            Age = age;
            PageLink = url;
            Tag = id;
            Description = "";
            DownloadConfig = new
            {
                Guid = "",
                Magnet = magnetLink,
                FileName = torrentFile,
                Seeders = seed,
                Leechers = leech
            };
        }
    }
}
