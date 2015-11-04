﻿namespace PhotoBattles.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Controllers;
    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    [TestClass]
    public class ContestsControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMock();
        }

        [TestMethod]
        public void AddContestShouldInsertOneRecordToTheRepository()
        {
            var contests = new List<Contest>();

            var fakeUser = this.mocks.FakeUserRepository.Object.GetAll().FirstOrDefault();
            if (fakeUser == null)
            {
                Assert.Fail();
            }

            this.mocks.FakeContestRepository
                .Setup(r => r.Add(It.IsAny<Contest>()))
                .Callback(
                    (Contest contest) =>
                        {
                            contest.Organizer = fakeUser;
                            contests.Add(contest);
                        });

            var mockContext = new Mock<IPhotoBattlesData>();
            mockContext.Setup(c => c.Users)
                       .Returns(this.mocks.FakeUserRepository.Object);
            mockContext.Setup(c => c.Contests)
                       .Returns(this.mocks.FakeContestRepository.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId())
                          .Returns(fakeUser.Id);

            var contestController = new ContestsController(mockContext.Object, mockIdProvider.Object);

            var newContest = new ContestBindingModel()
            {
                Title = "Contest three",
                Description = "Contests three description",
                VotingStrategyEnum = VotingStrategyEnum.Open,
                ParticipationStrategyEnum = ParticipationStrategyEnum.Open,
                RewardStrategyEnum = RewardStrategyEnum.SingleWinner,
                DeadlineStrategyEnum = DeadlineStrategyEnum.ParticipantsLimit,
                ParticipantsLimit = 3
            };

            var response = contestController.AddContest(newContest);

            var expected = contests.FirstOrDefault(c => c.Title == newContest.Title);
            if (expected == null)
            {
                Assert.Fail();
            }

            mockContext.Verify(v => v.SaveChanges(), Times.Once);
            Assert.IsInstanceOfType(response, typeof(RedirectToRouteResult));
            Assert.AreEqual(1, contests.Count);
            Assert.AreEqual(newContest.Description, expected.Description);
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

            var contestController = new ContestsController(mockContext.Object, mockIdProvider.Object);

            fakeContest.Title = "updated contest title";
            fakeContest.Description = "updated contest des";
        }
    }
}