using System;
using System.Threading.Tasks;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace SchoolEM.Tests.Services.StudentServiceTests
{
    public partial class EventListServiceTest
    {
        [Fact]
        public void ShouldThrowNotAuthorizedExceptionWhenUserIsNotInRole()
        {

            // given

            var notAuthorizedException = new NotAuthorizedException();
            var expectedException = notAuthorizedException;


            userClaimsMock.Setup(claim => claim.HaseClaimAndThrow("admin"))
                .Throws(notAuthorizedException);


            // when . then
            Assert.ThrowsAsync<NotAuthorizedException>(
                    async () =>
                    {
                       await this.eventListService.
                        EventlistAsync(new Evaluation.Brokers.Repositories.SortAndPagingParameters());
                    });
            Assert.ThrowsAsync<NotAuthorizedException>(
                   async () =>
                   {
                       await this.eventListService.OnGoingEventListAsync();
                   });

            Assert.ThrowsAsync<NotAuthorizedException>(
                  async () =>
                  {
                      await this.eventListService.RemoveEvent(new Random(20).Next());
                  });

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning(expectedException.Message),
                    Times.Exactly(3));

            this.eventRepositoryMock.VerifyNoOtherCalls();

        }
    }
}
