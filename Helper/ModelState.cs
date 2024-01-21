
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExamBusiness.Helper
{
    public static class ModelState
    {
        public static void AddToModelState(this ValidationResult result , ModelStateDictionary ModelState)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
