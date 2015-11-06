using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace KickassTorrents
{
    public class KickassTorrentSearcher : IDisposable
    {
        private const string BaseUrl = "https://kat.ph/usearch/";
        private readonly dynamic _logger;

        public KickassTorrentSearcher(dynamic platform)
        {
            _logger = platform.Log;
        }

        // ReSharper disable once UnusedMember.Global
        public KickassTorrent[] Search(string query, int category, int sortBy, int sortOrder)
        {
            var url = BuildUrlString(query, category, sortBy, sortOrder);
            _logger.WriteLine("Looking up Kickass torrents URL: " + url);
            var xmlStream = PerformKickassRequest(url);
            var xmlSerializer = new XmlSerializer(typeof(KickassRssContainer));
            var torrents = (KickassRssContainer)xmlSerializer.Deserialize(xmlStream);
            xmlStream.Close();

            return torrents.Channel.Torrents;
        }

        private Stream PerformKickassRequest(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:34.0) Gecko/20100101 Firefox/34.0";
            webRequest.AllowAutoRedirect = true;
            webRequest.AutomaticDecompression = DecompressionMethods.Deflate |
                                                DecompressionMethods.GZip |
                                                DecompressionMethods.None;
            var stream = new MemoryStream();
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var responseStream = webResponse.GetResponseStream();
                if (responseStream == null)
                {
                    _logger.WriteLine("Response from kickass torrents server was null.");
                    return null;
                }
                var reader = new StreamReader(responseStream);
                var text = reader.ReadToEnd();
                _logger.WriteLine("---- Response ----\n" + text + "\n----   Halt   ----");
                reader.Close();
                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
            }
            stream.Position = 0;
            return stream;
        }

        private static string BuildUrlString(string query, int category, int sortBy,
            int sortOrder)
        {
            string sortOpt = null, sortOrd = null, cat;
            switch (sortBy)
            {
                case 0: sortOrd = "name"; break;
                case 1: sortOrd = "size"; break;
                case 2: sortOrd = "time_add"; break;
                case 3: sortOrd = "files_count"; break;
                case 4: sortOrd = "seeders"; break;
                case 5: sortOrd = "leechers"; break;
            }
            switch (sortOrder)
            {
                case 0:  sortOpt = "asc"; break;
                case 1:  sortOpt = "desc"; break;
            }
            switch (category)
            {
                default: cat = "all"; break;
                case 1: cat = "movie"; break;
                case 2: cat = "tv"; break;
                case 3: cat = "anime"; break;
                case 4: cat = "music"; break;
                case 5: cat = "book"; break;
                case 6: cat = "application"; break;
                case 7: cat = "game"; break;
                case 8: cat = "adult"; break;
            }
            return $"{BaseUrl}{query} category:{cat}/?field={sortOrd}&sorder={sortOpt}&rss=1";
        }

        public void Dispose()
        {
        }
    }
}
