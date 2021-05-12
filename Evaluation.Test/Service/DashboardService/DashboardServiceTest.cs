using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Brokers.Repositories;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Services;
using Evaluation.Services.DTO;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NUnit.Framework;
using Tynamix.ObjectFiller;
using Xunit;

namespace SchoolEM.Tests.Services.StudentServiceTests
{
    public partial class DashboardServiceTest
    {
        private readonly DashboardService dashboardService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;


        private readonly Mock<IEventRepository> eventRepositoryMock;
        private readonly Mock<IEvaluateeRepository> evaluateeRepositoryMock;

        private readonly Mock<IUserClaims> userClaimsMock;

        public DashboardServiceTest()
        {
            this.userClaimsMock = new Mock<IUserClaims>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.eventRepositoryMock = new Mock<IEventRepository>();
            this.evaluateeRepositoryMock = new Mock<IEvaluateeRepository>();

            this.dashboardService =
                new DashboardService(this.loggingBrokerMock.Object,
                                     this.userClaimsMock.Object,
                                     this.eventRepositoryMock.Object,
                                     this.evaluateeRepositoryMock.Object);
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private SqlException GetSqlException() =>
            FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

        private Entity CreateRandomEntity()
        {
            return new Filler<Entity>().Create();
        }

        private List<Event> CreateRandomEvents()
        {
            int randomNumber = new IntRange(min: 1, max: 2).GetValue();

            var filler = new Filler<Event>();
            filler.Setup()
                .ListItemCount(1);

            return filler.Create(randomNumber).ToList();
        }

        private List<UserEmploeeLastEventViewTable> CreateRandomUserEmploeeLastEventViewTable()
        {
            int randomNumber = new IntRange(min: 1, max: 2).GetValue();


            var filler = new Filler<UserEmploeeLastEventViewTable>();
            filler.Setup()
                .ListItemCount(1);

            return filler.Create(randomNumber).ToList();
        }

        private List<UserEmploeeTotalPointViewTable> CreateRandomUserEmploeeTotalPointViewTable()
        {
            int randomNumber = new IntRange(min: 1, max: 2).GetValue();


            var filler = new Filler<UserEmploeeTotalPointViewTable>();
            filler.Setup()
                .ListItemCount(1);

            return filler.Create(randomNumber).ToList();
        }

        private List<EventListForSearchDto> CreateRandomEventListForSearchDto()
        {
            int randomNumber = new IntRange(min: 1, max: 2).GetValue();


            var filler = new Filler<EventListForSearchDto>();
            filler.Setup()
                .ListItemCount(1);

            return filler.Create(randomNumber).ToList();
        }

        [Fact]
        public async Task ShouldRetrieveAllUserNotEvaluated()
        {
            // given
            List<UserEmploeeLastEventViewTable> storageNotEvaluated =
                CreateRandomUserEmploeeLastEventViewTable();

            List<UserNotEvaluatedDto> expectedNotEvaluated =
                storageNotEvaluated.Select(i => new UserNotEvaluatedDto()
                {
                    LastEvaluationDateTime = i.LastEventAsEvaluatee.DueDate,
                    Position = i.User.Position,
                    SureName = i.User.SureName
                }).ToList(); ;



            this.evaluateeRepositoryMock.Setup(broker =>
                    broker.GetUsersNotEvaluatedFromThisMonthAsync(6))
                        .ReturnsAsync(storageNotEvaluated);

            //when
            List<UserNotEvaluatedDto> actual =
               await  this.dashboardService.GetUsersNotEvaluatedAsync();

            //then
            actual.Should().BeEquivalentTo(expectedNotEvaluated);

            this.evaluateeRepositoryMock.Verify(broker =>
                broker.GetUsersNotEvaluatedFromThisMonthAsync(6),
                    Times.Once);

            this.evaluateeRepositoryMock.VerifyNoOtherCalls();
            this.eventRepositoryMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }



        [Fact]
        public async Task ShouldRetrieveAllTopStars()
        {
            // given
            List<UserEmploeeTotalPointViewTable> storageNotEvaluated =
                CreateRandomUserEmploeeTotalPointViewTable();

            List<TopStarUsersDto> expectedTopStars =
                storageNotEvaluated.Select(i => new TopStarUsersDto()
                {
                    Score = i.TotalScore,
                    Position = i.User.Position,
                    SureName = i.User.SureName
                }).ToList(); ;



            this.evaluateeRepositoryMock.Setup(broker =>
                    broker.GetUsersWithTopEvaluateeScoreAsync(12))
                        .ReturnsAsync(storageNotEvaluated);

            //when
            List<TopStarUsersDto> actual =
               await this.dashboardService.GetTopStarAsyncs();

            //then
            actual.Should().BeEquivalentTo(expectedTopStars);

            this.evaluateeRepositoryMock.Verify(broker =>
                broker.GetUsersWithTopEvaluateeScoreAsync(12),
                    Times.Once);

            this.evaluateeRepositoryMock.VerifyNoOtherCalls();
            this.eventRepositoryMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        //[Fact]
        //public  async Task ShouldRetrieveChartData()
        //{
        //    // given

        //    DateTime today = DateTime.Now.Date;

        //    List<Event> storageEvents =
        //        CreateRandomEvents();

        //    DashboardChartDto expectedChart = new DashboardChartDto()
        //    {

        //        CompletedEvents = storageEvents.Count(i => i.DueDate < today),
        //        OnGoingEvents = storageEvents.Count(i => i.DueDate > today && i.StartDate < today),
        //        NotStartedEvents = storageEvents.Count(i => i.StartDate > today),
        //        TotalEvents = storageEvents.Count()
        //    };

        //    this.eventRepositoryMock.Setup(broker =>
        //            broker.FindAsync(i => i.StartDate.Year == today.Year))
        //                .ReturnsAsync(storageEvents);

        //    //when
        //    DashboardChartDto actual =
        //       await this.dashboardService.DashBoardChartValuesAsync();

        //    //then
        //    actual.Should().BeEquivalentTo(expectedChart);


        //    this.evaluateeRepositoryMock.VerifyNoOtherCalls();
        //    this.eventRepositoryMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}



        //[Fact]
        //public async Task ShouldSearchEvents()
        //{
        //    // given

        //    DateTime today = DateTime.Now.Date;

        //    List<Event> storageEvents =
        //        CreateRandomEvents();

        //    List<EventListForSearchDto> expectedEvents = storageEvents.Select(i => new EventListForSearchDto()
        //    {
        //        DueDate = i.DueDate,
        //        EntryDatetime = i.EntryDatetime,
        //        EventTitle = i.EventTitle,
        //        StartDate = i.StartDate
        //    }).ToList(); ;

        //    this.eventRepositoryMock.Setup(
        //            c => c.FindAsync(It.Is<Expression<Func<Event, bool>>>(criteria => true)))
        //                .ReturnsAsync(storageEvents);



        //    //when
        //    var actual =
        //       await this.dashboardService.SearchEventsAsync(today.ToString());

        //    //then
        //    actual.Should().BeEquivalentTo(expectedEvents);

        //    this.evaluateeRepositoryMock.VerifyNoOtherCalls();
        //    this.eventRepositoryMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}
    }
}
