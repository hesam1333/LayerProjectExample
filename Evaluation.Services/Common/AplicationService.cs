using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using System.Collections.Generic;

namespace Evaluation.Services
{
    public delegate ValueTask<T> ReturningFunction<T>();
    public delegate IQueryable<T> ReturningItemsFunction<T>();

    public interface IAplicationService
    {
        ValueTask<T> TryCatchAsync<T>(ReturningFunction<T> returningFunction);
        IQueryable<T> TryCatch<T>(ReturningItemsFunction<T> returningItemsFunction);
    }


    public  class AplicationService : IAplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        public AplicationService(
            ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }
        

        public async ValueTask<T> TryCatchAsync<T>(ReturningFunction<T> returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (NullException nullException)
            {
                throw CreateAndLogValidationException(nullException);
            }
            catch (InvalidException invalidException)
            {
                throw CreateAndLogValidationException(invalidException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundException NotFoundException)
            {
                throw CreateAndLogValidationException(NotFoundException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (ModelValidationException validationException)
            {
                throw CreateAndLogModelValidationException(validationException);
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                throw CreateAndLogNotAuthorizedException(notAuthorizedException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        public IQueryable<T> TryCatch<T>(ReturningItemsFunction<T> returningItemsFunction)
        {
            try
            {
                return returningItemsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }

            catch (NotAuthorizedException notAuthorizedException)
            {
                throw CreateAndLogNotAuthorizedException(notAuthorizedException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }

        }


        private Exception CreateAndLogNotAuthorizedException(NotAuthorizedException notAuthorizedException)
        {
            this.loggingBroker.LogWarning(notAuthorizedException.Message);
            return notAuthorizedException;
        }



        protected ValidationException CreateAndLogValidationException(Exception exception)
        {
            var ValidationException = new ValidationException(exception , Message:exception.Message);
            this.loggingBroker.LogError(ValidationException);

            return ValidationException;
        }


        protected ModelValidationException CreateAndLogModelValidationException(ModelValidationException exception)
        {
            this.loggingBroker.LogWarning(exception.Message);
            return exception;
        }

        protected DependencyException CreateAndLogDependencyException(Exception exception)
        {
            var DependencyException = new DependencyException(exception);
            this.loggingBroker.LogError(DependencyException);

            return DependencyException;
        }

        protected DependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var DependencyException = new DependencyException(exception);
            this.loggingBroker.LogCritical(DependencyException);

            return DependencyException;
        }

        protected ServiceException CreateAndLogServiceException(Exception exception)
        {
            var ItemserviceException = new ServiceException(exception);

            this.loggingBroker.LogError(ItemserviceException);

            return ItemserviceException;
        }

    }
}
