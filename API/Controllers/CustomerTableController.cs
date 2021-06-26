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
    public class CustomerTableController : BaseApiController<CustomerTable>
    {
        public CustomerTableController(IBusiness<CustomerTable> business, ILogger<CustomerTableController> logger, IActionContextAccessor accessor) : base(business, logger, accessor)
        {
        }

        // GET: api/[controller]/customerProcess/customerCode/customerName/customerCity/type/filter
        [HttpGet("customerProcess/{customerCode}/{customerName}/{customerCity}/{type}/{filter}")]
        public virtual async Task<ActionResult<ApiResult<CustomerTable>>> CustomerProcess(
            [FromRoute] string customerCode, [FromRoute] string customerName, [FromRoute] string customerCity, [FromRoute] int type, [FromRoute] string filter)
        {
            this._logger.LogInformation($"[customerProcess/{customerCode}/{customerName}/{customerCity}/{type}/{filter}] [{this._ip}]");

            switch (type)
            {
                case 0:
                    return await this.Post(new CustomerTable { CustomerCode = customerCode, CustomerName = customerName, CustomerCity = customerCity }).ConfigureAwait(false);
                case 1:
                    return await this.Put(new CustomerTable { CustomerCode = customerCode, CustomerName = customerName, CustomerCity = customerCity }).ConfigureAwait(false);
                case 2:
                    return await this.Delete(customerCode).ConfigureAwait(false);
                case 3:
                    string propertyValue =
                        (filter == "CustomerCode" ? customerCode :
                            (filter == "CustomerName" ? customerName :
                                (filter == "CustomerCity" ? customerCity : null)
                            )
                        );
                    if (string.IsNullOrWhiteSpace(propertyValue))
                    {
                        return this.BadRequestApi();
                    }
                    return Ok(new ApiResult<CustomerTable>(true, await this._business.GetBy(filter, propertyValue, this._ip).ConfigureAwait(false), null));
                default:
                    return Ok(new ApiResult<CustomerTable>(false, null, new ErrorResult(404, "Type error!")));
            }
        }
    }
}
