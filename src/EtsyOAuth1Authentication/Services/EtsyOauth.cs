using EtsyOAuth1Authentication.Enums;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using EtsyOAuth1Authentication.Interfaces;

namespace EtsyOAuth1Authentication.Services
{
    public class EtsyOauth : IEtsyOauth
    {
        private readonly Uri BASE_URL = new Uri("https://openapi.etsy.com/v2/");
        private readonly RestClient restClient;
        private readonly string appKey;
        private readonly string sharedSecret;
        private List<ScopesEnum> scopes;

        public EtsyOauth(string appKey_, string sharedSecret_)
        {
            appKey = appKey_;
            sharedSecret = sharedSecret_;
            
            restClient = new RestClient(BASE_URL);
        }

        public string GetConfirmUrlWithTempTokens(out string oauth_token, out string oauth_token_secret, List<ScopesEnum> scopes_, string callbackUrl = null)
        {
            scopes = scopes_;
            restClient.Authenticator = OAuth1Authenticator.ForRequestToken(appKey, sharedSecret, callbackUrl ?? "oob");

            var restRequest = new RestRequest("oauth/request_token", Method.POST);
            if (scopes.Count > 1)
                restRequest.AddQueryParameter("scopes", Permissions);
            var response = restClient.Execute(restRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                oauth_token = null;
                oauth_token_secret = null;
                return null;
            }

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(response.Content);

            oauth_token = queryString["oauth_token"];
            oauth_token_secret = queryString["oauth_token_secret"];

            return queryString["login_url"];
        }

        public void ObtainTokenCredentials(string oauth_token_temp_, string oauth_token_secret_temp_, string oauth_verifier_, out string permanent_oauth_token_, out string permanent_oauth_token_secret_)
        {
            //consumerKey is the appKey you got when you registered your app, same for sharedSecret
            restClient.Authenticator = OAuth1Authenticator.ForAccessToken(appKey, sharedSecret, oauth_token_temp_, oauth_token_secret_temp_, oauth_verifier_);

            RestRequest restRequest = new RestRequest("oauth/access_token", Method.GET);
            IRestResponse irestResponse = restClient.Execute(restRequest);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(irestResponse.Content);

            permanent_oauth_token_ = queryString["oauth_token"];
            permanent_oauth_token_secret_ = queryString["oauth_token_secret"];
        }

        public string GetScopes(string accessToken_, string accessTokenSecret_)
        {
            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(appKey, sharedSecret, accessToken_, accessTokenSecret_);

            RestRequest restRequest = new RestRequest("oauth/scopes", Method.GET);

            IRestResponse irestResponse = restClient.Execute(restRequest);

            return irestResponse.Content;
        }

        private string Permissions
        {
            get { return string.Join(" ", scopes); }
        }
    }
}