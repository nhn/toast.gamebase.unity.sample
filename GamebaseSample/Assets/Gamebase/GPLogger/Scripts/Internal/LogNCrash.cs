using GamePlatform.Logger.ThirdParty;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace GamePlatform.Logger.Internal
{
    public class LogNCrash
    {
        public const string VERSION = "v2";

        private readonly ServiceZone zone;
        private readonly string appKey;

        public GpLoggerResponse.LogNCrashSettings Settings { get; private set; }

        public LogNCrash(string appKey, ServiceZone zone)
        {
            if (string.IsNullOrEmpty(appKey) == true)
            {
                GpLog.Warn(string.Format("Log&Crash appKey cannot be null. appKey:{0}", appKey), GetType(), "LogNCrash");
            }

            this.appKey = appKey;
            this.zone = zone;

            Settings = new GpLoggerResponse.LogNCrashSettings();
        }

        public string GetAppKey()
        {
            return appKey;
        }

        public ServiceZone GetServiceZone()
        {
            return zone;
        }

        public string GetCollectorUrl()
        {
            var builder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = string.Format("{0}{1}", GetZoneString(), GpUtil.MergeStrings("ailgcahcodtatcm", "p-onrs.lu.os.o")),
                Path = string.Format("{0}/{1}", VERSION, "log")
            };

            return builder.ToString();
        }

        private string GetSettingsUrl()
        {
            var builder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = string.Format("{0}{1}", GetZoneString(), GpUtil.MergeStrings("stiglgcahcodtatcm", "etn-onrs.lu.os.o")),
                Path = string.Format("{0}/{1}/{2}", VERSION, appKey, "logsconf")
            };

            return builder.ToString();
        }

        public IEnumerator RequestLogNCrashSettings(Action callback)
        {
            using (var request = UnityWebRequest.Get(GetSettingsUrl()))
            {
                var helper = new UnityWebRequestHelper(request);
                yield return GameObjectManager.GetCoroutineComponent(GameObjectType.GP_LOGGER).StartCoroutine(helper.SendWebRequest(() =>
                {
                    var jsonString = helper.GetData();

                    if (string.IsNullOrEmpty(jsonString) == false)
                    {
#if UNITY_STANDALONE || UNITY_EDITOR
                        SettingsFileManager.SaveFile(GetAppKey(), jsonString);
#endif
                        if (string.IsNullOrEmpty(jsonString) == false)
                        {
                            Settings = JsonMapper.ToObject<GpLoggerResponse.LogNCrashSettings>(jsonString);
                        }
                    }
                    else
                    {
#if UNITY_STANDALONE || UNITY_EDITOR
                        jsonString = SettingsFileManager.LoadFile(GetAppKey());

                        if (string.IsNullOrEmpty(jsonString) == false)
                        {
                            Settings = JsonMapper.ToObject<GpLoggerResponse.LogNCrashSettings>(jsonString);
                        }
#endif
                    }

                    callback();
                }));
            }
        }

        private string GetZoneString()
        {
            if (zone == ServiceZone.REAL)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0}-", zone.ToString().ToLower());
            }
        }
    }
}