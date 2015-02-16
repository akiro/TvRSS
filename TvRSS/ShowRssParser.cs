using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TvRSS
{
    class ShowRssParser : IParser
    {

        public List<string> Parse()
        {
            var timestampFile = ConfigurationManager.AppSettings["timestampFile"];

            var timestamp = DateTime.Now - new TimeSpan(2, 0, 0, 0);
            if (File.Exists(timestampFile))
            {
                var foo = File.ReadAllText(timestampFile);
                timestamp = DateTime.Parse(foo);
            }

            var url = ConfigurationManager.AppSettings["rssUrl"];

            var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            var hits = new List<string>();
            foreach (var item in feed.Items)
            {
                var magnet = item.Links.SingleOrDefault(x => x.RelationshipType == "enclosure").Uri.ToString();
                var title = item.Title.Text;
                hits.Add(magnet);
            }

            File.WriteAllText(timestampFile, DateTime.Now.ToString());

            return hits;
        }
    }
}
