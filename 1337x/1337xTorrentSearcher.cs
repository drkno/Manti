using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace _1337x
{
    public class X1337XTorrentSearcher : IDisposable
    {
        private const string BaseUrl = "http://1337x.to/";

        private readonly dynamic _logger;
        public X1337XTorrentSearcher(dynamic platform)
        {
            _logger = platform.Log;
        }

        public IEnumerable<X1337XTorrent> Search(string query, int category, int sortBy, int sortOrder)
        {
            try
            {
                var url = BuildSearchUrl(query, category, sortBy, sortOrder, 1);
                _logger.WriteLine("URL = " + url);
                var searchResults = GetSearchResults(url);
                return searchResults.Select(GetTorrent);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        private class SearchResult
        {
            public string Title { get; private set; }
            public string Uploader { get; private set; }
            public string Url { get; private set; }
            public uint Seed { get; private set; }
            public uint Leech { get; private set; }
            public ulong Size { get; private set; }

            public SearchResult(string title, string url, uint seed, uint leech, ulong size, string uploader)
            {
                Title = title;
                Url = url;
                Seed = seed;
                Leech = leech;
                Size = size;
                Uploader = uploader;
            }
        }

        private string BuildSearchUrl(string query, int category, int sortBy, int sortOrder, int pageNum)
        {
            string sortUrl;
            switch (sortBy)
            {
                case 2: sortUrl = "{0}sort-search/{1}/time/{2}/{3}/"; break;
                case 5: sortUrl = "{0}sort-search/{1}/leechers/{2}/{3}/"; break;
                case 4: sortUrl = "{0}sort-search/{1}/seeders/{2}/{3}/"; break;
                case 1: sortUrl = "{0}sort-search/{1}/size/{2}/{3}/"; break;
                case 0: sortUrl = "{0}search/{1}/{3}/"; break;
                default: goto case  0;
            }

            string sortOrd;
            switch (sortOrder)
            {
                case 0: sortOrd = "asc"; break;
                case 1: sortOrd = "desc"; break;
                default: goto case 0;
            }

            query = query.Replace(' ', '+');
            return string.Format(sortUrl, BaseUrl, query, sortOrd, pageNum);
        }

        private X1337XTorrent GetTorrent(SearchResult result)
        {
            var htmlData = GetHtmlData(result.Url);

            var categoryStr = Regex.Match(htmlData, "(?<=(<strong>Category</strong>\\s*<span>))[^<]+(?=(</span>))").Value;
            var downloadsStr = Regex.Match(htmlData, "(?<=(<strong>Downloads</strong>\\s*<span>))[^<]+(?=(</span>))").Value;
            var dateUploaded = Regex.Match(htmlData, "(?<=(<strong>Date uploaded</strong>\\s*<span>))[^<]+(?=(</span>))").Value;
            var magnetLink = Regex.Match(htmlData, "(?<=(<a href=\"))magnet:\\?xt=[^\"]+(?=(\" onclick))").Value;
            var torrentLink = Regex.Match(htmlData, "(?<=(href=\"))http://torcache.net/torrent/[A-Z0-9]+.torrent(?=(\" onclick))").Value;
            var id = Regex.Match(torrentLink, "(?<=(href=\"http://torcache.net/torrent/))[A-Z0-9]+(?=(.torrent\" onclick))").Value;

            var ageSpl = dateUploaded.Split(' ');
            var age = uint.Parse(ageSpl[0]);
            switch (ageSpl[1])
            {
                case "years":
                case "year": age = (uint)(365.25 * age); goto case "month";
                case "months":
                case "month": age = (uint)(30.42 * age); goto case "day";
                case "days":
                case "day": age *= 24; goto case "hour";
                case "hours":
                case "hour": age *= 60; goto case "minutes";
                case "minutes":
                case "minute": age *= 60; break;
            }
            
            var category = 0;
            switch (categoryStr)
            {
                case "Anime": category = 3; break;
                case "XXX": category = 8; break;
                case "Apps": category = 6; break;
                case "E-Books": category = 5; break;
                case "Music": category = 4; break;
                case "Games": category = 7; break;
                case "Movies": category = 1; break;
                case "TV": category = 2; break;
            }

            return new X1337XTorrent(result.Title, result.Url, result.Seed, result.Leech, result.Size, result.Uploader, magnetLink, uint.Parse(downloadsStr), category, age, torrentLink, id);
        }

        private SearchResult[] GetSearchResults(string url)
        {
            var htmlData = GetHtmlData(url);
            var titleLinks = Regex.Matches(htmlData, "(?<=(<a href=\"/))torrent/[0-9]+/[^/]+/\">([^<]|(</?b>))+(?=(</a>))");
            var seeds = Regex.Matches(htmlData, "(?<=(<span class=\"green\">))[0-9]+(?=(</span></div>))");
            var leechers = Regex.Matches(htmlData, "(?<=(<span class=\"red\">))[0-9]+(?=(</span></div>))");
            var sizes = Regex.Matches(htmlData, "(?<=(coll-4\"><span>))[0-9.]+ [GgKkMmTt]?B(?=(</span>))");
            var uploaders = Regex.Matches(htmlData, "(?<=(/user/[^/]+/\">))[^<]+(?=(</a>))");

            var searchResults = new List<SearchResult>();
            for (var i = 0; i < titleLinks.Count; i++)
            {
                var tl = Regex.Replace(titleLinks[i].Value, "</?b>", "");
                var spl = tl.Split(new []{"\">"}, StringSplitOptions.RemoveEmptyEntries);

                var sizeSpl = sizes[i].Value.Split(' ');
                var size = double.Parse(sizeSpl[0]);
                switch (sizeSpl[1].ToLower().Trim())
                {
                    case "tb": size *= 1024; goto case "gb";
                    case "gb": size *= 1024; goto case "mb";
                    case "mb": size *= 1024; goto case "kb";
                    case "kb": size *= 1024; break;
                }

                searchResults.Add(new SearchResult(spl[1], BaseUrl + spl[0], uint.Parse(seeds[i].Value), uint.Parse(leechers[i].Value), (ulong)size, uploaders[i].Value));
            }
            return searchResults.ToArray();
        }

        private static string GetHtmlData(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowAutoRedirect = true;
            webRequest.AutomaticDecompression = DecompressionMethods.Deflate |
                                                DecompressionMethods.GZip |
                                                DecompressionMethods.None;
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:34.0) Gecko/20100101 Firefox/34.0";

            var html = "";
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var stream = webResponse.GetResponseStream();
                if (stream == null) return html;
                var streamReader = new StreamReader(stream);
                html = streamReader.ReadToEnd();
                streamReader.Close();
            }
            return html;
        }

        public void Dispose()
        {
        }
    }
}
