using System.Data.Entity.Migrations;

namespace Xtrmstep.Extractor.Core.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ExtractorDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}