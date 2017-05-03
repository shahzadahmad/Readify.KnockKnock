using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Readify.KnockKnock.WebAPI.Controllers
{
    [RoutePrefix("api/Branch")]
    public class BranchController : BaseApiController
    {
       /// <summary>
        /// Get branch for given Id
        /// </summary>
        /// <param name="id">branch Id</param>
        /// <returns>branch</returns>
        // GET: api/Branch/4
        // [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public HttpResponseMessage GetBranch(int id)
        {
            try
            {
                if (id != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}