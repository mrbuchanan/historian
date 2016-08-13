namespace Historian.Loggers.SqlLogger.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Historian.Loggers.SqlLogger.Entities.SqlLoggerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Historian.Loggers.SqlLogger.Entities.SqlLoggerContext context)
        {
            //  This method will be called after migrating to the latest version.

            var historianGroup = new Entities.ChannelGroup { Name = "Historian" };
            var unknownGroup = new Entities.ChannelGroup { Name = "Unknown" };

            context.ChannelGroups.AddOrUpdate(historianGroup, unknownGroup);

            context.Channels.AddOrUpdate(new Entities.Channel { Name = "historian.errors", ChannelGroup = historianGroup });
        }
    }
}
