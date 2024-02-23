using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ClothingStore.API.Helpers;

public static class CustomValidationResponse
{
    public static ValidationProblemDetails CreateResponse(string key, string details, int status, ModelStateDictionary modelState)
    {
        modelState.AddModelError(key, details);
        var problemDetails = new ValidationProblemDetails(modelState)
        {
            Status = status
        };

        return problemDetails;
    }
}
