namespace PhotoBattles.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using PhotoBattles.App.Areas.Admin.Controllers;
    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Controllers;
    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    [TestClass]
    public class AdminContestsControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMock();
        }

        [TestMethod]
        public void EditContestShouldUpdateTheRecordCorrectly()
        {
            var fakeContest = this.mocks.FakeContestRepository.Object.GetAll().FirstOrDefault();
            if (fakeContest == null)
            {
                Assert.Fail();
            }

            var fakeUser = this.mocks.FakeUserRepository.Object.GetAll().FirstOrDefault();
            if (fakeUser == null)
            {
                Assert.Fail();
            }

            var mockContext = new Mock<IPhotoBattlesData>();
            mockContext.Setup(c => c.Users)
                       .Returns(this.mocks.FakeUserRepository.Object);
            mockContext.Setup(c => c.Contests)
                       .Returns(this.mocks.FakeContestRepository.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId())
                          .Returns(fakeUser.Id);

            var contestController = new AdminContestsController(mockContext.Object, mockIdProvider.Object);

            var editContest = new ContestBindingModel()
            {
                Id = 1,
                Title = "Edited contest title",
                Description = "Edited contest description",
                VotingStrategyEnum = VotingStrategyEnum.Open,
                ParticipationStrategyEnum = ParticipationStrategyEnum.Open,
                RewardStrategyEnum = RewardStrategyEnum.SingleWinner,
                DeadlineStrategyEnum = DeadlineStrategyEnum.ParticipantsLimit,
                ParticipantsLimit = 3
            };

            var response = contestController.AdminEditContest(editContest);

            var result = this.mocks.FakeContestRepository.Object.GetAll().FirstOrDefault();
            if (result == null)
            {
                Assert.Fail();
            }

            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.IsInstanceOfType(response, typeof(RedirectToRouteResult));
            Assert.AreEqual(editContest.Title, result.Title);
            Assert.AreEqual(editContest.Description, result.Description);
        }
    }
}