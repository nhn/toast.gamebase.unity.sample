using System;
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public interface IAuthAdapter
    {
        void IDPLogin(Action<GamebaseError> callback);
        void IDPLogin(Dictionary<string, object> additionalInfo, Action<GamebaseError> callback);
        void IDPLogout();
        string GetIDPName();
        string GetIDPUserID();
        string GetIDPAccessToken();
        GamebaseResponse.Auth.AuthProviderProfile GetIDPProfile();
    }
}