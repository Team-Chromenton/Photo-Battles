namespace PhotoBattles.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Data.Migrations;
    using PhotoBattles.Models;

    public class PhotoBattlesContext : IdentityDbContext<User>, IPhotoBattlesContext
    {
        public PhotoBattlesContext()
            : base("name=PhotoBattlesContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoBattlesContext, Configuration>());
        }

        public virtual IDbSet<Contest> Contests { get; set; }

        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}