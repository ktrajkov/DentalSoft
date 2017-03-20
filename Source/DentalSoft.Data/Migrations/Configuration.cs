namespace DentalSoft.Data.Migrations
{
    using DentalSoft.Common;
    using DentalSoft.Data.Models.Dentists;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            // TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        private void SeedRegions(ApplicationDbContext context)
        {
            if (context.Regions.Any() || context.Municipalityes.Any() || context.Locations.Any())
            {
                return;
            }
            var regions = RegionsGenerator.Regions();
            foreach (var region in regions)
            {
                context.Regions.Add(region);
                context.SaveChanges();
            }
        }
    }
}
