using API.Concrets;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Handlers
{
    public class CustomErrorHandler
    {
        private readonly ILogger _logger;
        private readonly bool _isApi;

        public CustomErrorHandler(ILogger logger, bool isApi = false)
        {
            this._logger = logger;
            this._logger.LogDebug(1, "NLog injected into CustomErrorHandler");
            this._isApi = isApi;
        }

        public Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
        => WriteResponse(httpContext, includeDetails: true);

        public Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: false);

        public async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            var statusCode = 500;
            // Try and retrieve the error from the ExceptionHandler middleware
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            // Should always exist, but best to be safe!
            if (ex != null)
            {
                // Get the details to display, depending on whether we want to expose the raw exception
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                var title = includeDetails ? errorMessage : "An error occured";
                Regex statusCodePattern = new Regex(@"\[\d+\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                if (statusCodePattern.IsMatch(errorMessage))
                {
                    Match match = statusCodePattern.Match(errorMessage);
                    for (int ii = 0; ii < match.Groups.Count; ii++)
                    {
                        int code = int.Parse(match.Groups[ii]?.Value.TrimStart('[').TrimEnd(']'));

                        if (Array.IndexOf(HttpStatusCodes.Keys.ToArray(), code) > -1)
                        {
                            statusCode = code;
                            title = new Regex(@"\[" + code.ToString() + @"\] ", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(title, "");
                            break;
                        }
                    }
                }
                if (new Regex(@"^\[SQL_*\w*\]", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(errorMessage))
                {
                    title = new Regex(@"^\[SQL_*\w*\]", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(title, "");
                    title = $"[SQL {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] {title}";
                }
                // Show UI error in production
                else if (new Regex(@"^\[UI_*\w*\]", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(errorMessage))
                {
                    errorMessage = new Regex(@"^\[UI_*\w*\]", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(errorMessage, "");
                    title = $"[UI {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] {errorMessage}";
                }

                this._logger.LogError(ex, title);

                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                var stream = httpContext.Response.Body;

                if (this._isApi)
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = 200; // statusCode;
                    await JsonSerializer.SerializeAsync(stream, new ApiResult(false, null, new ErrorResult(statusCode, title))).ConfigureAwait(false);
                }
                else
                {
                    // ProblemDetails has it's own content type
                    httpContext.Response.ContentType = "application/problem+json";
                    var details = includeDetails ? ex.ToString() : null;
                    var problem = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = title,
                        Detail = details
                    };

                    // This is often very handy information for tracing the specific request
                    var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
                    if (traceId != null)
                    {
                        problem.Extensions["traceId"] = traceId;
                    }

                    await JsonSerializer.SerializeAsync(stream, problem).ConfigureAwait(false);
                }
            }
        }

        // <ref: https://restfulapi.net/http-status-codes/>
        private readonly Dictionary<int, string> HttpStatusCodes = new Dictionary<int, string> {
            // 1×× Informational
            {100, "Continue"},
            {101, "Switching Protocols"},
            {102, "Processing"},
            // 2×× Success
            {200, "OK"},
            {201, "Created"},
            {202, "Accepted"},
            {203, "Non-authoritative Information"},
            {204, "No Content"},
            {205, "Reset Content"},
            {206, "Partial Content"},
            {207, "Multi-Status"},
            {208, "Already Reported"},
            {226, "IM Used"},
            // 3×× Redirection
            {300, "Multiple Choices"},
            {301, "Moved Permanently"},
            {302, "Found"},
            {303, "See Other"},
            {304, "Not Modified"},
            {305, "Use Proxy"},
            {307, "Temporary Redirect"},
            {308, "Permanent Redirect"},
            // 4×× Client Error
            {400, "Bad Request"},
            {401, "Unauthorized"},
            {402, "Payment Required"},
            {403, "Forbidden"},
            {404, "Not Found"},
            {405, "Method Not Allowed"},
            {406, "Not Acceptable"},
            {407, "Proxy Authentication Required"},
            {408, "Request Timeout"},
            {409, "Conflict"},
            {410, "Gone"},
            {411, "Length Required"},
            {412, "Precondition Failed"},
            {413, "Payload Too Large"},
            {414, "Request-URI Too Long"},
            {415, "Unsupported Media Type"},
            {416, "Requested Range Not Satisfiable"},
            {417, "Expectation Failed"},
            {418, "I’m a teapot"},
            {421, "Misdirected Request"},
            {422, "Unprocessable Entity"},
            {423, "Locked"},
            {424, "Failed Dependency"},
            {426, "Upgrade Required"},
            {428, "Precondition Required"},
            {429, "Too Many Requests"},
            {431, "Request Header Fields Too Large"},
            {444, "Connection Closed Without Response"},
            {451, "Unavailable For Legal Reasons"},
            {499, "Client Closed Request"},
            // 5×× Server Error
            {500, "Internal Server Error"},
            {501, "Not Implemented"},
            {502, "Bad Gateway"},
            {503, "Service Unavailable"},
            {504, "Gateway Timeout"},
            {505, "HTTP Version Not Supported"},
            {506, "Variant Also Negotiates"},
            {507, "Insufficient Storage"},
            {508, "Loop Detected"},
            {510, "Not Extended"},
            {511, "Network Authentication Required"},
            {599, "Network Connect Timeout Error"}
        };
    }
}
