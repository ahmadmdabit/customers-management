using API.Bases;
using API.Concrets;
using BLL.Businesses;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        // GET: api/[controller]/byUser/userCode
        [HttpGet("byUser/{userCode}")]
        public virtual async Task<ActionResult<ApiResult<CustomerTable>>> GetBy([FromRoute] string userCode)
        {
            this._logger.LogInformation($"[Get:byUser/{userCode}] [{this._ip}]");
            var entities = await (this._business as CustomerTableBusiness).GetByUser(userCode, this._ip).ConfigureAwait(false);
            return Ok(new ApiResult<CustomerTable>(true, entities, null));
        }

        // POST: api/[controller]/customerProcess
        [HttpPost("customerProcess")]
        public virtual async Task<ActionResult<ApiResult<CustomerTable>>> CustomerProcess([FromBody] CustomerProcessModel customerProcessModel)
        {
            this._logger.LogInformation($"[customerProcess] [{this._ip}] {JsonConvert.SerializeObject(customerProcessModel)}");

            switch (customerProcessModel.Type)
            {
                case 0:
                    return await this.Post(new CustomerTable
                    {
                        CustomerCode = customerProcessModel.Customer.CustomerCode,
                        CustomerName = customerProcessModel.Customer.CustomerName,
                        CustomerCity = customerProcessModel.Customer.CustomerCity
                    }).ConfigureAwait(false);

                case 1:
                    return await this.Put(new CustomerTable
                    {
                        CustomerCode = customerProcessModel.Customer.CustomerCode,
                        CustomerName = customerProcessModel.Customer.CustomerName,
                        CustomerCity = customerProcessModel.Customer.CustomerCity
                    }).ConfigureAwait(false);

                case 2:
                    return await this.Delete(customerProcessModel.Customer.CustomerCode).ConfigureAwait(false);

                case 3:
                    string propertyValue =
                        (customerProcessModel.Filter == "CustomerCode" ? customerProcessModel.Customer.CustomerCode :
                            (customerProcessModel.Filter == "CustomerName" ? customerProcessModel.Customer.CustomerName :
                                (customerProcessModel.Filter == "CustomerCity" ? customerProcessModel.Customer.CustomerCity : null)
                            )
                        );
                    if (string.IsNullOrWhiteSpace(propertyValue))
                    {
                        return this.BadRequestApi();
                    }
                    //return Ok(new ApiResult<CustomerTable>(true, await this._business.GetBy(customerProcessModel.Filter, propertyValue, this._ip).ConfigureAwait(false), null));
                    var entities = await (this._business as CustomerTableBusiness).GetByUser(customerProcessModel.UserCode, this._ip).ConfigureAwait(false);
                    var propertyType = typeof(CustomerTable).GetProperty(customerProcessModel.Filter).PropertyType;
                    var param = Expression.Parameter(typeof(CustomerTable));
                    var condition =
                        Expression.Lambda<Func<CustomerTable, bool>>(
                            Expression.Equal(
                                Expression.Property(param, customerProcessModel.Filter),
                                Expression.Constant(Convert.ChangeType(propertyValue, propertyType), propertyType)
                            ),
                            param
                        )
                    .Compile(); // for LINQ to SQl/Entities skip Compile() call
                    return Ok(new ApiResult<CustomerTable>(true, entities.Where(condition), null));

                default:
                    return Ok(new ApiResult<CustomerTable>(false, null, new ErrorResult(404, "Type error!")));
            }
        }
    }
}