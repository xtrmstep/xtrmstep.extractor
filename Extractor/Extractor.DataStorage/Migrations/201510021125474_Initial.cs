namespace Xtrmstep.Extractor.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebPages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Url = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebPages");
        }
    }
}
