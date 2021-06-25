using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Bases;
using DAL.Interfaces;

namespace API.Concrets
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public object Data { get; set; }

        public ErrorResult Error { get; set; }

        public ApiResult()
        {
        }

        public ApiResult(bool success, object data, ErrorResult error)
        {
            this.Success = success;
            this.Data = data;
            this.Error = error;
        }

        public ApiResult(bool success, object data, int errorCode, string errorMessage)
        {
            this.Success = success;
            this.Data = data;
            this.Error = new ErrorResult(errorCode, errorMessage);
        }
    }

    public class ApiResult<TEntity> : ApiResult
        where TEntity : BaseEntity, IEntity
    {
        public ApiResult()
        {
        }

        public ApiResult(bool success, object data, ErrorResult error) : base(success, data, error)
        {
            this.Data = data;
        }

        public ApiResult(bool success, object data, int errorCode, string errorMessage) : base(success, data, errorCode, errorMessage)
        {
            this.Data = data;
        }
    }

    public class RequestValidationApiResult : ApiResult, IActionResult
    {
        public RequestValidationApiResult()
        {
            this.Success = false;
            this.Data = null;
            this.Error = new ErrorResult(400, "Request Validation Error: A non-empty request body is required.");
        }

        public RequestValidationApiResult(bool success, object data, ErrorResult error) : base(success, data, error)
        {
        }

        public RequestValidationApiResult(bool success, object data, int errorCode, string errorMessage) : base(success, data, errorCode, errorMessage)
        {
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new ObjectResult(this) { StatusCode = StatusCodes.Status200OK }.ExecuteResultAsync(context).ConfigureAwait(false);
        }
    }
}
