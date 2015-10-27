namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;
    using PhotoBattles.Models.Strategies.VotingStrategies;

    [Authorize]
    public class ContestsController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            IQueryable<ContestViewModel> contests = this.Data.Contests
                    .GetAll()
                    .OrderByDescending(c => c.IsActive)
                    .ThenByDescending(c => c.IsOpen)
                    .ThenByDescending(c => c.CreatedOn)
                    .ProjectTo<ContestViewModel>();

            return this.View(contests);
        }

        public ActionResult AddContest()
        {
            return this.PartialView("~/Views/Contests/_AddContest.cshtml");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("~/Views/Contests/_AddContest.cshtml", model);
            }

            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var currentUser = this.Data.Users.GetAll().Where(u => u.Id == currentUserId).ProjectTo<UserViewModel>().FirstOrDefault();

            var newContest = new Contest
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsOpen = true,
                OrganizerId = currentUserId
            };

            this.Data.Contests.Add(newContest);

            // Voting Strategy
            if (model.VotingStartegy == VotingStrategy.Open)
            {
                newContest.VotingStrategy = VotingStrategy.Open;
            }
            else if (model.VotingStartegy == VotingStrategy.Closed)
            {
                newContest.VotingStrategy = VotingStrategy.Closed;

                string[] commiteeMembersNames = model.CommiteeMembers.Split(',').Select(cm => cm.Trim()).ToArray();
                var commiteeMembers = this.Data.Users.GetAll().Where(u => commiteeMembersNames.Contains(u.UserName)).Select(u => u.UserName).ToArray();

                string members = string.Join(", ", commiteeMembers);

                newContest.VotingStrategy = VotingStrategy.Closed;

                this.Data.Commitees.Add(new Commitee() { ContestId = newContest.Id, Members = members });
            }

            // Participation Strategy
            if (model.ParticipationStrategy == ParticipationStrategy.Closed)
            {
                string[] participantsNames = model.Participants.Split(',').Select(p => p.Trim()).ToArray();
                var participants = this.Data.Users
                        .GetAll()
                        .Where(u => participantsNames.Contains(u.UserName))
                        .ToList();

                newContest.Participants = participants;
                newContest.IsOpen = false;
            }

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Contests");
        }
    }
}