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
            var builder = new StringBuilder();
            builder.Append("utf8=&#x2713;");
            var authorizationData = GetAuthorizationValues();
            builder.Append("&paste[authorization]=" + authorizationData.AuthenticityToken);
            builder.Append("&paste[parser_id]=" + (int)language);
            builder.Append("&paste[body]=" + CustomUrlEncoder.UrlEncode(code));
            builder.Append("&paste[restricted]=0");

            builder.Append("&paste[authorization]=" + authorizationData.PasteAuthorization);

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            request.ContentLength = bytes.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            var response = request.GetResponse();
            return response.ResponseUri.ToString();
        }

        private static AuthorizationData GetAuthorizationValues()
        {
            const string authStartText = "$('#paste_authorization').val('";
            const string authTokenStartText = @"name=""authenticity_token"" type=""hidden"" value=""";

            using (var client = new WebClient())
            {
                string html = client.DownloadString("http://pastie.org/");
                var index = html.IndexOf(authStartText) + authStartText.Length;
                var endIndex = html.IndexOf('\'', index);
                var result = new AuthorizationData();
                result.PasteAuthorization = html.Substring(index, endIndex - index).Replace("'", String.Empty);

                index = html.IndexOf(authTokenStartText) + authTokenStartText.Length;
                endIndex = html.IndexOf("\"", index);
                result.AuthenticityToken = html.Substring(index, endIndex - index);
                return result;
            }
        }

        private class AuthorizationData
        {
            public string PasteAuthorization { get; set; }
            public string AuthenticityToken { get; set; }
        }

    }


}
