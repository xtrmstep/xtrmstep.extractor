using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xtrmstep.Extractor.Core;
using Xtrmstep.Extractor.Core.Model;
using Xtrmstep.Extractor.Properties;

namespace Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = CreateIoC();

            var fileReader = container.Resolve<FileReader>();
            var files = fileReader.LookupFiles(Settings.Default.CrawlerData);

            var extractor = container.Resolve<DataExtractor>();
            foreach (var fileName in files)
            {
                var data = fileReader.Read(fileName);
                extractor.Save(data);
            }
        }

        private static IContainer CreateIoC()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExtractorDbContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.Register(c => (ExtractorDbContext)c.Resolve<IDbContext>()).As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IDbContext>().Pages).As<IDbSet<WebPage>>().InstancePerLifetimeScope();

            builder.RegisterType<FileReader>().InstancePerLifetimeScope();
            builder.RegisterType<DataExtractor>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
