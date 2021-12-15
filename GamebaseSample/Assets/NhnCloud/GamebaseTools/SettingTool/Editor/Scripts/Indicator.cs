using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class Indicator : IDisposable
    {
        private const string BODY = "SettingTool";

        private string url;
        private Dictionary<string, string> staticData;

        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        public void Initialize()
        {
            var data = DataManager.GetData<SettingToolResponse.LaunchingData>(DataKey.LAUNCHING);

#if GAMEBASE_SETTINGTOOL_ALPHA_ZONE
            var zone = data.launching.settingTool.alpha;
#else
            var zone = data.launching.settingTool.real;
#endif
            url = zone.url;

            staticData = new Dictionary<string, string>
            {
                {"projectName", zone.appKey},
                {"projectVersion", SettingTool.VERSION},
                {"logVersion", zone.logVersion},
                {"body", BODY}
            };
        }

        public void Send(Dictionary<string, string> data)
        {
            if (data == null)
            {
                data = staticData;
            }
            else
            {
                staticData.ToList().ForEach(x => data.Add(x.Key, x.Value));
            }

            var jsonString = JsonMapper.ToJson(data);
            EditorCoroutines.StartCoroutine(SendHTTPPost(staticData["logVersion"], jsonString), this);
        }

        private IEnumerator SendHTTPPost(string logVersion, string jsonString)
        {
            var encoding = new UTF8Encoding().GetBytes(jsonString);

            var request = UnityWebRequest.Put(string.Format("{0}/{1}/log", url, logVersion), encoding);
            request.method = UnityWebRequest.kHttpVerbPOST;

            var helper = new UnityWebRequestHelper(request);

            yield return EditorCoroutines.StartCoroutine(helper.SendWebRequest(), this);
        }
    }
}