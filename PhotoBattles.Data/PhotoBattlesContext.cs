namespace PhotoBattles.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;

    public class PhotoBattlesContext : IdentityDbContext<User>, IPhotoBattlesContext
    {
        public PhotoBattlesContext()
            : base("name=PhotoBattlesContext")
        {
        }

        public virtual IDbSet<Contest> Contests { get; set; }

        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Vote> Votes { get; set; }

        public static PhotoBattlesContext Create()
        {
            return new PhotoBattlesContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>()
                        .HasMany(u => u.OwnContests)
                        .WithRequired(c => c.Organizer);

            modelBuilder.Entity<Contest>()
                        .HasMany(c => c.Participants)
                        .WithMany(u => u.ParticipatingContest)
                        .Map(
                            m =>
                                {
                                    m.MapLeftKey("ContestId");
                                    m.MapRightKey("UserId");
                                    m.ToTable("ParticipatingContests");
                                });

            //modelBuilder.Entity<Contest>()
            //            .HasMany(c => c.AllowedForParticipation)
            //            .WithMany(u => u.AllowedForParticipation)
            //            .Map(
            //                m =>
            //                    {
            //                        m.MapLeftKey("ContestId");
            //                        m.MapRightKey("UserId");
            //                        m.ToTable("ParticipationContest");
            //                    });

            //modelBuilder.Entity<Contest>()
            //            .HasMany(c => c.AllowedForVoting)
            //            .WithMany(u => u.AllowedForVoting)
            //            .Map(
            //                m =>
            //                    {
            //                        m.MapLeftKey("ContestId");
            //                        m.MapRightKey("UserId");
            //                        m.ToTable("VoteContest");
            //                    });

            modelBuilder.Entity<Contest>()
                        .HasMany(c => c.Winners)
                        .WithMany(u => u.WinContests)
                        .Map(
                            m =>
                                {
                                    m.MapLeftKey("ContestId");
                                    m.MapRightKey("UserId");
                                    m.ToTable("WinerContest");
                                });

            base.OnModelCreating(modelBuilder);
        }
    }
}