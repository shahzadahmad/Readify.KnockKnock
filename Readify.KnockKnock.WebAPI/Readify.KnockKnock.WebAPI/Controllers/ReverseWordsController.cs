using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using System.Runtime.Caching;
using System.Text;

namespace Readify.KnockKnock.WebAPI.Controllers
{
    [RoutePrefix("api/ReverseWords")]
    public class ReverseWordsController : BaseApiController
    {
        // The separator between words.
        // Note: Add dot, comma, question mark, exclamation mark etc. to make it more intelligence.
        protected char[] separator = { ' ' };

        /// <summary>
        /// Reverses words in the specified string.
        /// </summary>
        /// <param name="sentence">The string.</param>
        /// <returns>The string with reversed words.</returns>
        /// GET: api/ReverseWords/Apple
        [HttpGet]
        //[Route("{sentence}")]
        public HttpResponseMessage GetReverseWords(string sentence)
        {
            try
            {
                if (sentence == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value cannot be null.");
                }

                var key = string.Format("ReverseWords{0}", sentence.GetHashCode());
                var cacheItem = MemoryCache.Default.GetCacheItem(key);

                string result = string.Empty;

                if (cacheItem != null)
                {
                    result = (string)cacheItem.Value;
                }
                else
                {
                    var words = this.Split(sentence, separator);
                    var reversedWords = new StringBuilder();

                    foreach (var word in words)
                    {
                        reversedWords.Append(ReverseWord(word));
                    }

                    result = reversedWords.ToString();

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

        /// <summary>
        /// Reverses the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The reversed word.</returns>
        public string ReverseWord(string word)
        {
            char[] charArray = word.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

        #region Protected Methods

        protected string[] Split(string value, char[] separator)
        {
            var result = new List<string>();
            string[] temp;

            do
            {
                temp = this.InnerSplit(value, separator);
                result.Add(temp.First());
                value = temp.Last();
            }
            while (temp.Length > 1);

            return result.ToArray();
        }

        protected string[] InnerSplit(string value, char[] separator)
        {
            string[] result = new string[2];

            if (value.Length > 1)
            {
                for (int index = 0; index < value.Length; index++)
                {
                    if (this.IsDelimiterChar(value[index], separator))
                    //if (value[index].Equals(' '))
                    {
                        int endIndex = index == 0 ? index + 1 : index;

                        result[0] = value.Substring(0, endIndex);
                        result[1] = value.Substring(endIndex);

                        return result;
                    }
                }
            }

            return new string[] { value };
        }

        protected bool IsDelimiterChar(char character, char[] delimiterCharacters)
        {
            bool result = false;

            for (int index = 0; index < delimiterCharacters.Length; index++)
            {
                if (delimiterCharacters[index] == character)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

    }
}