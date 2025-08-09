using FluentValidation.Results;

namespace SD_Turizm.Core.Exceptions
{
    public class ValidationException : BusinessException
    {
        public IEnumerable<ValidationFailure> Errors { get; }

        public ValidationException(string message, IEnumerable<ValidationFailure> errors)
            : base(message, "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }

        public ValidationException(IEnumerable<ValidationFailure> errors)
            : base("Validation failed", "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }
    }
}
