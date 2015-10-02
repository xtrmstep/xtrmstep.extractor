using System;

namespace Xtrmstep.Extractor.Core.Model
{
    public class WebPage
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}