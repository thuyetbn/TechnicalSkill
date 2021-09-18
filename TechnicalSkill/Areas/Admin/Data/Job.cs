using Newtonsoft.Json;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using TechnicalSkill.BLL;
using TechnicalSkill.DAL;
using static TechnicalSkill.Models.RootModel;

namespace TechnicalSkill.Areas.Admin.Data
{
    public class Job : IJob
    {
        private readonly IRepository<Post> posts;
        private readonly IRepository<Category> categories;
        public Job()
        {
            categories = new Repository<Category>();
            posts = new Repository<Post>();
        }
        public Task Execute(IJobExecutionContext context)
        {
            
            var cat = categories.Get();
            foreach (var item1 in cat)
            {
                var listposts = ConvertRss(item1);
                posts.AddRange(listposts);
            }
            Console.WriteLine("Ok");
            return Task.CompletedTask;
        }
        public List<Post> ConvertRss(Category category)
        {
            var doc = new XmlDocument();
            doc.Load(category.LinkRSS);

            var json = JsonConvert.SerializeXmlNode(doc);
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(json);

            var ltposts = new List<Post>();

            myDeserializedClass.rss.channel.item.ForEach(x => 
            {
                var sourceID = GetSourceID(x.link);
                string[] arsourceID = posts.Get().OrderByDescending(y => y.SourceID).Select(y => y.SourceID).ToArray();

                if (arsourceID.Length == 0)
                {
                    goto add;
                }
                if (posts.CheckDuplicate(y => y.SourceID == sourceID))
                {
                    return;
                }
            //if (CheckBinary(arsourceID, sourceID, 0, arsourceID.Count() - 1))
            //{
            //    return;
            //}
            add:
                var p = new Post();
                StringWriter myWriter = new StringWriter();
                HttpUtility.HtmlDecode(x.title, myWriter);
                p.Title = myWriter.ToString();
                p.CategoryId = category.Id;
                p.Description = RemoveImage(x.description);
                StringWriter myWriter2 = new StringWriter();
                HttpUtility.HtmlDecode(GetContent(x.link), myWriter2);
                p.Content = myWriter2.ToString();
                p.Created_At = x.pubDate;
                p.SourceID = GetSourceID(x.link);
                ltposts.Add(p);
            });
            return ltposts;
        }

        public bool CheckBinary(long[] posts, long x, int minleght, int maxleght)
        {
            while (minleght <= maxleght)
            {
                int midleght = minleght + (maxleght - minleght) / 2;
                if (posts[midleght] == x)
                    return true;
                if (posts[midleght] > x)
                {
                    minleght = midleght + 1;
                }
                else
                {
                    maxleght = midleght - 1;
                }
            }
            return false;
        }
        public string GetSourceID(string text)
        {
            var result = Regex.Replace(text, "^https:\\/\\/vnexpress.net\\/(.*?)-([0-9]+).html$", "$2", RegexOptions.IgnorePatternWhitespace, TimeSpan.FromSeconds(.25));
            return result;
        }
        public string RemoveImage(Description text)
        {
            var result = Regex.Replace(text.CdataSection, "<[^>]*>", "", RegexOptions.IgnorePatternWhitespace, TimeSpan.FromSeconds(.25));

            result = Regex.Replace(result, @"^\s+$[\r\n]*", @"", RegexOptions.Multiline);
            return result;
        }
        public string GetContent(string link)
        {
            var client = new RestClient(link);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "device_env=4; device_env_real=4");
            IRestResponse response = client.Execute(request);
            var result = Regex.Match(response.Content, "<article class=\"fck_detail \">(.*)<\\/article>", RegexOptions.Singleline).Value;

            result = Regex.Replace(result, "<[^>]*>", "", RegexOptions.IgnorePatternWhitespace, TimeSpan.FromSeconds(.25));

            result = Regex.Replace(result, @"^\s+$[\r\n]*", @"", RegexOptions.Multiline);

            return result;
        }
    }
}