using API.Bases;
using API.Concrets;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        // POST: api/[controller]/userCheck
        [HttpPost("userCheck")]
        public virtual async Task<ActionResult<ApiResult<UserTable>>> UserCheck([FromBody] UserTable userTable)
        {
            this._logger.LogInformation($"[userCheck] [{this._ip}] {JsonConvert.SerializeObject(userTable)}");
            var entities = await this._business.GetBy("UserCode", userTable.UserCode, "UserPass", userTable.UserPass, this._ip).ConfigureAwait(false);
            if (entities?.Count > 0)
            {
                return Ok(new ApiResult<UserTable>(true, 0, null));
            }
            entities = await this._business.GetBy("UserCode", userTable.UserCode, this._ip).ConfigureAwait(false);
            if (entities?.Count > 0)
            {
                return Ok(new ApiResult<UserTable>(false, 3, new ErrorResult(404, "UserPass Error!")));
            }
            else
            {
                return Ok(new ApiResult<UserTable>(false, 1, new ErrorResult(404, "UserCode Not Found!")));
            }
        }
    }
}
