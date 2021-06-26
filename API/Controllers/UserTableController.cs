using API.Bases;
using API.Concrets;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserTableController : BaseApiController<UserTable>
    {
        public UserTableController(IBusiness<UserTable> business, ILogger<UserTableController> logger, IActionContextAccessor accessor) : base(business, logger, accessor)
        {
        }

        // GET: api/[controller]/userCheck/userCode/userPass
        [HttpGet("userCheck/{userCode}/{userPass}")]
        public virtual async Task<ActionResult<ApiResult<UserTable>>> UserCheck([FromRoute] string userCode, [FromRoute] string userPass)
        {
            this._logger.LogInformation($"[userCheck/{userCode}/{userPass}] [{this._ip}]");
            var entities = await this._business.GetBy("UserCode", userCode, "UserPass", userPass, this._ip).ConfigureAwait(false);
            if (entities?.Count > 0)
            {
                return Ok(new ApiResult<UserTable>(true, 0, null));
            }
            entities = await this._business.GetBy("UserCode", userCode, this._ip).ConfigureAwait(false);
            if (entities?.Count > 0)
            {
                return Ok(new ApiResult<UserTable>(true, 3, null));
            }
            else
            {
                return Ok(new ApiResult<UserTable>(true, 1, null));
            }
        }
    }
}
