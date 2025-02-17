using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RunnymedeScouts.API.Extensions;

public static class ModelStateDictionaryExtensions
{

    public static string FriendlyMessage(this ModelStateDictionary modelState)
    {
        return string.Join("; ", modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));
    }
}
