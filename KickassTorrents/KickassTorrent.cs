using System;
using System.Threading;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace KickassTorrents
{
    [XmlType(AnonymousType = true, Namespace = "http://xmlns.ezrss.it/0.1/"), XmlRoot("item")]
    public class KickassTorrent
    {
        public uint Type { get; } = 0x1;

        private string Description { get; }

        public dynamic DownloadConfig { get; } = new {
            Guid = "",
            Magnet = "",
            FileName = ""
        };


        [XmlElement("guid", Form = XmlSchemaForm.Unqualified)]
        public string KickassGuid { set { DownloadConfig.Guid = value; } }

        [XmlElement("magnetURI")]
        public string KickassMagnetUri { set { DownloadConfig.Magnet = value; } }

        [XmlElement("fileName")]
        public string KickassFileName { set { DownloadConfig.FileName = value; } }

        public uint Age { get; set; }

        [XmlElement("title", Form = XmlSchemaForm.Unqualified)]
        public string Title { set; get; }

        int Category { get; set; }
        [XmlElement("category", Form = XmlSchemaForm.Unqualified)]
        public string KickassCategory
        {
            set
            {
                var cultureInfo = Thread.CurrentThread.CurrentCulture;
                var textInfo = cultureInfo.TextInfo;
                value = textInfo.ToTitleCase(value.ToLower());
                Console.WriteLine("cat=" + value);
            }
        }

        [XmlElement("author", Form = XmlSchemaForm.Unqualified)]
        public string Uploader { set; get; }

        [XmlElement("link", Form = XmlSchemaForm.Unqualified)]
        public string PageLink { set; get; }

        [XmlElement("pubDate", Form = XmlSchemaForm.Unqualified)]
        public string KickassPublicationDate
        {
            set
            {
                DateTime result;
                if (DateTime.TryParse(value, out result))
                {
                    Age = (uint)(DateTime.Now - result).TotalSeconds;
                }
            }
        }

        [XmlElement("contentLength")]
        public ulong Size { set; get; }

        [XmlElement("infoHash")]
        public string Tag { set; get; }

        [XmlElement("seeds")]
        public uint Downloads { set; get; }

        [XmlElement("verified")]
        public bool Trusted { set; get; }

        [XmlElement("enclosure", Form = XmlSchemaForm.Unqualified)]
        public KickassTorrentEnclosure KickassEnclosure { get; set; }
    }
}
