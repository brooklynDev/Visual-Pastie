using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PastieAPI.Internal;

namespace PastieAPI
{
    public static class Pastie
    {
        /// <summary>
        /// Pastes the provided block of code to Pastie using the specified language. Returns the URL to the new pastie.
        /// </summary>
        /// <param name="code">The code block to paste. (Note: This will automatically be encoded)</param>
        /// <param name="language">The language of the code to paste.</param>
        /// <returns>The URL to the new Pastie.</returns>
        public static string Paste(string code, Language language)
        {
            if (String.IsNullOrEmpty(code))
            {
                throw new ArgumentException("code");
            }

            if (!Enum.IsDefined(typeof(Language), language))
            {
                throw new ArgumentException("language");
            }

            var request = (HttpWebRequest)WebRequest.Create("http://pastie.org/pastes");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Visual-Pastie";
            var builder = new StringBuilder();
            builder.Append("utf8=&#x2713;");
            builder.Append("&paste[parser_id]=" + (int)language);
            builder.Append("&paste[body]=" + CustomUrlEncoder.UrlEncode(code));
            builder.Append("&paste[restricted]=0");

            builder.Append("&paste[authorization]=burger");

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            request.ContentLength = bytes.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            var response = request.GetResponse();
            return response.ResponseUri.ToString();
        }
    }


}
