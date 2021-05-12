using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Validations
{
    public class Genericvalidator<Tentity> : AbstractValidator<Tentity>, IGenericValidator<Tentity> 
    {
        public async Task ValidateEntityAsync(Tentity entity)
        {
            ValidateEntityIsNotNull(entity);

            var validationResult = await this.ValidateAsync(entity);


            if (!validationResult.IsValid)
            {
                var validationError = new ValidationError()
                {
                    Errors = validationResult.Errors.Select(i => new VaidationErrorItem()
                    {
                        FieldErrorMessage = i.ErrorMessage,
                        FieldName = i.PropertyName
                    }).ToList()
                };


                throw new ModelValidationException(null, validationError);
            }
        }

        public void ValidateEntity(Tentity entity)
        {
            ValidateEntityIsNotNull(entity);

            var validationResult =  this.Validate(entity);

            if (!validationResult.IsValid)
            {
                var validationError = new ValidationError()
                {
                    Errors = validationResult.Errors.Select(i => new VaidationErrorItem()
                    {
                        FieldErrorMessage = i.ErrorMessage,
                        FieldName = i.PropertyName
                    }).ToList()
                };


                throw new ModelValidationException(null, validationError);
            }
        }




        public void ValidateEntityIsNotNull(Tentity entity)
        {
            if (entity is null)
            {
                throw new NullException();
            }
        }


        public void ValidateStorageEntity(Tentity storageEntity, int Id)
        {
            if (storageEntity == null)
            {
                throw new NotFoundException(Id);
            }
        }

    }
}
