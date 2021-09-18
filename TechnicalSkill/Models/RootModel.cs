using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalSkill.Models
{
    public class RootModel
    {
        public class Xml
        {
            [JsonProperty("@version")]
            public string Version { get; set; }

            [JsonProperty("@encoding")]
            public string Encoding { get; set; }
        }

        public class Image
        {
            public string url { get; set; }
            public string title { get; set; }
            public string link { get; set; }
        }

        public class Description
        {
            [JsonProperty("#cdata-section")]
            public string CdataSection { get; set; }
        }

        public class Item
        {
            public string title { get; set; }
            public Description description { get; set; }
            public string pubDate { get; set; }
            public string link { get; set; }
            public string guid { get; set; }

            [JsonProperty("slash:comments")]
            public string SlashComments { get; set; }
        }

        public class Channel
        {
            public string title { get; set; }
            public string description { get; set; }
            public Image image { get; set; }
            public string pubDate { get; set; }
            public string generator { get; set; }
            public string link { get; set; }
            public List<Item> item { get; set; }
        }

        public class Rss
        {
            [JsonProperty("@version")]
            public string Version { get; set; }

            [JsonProperty("@xmlns:slash")]
            public string XmlnsSlash { get; set; }
            public Channel channel { get; set; }
        }

        public class Root
        {
            [JsonProperty("?xml")]
            public Xml Xml { get; set; }
            public Rss rss { get; set; }
        }

    }
}