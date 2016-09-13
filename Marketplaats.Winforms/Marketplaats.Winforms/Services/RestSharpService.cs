using System;
using Marketplaats.Winforms.Model;
using System.Threading;
using Marketplaats.Winforms.Helper;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using static Marketplaats.Winforms.Properties.Settings;

namespace Marketplaats.Winforms.Services
{
    public class RestSharpService
    {

        public AccessToken AccessToken; 
        

        private  string _version = "2.0";
        private  string _clientId = Default.ClientID;
        private  string _clientSecret = Default.ClientSecret;
        private  string _redirectUri = Default.RedirectUri;
        RestRequest _request;
        RestClient _restClient;


        public bool Authentication()
        {
            bool result = false;


            do
            {
                // Get Authorization Code and an Approval from the User
                var baseUrl = "https://www.box.com";

                _restClient = new RestClient(baseUrl);

                _request = new RestRequest($"/api/oauth2/authorize?response_type=code&client_id={_clientId}&state=authenticated&redirect_uri={_redirectUri}", Method.POST);

                bool bHasUserGrantedAccess = false;

                var url = _restClient.BuildUri(_request).ToString();

                var authorizationCode = SetupHTTPServer(url);

                if (!string.IsNullOrEmpty(authorizationCode))
                {
                    bHasUserGrantedAccess = true;
                }

                if (false == bHasUserGrantedAccess)
                {
                    break;
                }
                
                
                if (GetAccessToken( authorizationCode, _restClient)) break;


#if USE_REFRESH_TOKEN
                
                // Refresh the access token (should be done if the 1 hour passed since the last access token has been obtained)
                //
                // Please not, the step refresh would fail in case the 1 hour has not passed
                // That is why the code has been cut using the preprocessor

                if (RefreshToken(ref accessToken, client)) break;
#endif






                result = true;
            }
            while (false);

            return result;
        }

        /// <summary>
        /// Below is a sample how to access any Box service regular using the valid access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public BoxUser GetBoxUser(AccessToken accessToken)
        {
            var baseUrl = "https://api.box.com";

            _restClient = new RestClient(baseUrl);

            _request = new RestRequest(string.Format("/{0}/users/me", _version), Method.GET);

            _request.AddParameter("Authorization",string.Format("Bearer {0}", accessToken.access_token), ParameterType.HttpHeader);

            var responseAccountInfo = _restClient.Execute<BoxUser>(_request);

            if (responseAccountInfo.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return responseAccountInfo.Data;
            
        }

        private  bool RefreshToken(RestClient client)
        {
            
           var request = new RestRequest("/api/oauth2/token", Method.POST);

            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("code", AccessToken.access_token);
            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("refresh_token", AccessToken.refresh_token);

            var response = client.Execute<AccessToken>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return true;
            }



            AccessToken = response.Data;


            if (string.IsNullOrEmpty(AccessToken.access_token) ||
                string.IsNullOrEmpty(AccessToken.refresh_token) ||
                (0 == AccessToken.expires_in))
            {
                return true;
            }
            return false;
        }

        private bool GetAccessToken( string authorizationCode, RestClient client)
        {
            // Get Access Token
            var request = new RestRequest("/api/oauth2/token", Method.POST);

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authorizationCode);
            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);

            var response = client.Execute<AccessToken>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                
                return true;
            }


            AccessToken = response.Data;


            if (string.IsNullOrEmpty(AccessToken.access_token) ||
                string.IsNullOrEmpty(AccessToken.refresh_token) ||
                (0 == AccessToken.expires_in))
            {
                return true;
            }
            return false;
        }

        private string SetupHTTPServer(string url)
        {
// Set up a local HTTP server to accept authetization callback
            string auth_code = null;
            var resetEvent = new ManualResetEvent(false);
            using (var svr = SimpleServer.Create(_redirectUri, context =>
            {
                var qs = HttpUtility.ParseQueryString(context.Request.RawUrl);
                auth_code = qs["code"];


                // Resume execution...
                resetEvent.Set();
            }))
            {
                // Launch a default browser to get the user's approval
                System.Diagnostics.Process.Start(url);

                // Wait until the user decides whether to grant access
                resetEvent.WaitOne();
            }
            return auth_code;
        }
    }

    
}
