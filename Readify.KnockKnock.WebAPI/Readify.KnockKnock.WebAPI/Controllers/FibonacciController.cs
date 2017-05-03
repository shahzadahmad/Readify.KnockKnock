using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using System.Runtime.Caching;

namespace Readify.KnockKnock.WebAPI.Controllers
{
    [RoutePrefix("api/Fibonacci")]
    public class FibonacciController : BaseApiController
    {
        /// <summary>
        /// The threshold for Fibonacci number if using long (Int64) type for calculating the result.
        /// </summary>
        protected readonly long threshold = 92;

        /// <summary>
        /// Calculates the negative Fibonacci sequence.
        /// </summary>
        /// <param name="n">The index in the sequence.</param>
        /// <returns>The Fibonacci number at specified position.</returns>      
        /// GET: api/Fibonacci/4
        [HttpGet]
        //[Route("{n:long}")]
        public HttpResponseMessage GetFibonacci(long n)
        {
            try
            {
                if (n > threshold)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value cannot be greater than {threshold}, since the result will cause a 64-bit integer overflow.");
                }

                if (n < -threshold)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value cannot be less than {-threshold}, since the result will cause a 64-bit integer overflow.");
                }

                var key = string.Format("FibonacciNumber{0}", n);
                var cacheItem = MemoryCache.Default.GetCacheItem(key);

                long result;

                if (cacheItem != null)
                {
                    result = (long)cacheItem.Value;
                }
                else
                {
                    result = CalculateBinetFormula(n);
                    MemoryCache.Default.Add(new CacheItem(key, result), new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(6) });
                }

                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
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

        #region Protected Methods

        // Calculates the Fibonacci number using the Binet's formula.
        // http://www.wikihow.com/Calculate-the-Fibonacci-Sequence
        protected long CalculateBinetFormula(long n)
        {
            var numerator = Math.Pow((1.0 + Math.Sqrt(5.0)), n) - Math.Pow((1.0 - Math.Sqrt(5.0)), n);
            var denominator = Math.Pow(2.0, n) * Math.Sqrt(5.0);
            var result = numerator / denominator;

            var roundedResult = Math.Round(result);

            return (long)roundedResult;
        }

        // Calculates the Fibonacci number using a sequence of "negafibonacci" numbers.
        protected long CalculateNega(long n)
        {
            long result = CalculateSequence(Math.Abs(n));

            // If n is negative and even, invert the sign.
            if (n < 0 && (n % 2 == 0))
            {
                result = -result;
            }

            return result;
        }

        // Calculates the Fibonacci number using a sequence.
        protected long CalculateSequence(long n)
        {
            if (n <= 1)
            {
                return n;
            }

            return CalculateSequence(n - 1) + CalculateSequence(n - 2);
        }

        #endregion

    }
}