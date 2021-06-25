using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Common.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using BLL.Interfaces;
using DAL.Bases;
using DAL.Interfaces;
using API.Concrets;

namespace ApiLayerLibrary.Bases
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public abstract class BaseApiController<TEntity> : ControllerBase, IController<TEntity>
        where TEntity : BaseEntity, IEntity
    {
        protected readonly IBusinessWrapper _businessWrapper;
        protected readonly IBusiness<TEntity> _business;
        protected readonly ILogger _logger;
        protected readonly IActionContextAccessor _accessor;
        protected readonly string _ip;

        protected BaseApiController(IBusinessWrapper businessWrapper, ILogger<BaseApiController<TEntity>> logger, IActionContextAccessor accessor)
        {
            this._businessWrapper = businessWrapper;
            this._business = this._businessWrapper.PropertyFindValue(typeof(IBusiness<TEntity>)) as IBusiness<TEntity>;
            this._logger = logger;
            this._accessor = accessor;
            this._ip = this._accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> Delete(long id)
        {
            this._logger.LogInformation($"[Delete:{id}] [{this._ip}]");
            var entity = await this._business.Delete(id, this._ip).ConfigureAwait(false);
            if (entity != null)
            {
                return Ok(new ApiResult<TEntity>(true, entity, null));
            }
            return this.NotFoundApi();
        }

        // DELETE: api/[controller]
        [HttpDelete]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> DeleteRange([FromBody]List<TEntity> entities)
        {
            this._logger.LogInformation($"[DeleteRange:{JsonConvert.SerializeObject(entities)}] [{this._ip}]");
            var entity = await this._business.DeleteRange(entities, this._ip).ConfigureAwait(false);
            if (entity.Count > 0)
            {
                return Ok(new ApiResult<TEntity>(true, entity, null));
            }
            return this.NotFoundApi();
        }

        // GET: api/[controller]
        [HttpGet]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> Get()
        {
            this._logger.LogInformation($"[Get] [{this._ip}]");
            return Ok(new ApiResult<TEntity>(true, await this._business.GetAll(this._ip).ConfigureAwait(false), null));
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> Get(long id)
        {
            this._logger.LogInformation($"[Get:{id}] [{this._ip}]");
            var entity = await this._business.Get(id, this._ip).ConfigureAwait(false);
            if (entity != null)
            {
                return Ok(new ApiResult<TEntity>(true, entity, null));
            }
            return this.NotFoundApi();
        }

        // GET: api/[controller]/by/id/5
        [HttpGet("by/{propertyName}/{propertyValue}")]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> GetBy([FromRoute]string propertyName, [FromRoute]string propertyValue)
        {
            this._logger.LogInformation($"[Get:by/{propertyName}/{propertyValue}] [{this._ip}]");
            var entities = await this._business.GetBy(propertyName, propertyValue, this._ip).ConfigureAwait(false);
            return Ok(new ApiResult<TEntity>(true, entities, null));
        }

        // GET: api/[controller]/by/id/5/name/test
        [HttpGet("by/{property1Name}/{property1Value}/{property2Name}/{property2Value}")]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> GetBy(
            [FromRoute]string property1Name, [FromRoute]string property1Value, [FromRoute]string property2Name, [FromRoute]string property2Value)
        {
            this._logger.LogInformation($"[Get:by/{property1Name}/{property1Value}/{property2Name}/{property2Value}] [{this._ip}]");
            var entities = await this._business.GetBy(property1Name, property1Value, property2Name, property2Value, this._ip).ConfigureAwait(false);
            return Ok(new ApiResult<TEntity>(true, entities, null));
        }

        // POST: api/[controller]
        [HttpPost]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> Post([FromBody]TEntity entity)
        {
            //return null;
            this._logger.LogInformation($"[Post] [{this._ip}] {JsonConvert.SerializeObject(entity)}");
            var businessEntity = await this._business.Add(entity, this._ip).ConfigureAwait(false);
            if (businessEntity != null)
            {
                return Ok(new ApiResult<TEntity>(true, businessEntity, null));
            }
            return this.BadRequestApi();
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<ApiResult<TEntity>>> Put(long id, [FromBody]TEntity entity)
        {
            this._logger.LogInformation($"[Put:{id}] [{this._ip}] {JsonConvert.SerializeObject(entity)}");
            var businessEntity = await this._business.Update(id, entity, this._ip).ConfigureAwait(false);
            if (businessEntity != null)
            {
                return Ok(new ApiResult<TEntity>(true, businessEntity, null));
            }
            return this.BadRequestApi();
        }

        protected virtual ActionResult<ApiResult<TEntity>> NotFoundApi()
        {
            this._logger.LogInformation("NotFound");
            return Ok(new ApiResult<TEntity>(false, null, new NotFoundResult().StatusCode, "NotFound"));
        }

        protected virtual ActionResult<ApiResult<TEntity>> BadRequestApi()
        {
            this._logger.LogInformation("BadRequest");
            return Ok(new ApiResult<TEntity>(false, null, new BadRequestResult().StatusCode, "BadRequest"));
        }
    }
}
