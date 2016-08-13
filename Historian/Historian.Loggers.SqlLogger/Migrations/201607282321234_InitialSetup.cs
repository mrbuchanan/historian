namespace Historian.Loggers.SqlLogger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChannelGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ChannelGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChannelGroups", t => t.ChannelGroupId, cascadeDelete: true)
                .Index(t => t.ChannelGroupId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contents = c.String(),
                        Kind = c.Int(nullable: false),
                        ChannelId = c.Int(nullable: false),
                        Title = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.ChannelId, cascadeDelete: true)
                .Index(t => t.ChannelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Channels", "ChannelGroupId", "dbo.ChannelGroups");
            DropIndex("dbo.Messages", new[] { "ChannelId" });
            DropIndex("dbo.Channels", new[] { "ChannelGroupId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Channels");
            DropTable("dbo.ChannelGroups");
        }
    }
}
