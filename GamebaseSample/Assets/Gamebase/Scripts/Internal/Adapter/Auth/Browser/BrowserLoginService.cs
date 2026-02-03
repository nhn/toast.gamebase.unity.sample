#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using Toast.Gamebase.Internal.Result;
using System;
using System.Collections;
using System.Threading;
using Toast.Gamebase;
using Toast.Gamebase.Internal;
using Toast.Gamebase.Internal.Single;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;
using static Toast.Gamebase.Internal.GamebaseGameObjectManager;

using static Toast.Gamebase.Internal.Single.Communicator.WebSocketRequest;

namespace Toast.Gamebase.Internal.Auth.Browser
{
    public class BrowserLoginService
    {
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(180);
        private readonly IBrowser browser;
        private string _loginTicket;
        private CancellationTokenSource _cts;

        public BrowserLoginService(IBrowser browser)
        {
            this.browser = browser;
        }

        public IEnumerator LoginWithBrowser(IdPUriBuilder uriBuilder, Action<GamebaseResult<string>> callback)
        {
            var cachedRunInbackground = Application.runInBackground;
            Application.runInBackground = true;

            try
            {
                _cts = new CancellationTokenSource();
                GamebaseResult<string> browserTicket = default;
                yield return GamebaseCoroutineManager.StartCoroutine(GameObjectType.WEBSOCKET_TYPE, RequestBrowserLoginTicket(_cts.Token, res => browserTicket = res));
                if (!browserTicket.IsSuccess)
                {
                    callback?.Invoke(GamebaseResult<string>.Failure(browserTicket.Error));
                    yield break;
                }

                _loginTicket = browserTicket.Value;
                browser.OpenLoginWindow(uriBuilder.AppendTicket(browserTicket.Value).Build());

                GamebaseResult<string> browserResult = default;
                yield return GamebaseCoroutineManager.StartCoroutine(GameObjectType.WEBSOCKET_TYPE, PollForBrowserLoginResult(browserTicket.Value, TimeSpan.FromSeconds(3), _cts.Token, res => browserResult = res));
                if (!browserResult.IsSuccess)
                {
                    callback?.Invoke(GamebaseResult<string>.Failure(browserResult.Error));
                    yield break;
                }

                browser.CloseLoginWindow();
                callback?.Invoke(GamebaseResult<string>.Success(browserResult.Value));
            }
            finally
            {
                _cts?.Dispose();
                Application.runInBackground = cachedRunInbackground;
            }
        }

        private IEnumerator RequestBrowserLoginTicket(CancellationToken cancelToken, Action<GamebaseResult<string>> completed)
        {
            bool isDone = false;
            AuthResponse.BrowserLoginTicket response = null;
            GamebaseError error;

            var request = new RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.REQUEST_BROWSER_LOGIN_TICKET,
                parameters = new { expiresIn = (int)_timeout.TotalSeconds }
            };
            WebSocket.Instance.Request(request, (resp, err) =>
            {
                response = JsonMapper.ToObject<AuthResponse.BrowserLoginTicket>(resp);
                error = err;
                isDone = true;
            });

            yield return new WaitUntil(() => isDone || cancelToken.IsCancellationRequested);
            if (cancelToken.IsCancellationRequested)
            {
                completed?.Invoke(GamebaseResult<string>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_USER_CANCELED)));
                yield break;
            }

            if (response.header.isSuccessful)
            {
                completed?.Invoke(GamebaseResult<string>.Success(response.sessionTicketId));
            }
            else
            {
                completed?.Invoke(GamebaseResult<string>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED)));
            }
        }

        private IEnumerator PollForBrowserLoginResult(string sessionTicket, TimeSpan interval, CancellationToken cancelToken, Action<GamebaseResult<string>> completed)
        {
            var request = new RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.REQUEST_BROWSER_LOGIN_RESULT,
                parameters = new { sessionTicketId = sessionTicket }
            };

            while (true)
            {
                yield return new WaitForSecondsRealtime((float)interval.TotalSeconds);

                bool isDone = false;
                AuthResponse.BrowserLoginResult response = null;
                WebSocket.Instance.Request(request, (res, error) =>
                {
                    response = JsonMapper.ToObject<AuthResponse.BrowserLoginResult>(res);
                    isDone = true;
                });

                yield return new WaitUntil(() => isDone || cancelToken.IsCancellationRequested);
                if (cancelToken.IsCancellationRequested)
                {
                    completed?.Invoke(GamebaseResult<string>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_LOGIN_CANCEL_FAILED)));
                    yield break;
                }

                GamebaseResult<string> result = default;
                if (response is null || response.header.resultCode is GamebaseServerErrorCode.BROWSER_LOGIN_IN_PROGRESS)
                {
                    result = default;
                    continue;
                }
                else if (response.header.resultCode is GamebaseServerErrorCode.BROWSER_INVALID_REQUEST)
                {
                    result = GamebaseResult<string>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_INVALID_REQUEST));
                }
                else if (response.header.resultCode is GamebaseServerErrorCode.BROWSER_AUTHENTICATION_FAILED)
                {
                    result = GamebaseResult<string>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_USER_FAILED));
                }
                else if (response.header.resultCode is GamebaseErrorCode.SUCCESS)
                {
                    result = GamebaseResult<string>.Success(response.session);
                }
                else
                {
                    result = GamebaseResult<string>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_INVALID_REQUEST));
                }

                completed?.Invoke(result);
                break;
            }
        }

        public IEnumerator CancelBrowserLogin()
        {
            try
            {
                _cts?.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // already cancelled.
            }

            bool isDone = false;
            AuthResponse.CancelBrowserLoginTicket response = null;
            GamebaseError error;

            var request = new RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.REQUEST_BROWSER_LOGIN_TICKET_CANCEL,
                parameters = new { sessionTicketId = _loginTicket }
            };
            WebSocket.Instance.Request(request, (resp, err) =>
            {
                response = JsonMapper.ToObject<AuthResponse.CancelBrowserLoginTicket>(resp);
                error = err;
                isDone = true;
            });
            yield return new WaitUntil(() => isDone);
        }
    }
}
#endif