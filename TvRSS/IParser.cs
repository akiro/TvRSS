using System;
using System.Collections.Generic;

namespace TvRSS
{
    interface IParser
    {
        List<string> Parse();
    }
}
