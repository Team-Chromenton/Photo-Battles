namespace PhotoBattles.App.Infrastructure
{
    using System.Threading;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Contracts;

    public class AspNetUserIdProvider : IUserIdProvider
    {
        public string GetUserId()
        {
            return Thread.CurrentPrincipal.Identity.GetUserId();
        }

        public string GetUserName()
        {
            return Thread.CurrentPrincipal.Identity.GetUserName();
        }
    }
}