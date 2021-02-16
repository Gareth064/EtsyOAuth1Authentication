using EtsyOAuth1Authentication.Enums;
using System.Collections.Generic;

namespace EtsyOAuth1Authentication.Interfaces
{
    public interface IEtsyOauth
    {
        string GetConfirmUrlWithTempTokens(out string oauth_token, out string oauth_token_secret, List<ScopesEnum> scopes_, string callbackUrl = null);
        void ObtainTokenCredentials(string oauth_token_temp_, string oauth_token_secret_temp_, string oauth_verifier_, out string permanent_oauth_token_, out string permanent_oauth_token_secret_);
        string GetScopes(string accessToken_, string accessTokenSecret_);



    }
}
