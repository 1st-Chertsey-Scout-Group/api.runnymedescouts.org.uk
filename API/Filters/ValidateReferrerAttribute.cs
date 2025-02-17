using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using RunnymedeScouts.API.Options;

namespace RunnymedeScouts.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ValidateReferrerAttribute : ActionFilterAttribute
{
    private CorsOptions? _options;

    public ValidateReferrerAttribute()
    {
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var svc = context.HttpContext.RequestServices;
        _options = svc.GetService<IOptions<CorsOptions>>()?.Value;

        base.OnActionExecuting(context);
        if (!IsValidRequest(context.HttpContext.Request))
        {
            context.Result = new ContentResult
            {
                Content = $"Invalid referer header"
            };
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        }
    }

    private bool IsValidRequest(HttpRequest request)
    {
        string referrerURL = "";
        if (request.Headers.TryGetValue("Referer", out StringValues value))
        {
            referrerURL = value.ToString();
        }

        if (string.IsNullOrWhiteSpace(referrerURL))
        {
            return false;
        }

        var allowedUrls = GetAllowedUrls(request);

        var referrerAuthority = new Uri(referrerURL).Authority;
        return allowedUrls.Contains(referrerAuthority);
    }

    private List<string> GetAllowedUrls(HttpRequest request)
    {
        var allowedUrls = new List<string>
        {
            request.Host.Value
        };

        if (_options != null)
        {
            var authorities = _options.AllowedOriginsList.Select(url => new Uri(url).Authority);
            allowedUrls.AddRange(authorities);
        }

        return allowedUrls;
    }
}
