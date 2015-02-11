using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvRSS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting..");

            var p = new Parser();
            var shows = p.Parse();

            var w = new Writer();
            w.WriteMagnets(shows);
        }
    }
}
