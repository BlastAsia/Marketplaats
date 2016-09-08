using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Marketplaats.Winforms
{
    public class RestSharpHelper
    {

        public IRestResponse GetApiResponse(string url, string endpoint, string username, string password, string body, string contentType, string xPage, Method method)
        {
            var restClient = new RestClient(url);
            var request = new RestRequest(method);
            request.Resource = endpoint;

            if ((!string.IsNullOrEmpty(username)) && (!string.IsNullOrEmpty(password)))
            {
                var encodedString = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));
                request.AddHeader("Authorization", string.Format("Basic {0}", encodedString));
            }

            if (!string.IsNullOrEmpty(body))
            {
                request.AddParameter(contentType, body, ParameterType.RequestBody);
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                request.AddHeader("Content-Type", contentType);
                request.AddHeader("Accept", contentType);
            }

            if (!string.IsNullOrEmpty(xPage))
            {
                request.AddHeader("X-Page", xPage);
            }

            return restClient.Execute(request);
        }
    }
}
