using System.Collections.Generic;
using System.IO;
using System.Linq;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class SettingHistory
    {
        public class HistoryData
        {
            public GamebaseVersion gamebaseVersion = new GamebaseVersion();
            public long saveTime = 0;

            public HistoryData()
            {
                
                
            }
            public HistoryData(GamebaseVersion gamebaseVersion)
            {
                this.gamebaseVersion = gamebaseVersion;
            }
        }

        public SettingHistory()
        {
            
        }
        
        public List<HistoryData> histories = new List<HistoryData>();

        public void Remove(IGamebaseVersion gamebaseVersion)
        {
            int index = histories.FindIndex(data => data.gamebaseVersion.Equals(gamebaseVersion));
            if (index > -1)
            {
                histories.RemoveAt(index);
            }
        }

        public void AddSave(GamebaseVersion gamebaseVersion)
        {
            if (gamebaseVersion.IsValid())
            {
                int index = histories.FindIndex(data => data.gamebaseVersion.Equals(gamebaseVersion));
                if (index > -1)
                {
                    histories.RemoveAt(index);
                }

                if (histories.Count() > 4)
                {
                    histories.RemoveRange(4, histories.Count() - 4);
                }

                var data = new HistoryData(gamebaseVersion);
                data.saveTime = System.DateTime.UtcNow.Ticks;
                histories.Insert(0, data);

                string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO)
                    .adapterSelection.historyPath;
                File.WriteAllText(filePath, JsonMapper.ToJson(this));
            }
        }
    }
}