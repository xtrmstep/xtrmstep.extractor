using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using log4net;
using Xtrmstep.Extractor.Core.JsonFormats;
using Xtrmstep.Extractor.Core.Model;

namespace Xtrmstep.Extractor.Core
{
    public class DataExtractor
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDbSet<WebPage> dbPages;
        private readonly IUnitOfWork unitOfWork;

        public DataExtractor(IDbSet<WebPage> pages, IUnitOfWork unitOfWork)
        {
            dbPages = pages;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<WebPage> Read(NameValueCollection data)
        {
            List<WebPage> result = new List<WebPage>();
            foreach (string url in data)
            {
                string content = data[url];
                result.Add(new WebPage
                {
                    Url = url,
                    Content = content
                });
            }
            return result;
        }

        public void Save(IEnumerable<Json80LegsFormat> data)
        {
            foreach (var item in data)
            {
                var webPage = new WebPage
                {
                    Url = item.url,
                    Content = item.result
                };
                Save(webPage);
            }
        }

        public void Save(WebPage page)
        {
            // update exiting and store new pages
            try
            {
                WebPage existingPage = dbPages.SingleOrDefault(p => p.Url == page.Url);
                if (existingPage != null)
                {
                    existingPage.Content = page.Content;
                }
                else
                {
                    page.Id = Guid.NewGuid();
                    dbPages.Add(page);
                }
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                log.Error(e);
            }
        }
    }
}