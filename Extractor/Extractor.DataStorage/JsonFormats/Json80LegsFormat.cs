using System;
using System.Collections.Specialized;

namespace Xtrmstep.Extractor.Core.JsonFormats
{
    public class Json80LegsFormat
    {
        public string url
        {
            get;
            set;
        }

        public string result
        {
            get;
            set;
        }

        public static Func<NameValueCollection, Json80LegsFormat> Converter = entry =>
            new Json80LegsFormat
            {
                url = entry["url"],
                result = entry["result"]
            };
    }
}