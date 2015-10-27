namespace PhotoBattles.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoBattles.Models;

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
                    var user = new User { Email = "user" + i + "@abv.bg", UserName = "User_" + i };
                    userManager.Create(user, "qwerty");

                    var identityUserRole = new IdentityUserRole { RoleId = userRole.Id, UserId = user.Id };
                    userRole.Users.Add(identityUserRole);
                }

                context.SaveChanges();
            }
        }
    }
}