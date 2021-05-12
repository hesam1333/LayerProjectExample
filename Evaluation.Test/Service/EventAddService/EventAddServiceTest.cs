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
    public partial class EventAddServiceTest
    {
        private readonly EventAddService eventAddService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;


        private readonly Mock<IEventRepository> eventRepositoryMock;
        private readonly Mock<IEvaluateeRepository> evaluateeRepositoryMock;
        private readonly Mock<IEvaluatorRepository> evaluatorRepositoryMock;
        private readonly Mock<IUserRepository> UserRepositoryMock;

        private readonly Mock<IUserClaims> userClaimsMock;

        public EventAddServiceTest()
        {
            this.userClaimsMock = new Mock<IUserClaims>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.eventRepositoryMock = new Mock<IEventRepository>();
            this.evaluateeRepositoryMock = new Mock<IEvaluateeRepository>();
            this.evaluatorRepositoryMock = new Mock<IEvaluatorRepository>();

            this.eventAddService =
                new EventAddService(this.loggingBrokerMock.Object,
                                     this.eventRepositoryMock.Object,
                                     this.userClaimsMock.Object);
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }



        private List<EventEvaluateeEvaluatorsViewTable> CreateRandomEvents()
        {
            int randomNumber = new IntRange(min: 1, max: 2).GetValue();

            var filler = new Filler<EventEvaluateeEvaluatorsViewTable>();
            filler.Setup()
                .ListItemCount(1);

            return filler.Create(randomNumber).ToList();
        }

        private Event CreateEvent()
        {
            return new Filler<Event>().Create();
        }


        [Fact]
        public async Task<Event> ShouldAddAndReturnAnEventWhitEvaluatorAndEvaluatees()
        {
            // given

            EventAddStage1DTO eventAddFirstPageDTO = new
                Filler<EventAddStage1DTO>().Create();
            int EvaluatorEditCalls = eventAddFirstPageDTO.evaluatorDtos.Count(i => i.UserId != null);
            int EvaluatorAddCalls = eventAddFirstPageDTO.evaluatorDtos.Count(i => i.UserId == null);


            int EvaluateeEditCalls = eventAddFirstPageDTO.evaluateeDtos.Count(i => i.UserId != null);
            int EvaluateeAddCalls = eventAddFirstPageDTO.evaluateeDtos.Count(i => i.UserId == null);

            Event storageEvents = CreateEvent();
            storageEvents.EventTitle = eventAddFirstPageDTO.EventTitle;
            Event expectedNotEvaluated = storageEvents;

            this.eventRepositoryMock.Setup(broker =>
                    broker.AddAsync(storageEvents))
                        .ReturnsAsync(storageEvents);

            //when
            List<EventListDto> actual =
               await this.eventAddService.CreatEvetEvaluatorAndEval(null);

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
