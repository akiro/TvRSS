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
    class KickAssParser : IParser
    {
        private List<string> Shows { get; set; }

        public KickAssParser()
        {
            Shows = new List<string>
            {
                "Top.Gear.*720p",
                "Mythbusters.*720p",
                "Game.Of.Thrones.*720p",
                "Bobs.Burgers.*720p",
                "Mad.Men.*720p",
                "Walking.Dead.*720p",
                "Modern.Family.*720p",
                "True.Detective.*720p",
                "House.*Lies.*720p",
            };
        }

        public List<string> Parse()
        {
            var timestampFile = ConfigurationManager.AppSettings["timestampFile"];

            var timestamp = DateTime.Now - new TimeSpan(2,0,0,0);
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
                var magnet = item.ElementExtensions.ReadElementExtensions<string>("magnetURI", "http://xmlns.ezrss.it/0.1/");
                var title = item.Title.Text;

                var hit = false;
                foreach(var show in Shows){
                    if (Regex.IsMatch(title, show, RegexOptions.IgnoreCase) &&
                        item.PublishDate > timestamp)
                    {
                        Console.WriteLine("Found match: " + title);
                        hits.Add(magnet.First().ToString());
                        hit = true;
                        break;
                    }
                }

                if(!hit) Console.WriteLine("Miss: " + title);
            }

            File.WriteAllText(timestampFile, DateTime.Now.ToString());

            return hits;
        }
    }
}
