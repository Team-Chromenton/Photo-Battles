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

                var user = new User { Email = "admin@admin.com", UserName = "Administrator" };
                userManager.Create(user, "qwerty");

                var role = new IdentityRole { Name = "Admin" };
                context.Roles.Add(role);

                var userRole = new IdentityUserRole { RoleId = role.Id, UserId = user.Id };
                role.Users.Add(userRole);

                context.SaveChanges();
            }
        }
    }
}