using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Services;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;

namespace SchoolEM.Tests.Services.StudentServiceTests
{
    public partial class AplicationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly AplicationService aplicationService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IGenericRepository<Entity>> genericRepository;

        public AplicationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.genericRepository = new Mock<IGenericRepository<Entity>>();

            this.aplicationService = new AplicationService(
                loggingBroker: this.loggingBrokerMock.Object);
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

        private IQueryable<Entity> CreateRandomEntitys()
        {
            int randomNumber = new IntRange(min: 2, max: 10).GetValue();

            return new Filler<Entity>().Create(randomNumber).AsQueryable();
        }
    }
}
