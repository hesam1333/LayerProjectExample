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
    public partial class AplicationServiceTests
    {
        [Fact]
        public void shouldThrowInvalidExceptionIfValidationErorOccredAsync()
        {

            // given

            var validationException = new ModelValidationException(new Exception() , new ValidationError());
            var expectedException = validationException;


            Task result() => this.aplicationService.TryCatchAsync<Entity>(
                              () => throw validationException).AsTask();

            // when . then
            Assert.ThrowsAsync<ModelValidationException>(result);



            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning(validationException.Message),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();

        }


        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveIfDatabaseUpdateExceptionThrownAndLogItAsync()
        {
            // given

            var databaseUpdateException = new DbUpdateException();
            var expectedStudentDependencyException = new DependencyException(databaseUpdateException);


            Task result() => this.aplicationService.TryCatchAsync<Entity>(
                              () => throw databaseUpdateException).AsTask();

            // when . then
            Assert.ThrowsAsync<DependencyException>(result);



            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowAplicationServiceExceptionnOnRetrieveIfGenericExceptionThrownAndLogItAsync()
        {
            //given

            var serviceException = new Exception();
            var expectedServiceException = new ServiceException(serviceException);

            Task result() => this.aplicationService.TryCatchAsync<Entity>(
                              () => throw serviceException).AsTask();

            // when . then
            Assert.ThrowsAsync<ServiceException>(result);


            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowAplicationServiceExceptionnOnRetrieveIfGenericExceptionThrownAndLogIt()
        {
            //given
            Guid someId = Guid.NewGuid();

            var serviceException = new Exception();
            var expectedServiceException = new ServiceException(serviceException);


            // when . then
            Assert.Throws<ServiceException>(() => {
                this.aplicationService.TryCatch<Entity>(
                              () => throw serviceException);
            });


            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public  void ShouldThrowCriticalDependencyExceptionOnRegisterIfSqlExceptionWasThrownAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();
            var expectedDependencyException = new DependencyException(sqlException);

            Task result() => this.aplicationService.TryCatchAsync<Entity>(
                              () => throw sqlException).AsTask();

            // when . then
            Assert.ThrowsAsync<DependencyException>(result);


            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowCriticalDependencyExceptionIfSqlExceptionWasThrownAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();
            var expectedDependencyException = new DependencyException(sqlException);


            // when . then
            DependencyException ex =
                Assert.Throws<DependencyException>(
                     () =>
                    {
                        this.aplicationService.TryCatch<Entity>(
                             () => throw sqlException);
                    });




            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
