using System.Collections.Generic;
using System.Data.Entity;
using Autofac;
using Xtrmstep.Extractor.Core;
using Xtrmstep.Extractor.Core.JsonFormats;
using Xtrmstep.Extractor.Core.Model;
using Xtrmstep.Extractor.Properties;

namespace Extractor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IContainer container = CreateIoC();

            JsonFileReader fileReader = container.Resolve<JsonFileReader>();
            string[] files = fileReader.LookupFiles(Settings.Default.CrawlerData);

            DataExtractor extractor = container.Resolve<DataExtractor>();
            foreach (string fileName in files)
            {
                IEnumerable<Json80LegsFormat> data = fileReader.Read(fileName, Json80LegsFormat.Converter);
                extractor.Save(data);
            }
        }

        private static IContainer CreateIoC()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExtractorDbContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.Register(c => (ExtractorDbContext) c.Resolve<IDbContext>()).As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IDbContext>().Pages).As<IDbSet<WebPage>>().InstancePerLifetimeScope();

            builder.RegisterType<JsonFileReader>().InstancePerLifetimeScope();
            builder.RegisterType<DataExtractor>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}