namespace PhotoBattles.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    public sealed class Configuration : DbMigrationsConfiguration<PhotoBattlesContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PhotoBattlesContext context)
        {
            if (!context.Users.Any(u => u.UserName == "Administrator"))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);

                var admin = new User { Email = "admin@admin.com", UserName = "Administrator" };
                userManager.Create(admin, "qwerty");

                var adminRole = new IdentityRole { Name = "Admin" };
                context.Roles.Add(adminRole);

                var identityRole = new IdentityUserRole { RoleId = adminRole.Id, UserId = admin.Id };
                adminRole.Users.Add(identityRole);

                var userRole = new IdentityRole { Name = "User" };
                context.Roles.Add(userRole);

                for (int i = 1; i < 7; i++)
                {
                    var user = new User { Email = "user" + i + "@abv.bg", UserName = "user" + i };
                    userManager.Create(user, "qwerty");

                    var identityUserRole = new IdentityUserRole { RoleId = userRole.Id, UserId = user.Id };
                    userRole.Users.Add(identityUserRole);
                }
            }

            if (!context.Contests.Any())
            {
                this.SeedContests(context);
            }

            context.SaveChanges();
        }

        private void SeedContests(PhotoBattlesContext dbo)
        {
            for (int i = 1; i < 25; i++)
            {
                var contest = new Contest
                {
                    Title = "Contest " + i,
                    Description = "This is sample contest No. " + i + ".",
                    CreatedOn = DateTime.Now.AddDays(i * -1),
                    IsOpen = true,
                    OrganizerId = dbo.Users.OrderBy(r => Guid.NewGuid()).Select(u => u.Id).FirstOrDefault(),
                    NumberOfWinners = 1
                };

                if (i % 2 == 0)
                {
                    contest.DeadlineStrategy = DeadlineStrategy.EndDate;
                    contest.EndDate = contest.CreatedOn.AddDays(14);
                }
                else
                {
                    contest.DeadlineStrategy = DeadlineStrategy.ParticipantsLimit;
                    contest.ParticipantsLimit = i + 5;
                }

                dbo.Contests.Add(contest);
            }
        }
    }
}