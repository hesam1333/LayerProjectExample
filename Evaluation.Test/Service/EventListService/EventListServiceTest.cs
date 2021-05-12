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
    public partial class EventListServiceTest
    {
        private readonly EventListService eventListService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;


        private readonly Mock<IEventRepository> eventRepositoryMock;

        private readonly Mock<IUserClaims> userClaimsMock;

        public EventListServiceTest()
        {
            this.userClaimsMock = new Mock<IUserClaims>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.eventRepositoryMock = new Mock<IEventRepository>();

            this.eventListService =
                new EventListService(this.loggingBrokerMock.Object,
                                     this.eventRepositoryMock.Object,
                                     this.userClaimsMock.Object);
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private SqlException GetSqlException() =>
            FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;


        private List<EventEvaluateeEvaluatorsViewTable> CreateRandomEvents()
        {
            int randomNumber = new IntRange(min: 1, max: 2).GetValue();

            var filler = new Filler<EventEvaluateeEvaluatorsViewTable>();
            filler.Setup()
                .ListItemCount(1);

            return filler.Create(randomNumber).ToList();
        }


        [Fact]
        public async Task ShouldRetrieveAllEventsBaseOnPagging()
        {
            // given
            List<EventEvaluateeEvaluatorsViewTable> storageEvents =
                CreateRandomEvents();

            List<EventListDto> expectedNotEvaluated =
                storageEvents.Select(i => new EventListDto()
                {
                    EndDate = i.DueDate,
                    StartDate = i.StartDate,
                    Id = i.Id,
                    EventTitle = i.EventTitle,
                    Status = eventListService.GetEventStatus(i.DueDate),
                    Evaluatees = string.Join(',', i.Evaluetees.Select(i => i.SureName)),
                    Evaluators = string.Join(',', i.Evaluators.Select(i => i.SureName))
                }).ToList(); 

           


            this.eventRepositoryMock.Setup(broker =>
                    broker.EventEvaluateeEvaluatorListAsync(null))
                        .ReturnsAsync(storageEvents);

            //when
            List<EventListDto> actual =
               await  this.eventListService.EventlistAsync(null);

            //then
            actual.Should().BeEquivalentTo(expectedNotEvaluated);

            this.eventRepositoryMock.Verify(broker =>
                broker.EventEvaluateeEvaluatorListAsync(null),
                    Times.Once);

            this.eventRepositoryMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }



        //[Fact]
        //public async Task ShouldRetrieveAllTopStars()
        //{
        //    // given
        //    List<UserEmploeeTotalPointViewTable> storageNotEvaluated =
        //        CreateRandomUserEmploeeTotalPointViewTable();

        //    List<TopStarUsersDto> expectedTopStars =
        //        storageNotEvaluated.Select(i => new TopStarUsersDto()
        //        {
        //            Score = i.TotalScore,
        //            Position = i.User.Position,
        //            SureName = i.User.SureName
        //        }).ToList(); ;



        //    this.evaluateeRepositoryMock.Setup(broker =>
        //            broker.GetUsersWithTopEvaluateeScoreAsync(14))
        //                .ReturnsAsync(storageNotEvaluated);

        //    //when
        //    List<TopStarUsersDto> actual =
        //       await this.dashboardService.GetTopStarAsyncs();

        //    //then
        //    actual.Should().BeEquivalentTo(expectedTopStars);

        //    this.evaluateeRepositoryMock.Verify(broker =>
        //        broker.GetUsersNotEvaluatedFromThisMonthAsync(14),
        //            Times.Once);

        //    this.evaluateeRepositoryMock.VerifyNoOtherCalls();
        //    this.eventRepositoryMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}


    }
}
