using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public static class ValidationResultExtensions
{
    public static ApiResponse ToApiResponse(this ValidationResult validationResult)
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Validation failed",
            Errors = validationResult.Errors.Select(error => new ValidationErrorDetail
            {
                Error = error.ErrorCode,
                Detail = error.ErrorMessage
            })
        };
    }
}