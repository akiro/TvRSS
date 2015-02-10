using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace TvRSS
{
    public class Writer
    {
        public void WriteMagnets(List<string> links)
        {
            var path = ConfigurationManager.AppSettings["torrentDir"];

            foreach (var link in links)
            {
                var m = Regex.Match(link, @"btih:([^&/]+)", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    var filepart = m.Groups[1].Value;
                    Console.WriteLine("Creating torrent file: " + filepart);
                    File.WriteAllText(path + "meta-" + filepart + ".torrent", "d10:magnet-uri" + link.Length + ":" + link + "e");
                }

            }

        }
    }
}
