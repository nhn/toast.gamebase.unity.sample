#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single
{
    public static class GamebaseUtil
    {
        public static WebSocketOperation IssueShortTermTicket(string userID, string purpose, int expiresin, string domain, GamebaseCallback.GamebaseDelegate<string> callback)
        {
            var requestVO = AuthMessage.GetIssueShortTermTicketMessage(userID, purpose, expiresin);
            return WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                if (error == null)
                {
                    var vo = JsonMapper.ToObject<AuthResponse.IssueShortTermTicketInfo>(response);
                    if (vo.header.isSuccessful == true)
                    {
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, domain);
                    }

                    callback(vo.ticket, error);
                }
                else
                {
                    callback(null, error);
                }
            });
        }
        
        public static bool IsLaunchingPlayable(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }
    }
}
#endif