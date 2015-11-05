﻿namespace PhotoBattles.App.Controllers
{
    using System.Web.Mvc;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Hubs;
    using PhotoBattles.App.Infrastructure;
    using PhotoBattles.Data;
    using PhotoBattles.Data.Contracts;

    [ValidateInput(false)]
    public class BaseController : Controller
    {
        protected ContestInformationHub hub = new ContestInformationHub();

        public BaseController()
            : this(new PhotoBattlesData(), new AspNetUserIdProvider())
        {
        }

        public BaseController(IPhotoBattlesData data, IUserIdProvider userIdProvider)
        {
            this.Data = data;
            this.UserIdProvider = userIdProvider;
        }

        public IPhotoBattlesData Data { get; set; }

        public IUserIdProvider UserIdProvider { get; set; }
    }
}