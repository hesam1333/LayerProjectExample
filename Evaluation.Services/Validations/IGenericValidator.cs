using Evaluation.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Validations
{
    public interface IGenericValidator<Tentity>
    {
        Task ValidateEntityAsync(Tentity item);
        void ValidateEntityIsNotNull(Tentity entity);
        void ValidateStorageEntity(Tentity storageEntity, int Id);
    }
}
