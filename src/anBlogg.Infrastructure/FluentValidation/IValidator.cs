using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace anBlogg.Infrastructure.FluentValidation
{
    public interface IValidator
    {
        bool FieldsAreInvalid<T>(string fields);

        bool OrderIsInvalid<TSource, TDestination>(string orderBy);

        bool DontMatchRules<T>(T input, ModelStateDictionary? modelState = null);
    }
}