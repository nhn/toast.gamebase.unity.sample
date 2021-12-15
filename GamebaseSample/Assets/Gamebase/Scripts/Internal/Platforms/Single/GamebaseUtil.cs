#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System;
using System.Text;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine.Networking;

namespace Toast.Gamebase.Internal.Single
{
    public static class GamebaseUtil
    {
        public static void IssueShortTermTicket(string purpose, int expiresin, string domain, GamebaseCallback.GamebaseDelegate<string> callback)
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                GamebaseLog.Warn("Not LoggedIn", typeof(GamebaseUtil), "IssueShortTermTicket");
                return;
            }

            var requestVO = AuthMessage.GetIssueShortTermTicketMessage(purpose, expiresin);

            WebSocket.Instance.Request(requestVO, (response, error) =>
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
    }
}
#endif
