using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using Microsoft.ApplicationInsights;

namespace Readify.KnockKnock.WebAPI.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : BaseApiController
    {
        // The Readify token associated with the shehzadahmed.se@gmail.com email.
        protected Guid token = new Guid("3a505596-649e-4ce3-bad0-5c912f4731f9");          

        protected TelemetryClient telemetry = new TelemetryClient();

        /// <summary>
        /// Whats the is your token.
        /// </summary>
        /// <returns>The Readify token.</returns>
        [HttpGet]
        public HttpResponseMessage GetToken()
        {
            try
            {
                var properties = new Dictionary<string, string> { { "Token", this.token.ToString() } };
                telemetry.TrackEvent("ReadifyToken", properties);

                if (this.token != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, this.token);
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