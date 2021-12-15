using System.Collections.Generic;
using System.Xml.Serialization;

namespace NhnCloud.GamebaseTools.SettingTool.Util.Ad
{
    public static class AdvertisementVO
    {        
        public class Day
        {
            [XmlElement("start")]
            public string start;
            [XmlElement("end")]
            public string end;
        }

        public class TimeInfo
        {
            [XmlElement("startTime")]
            public string startTime;
            [XmlElement("endTime")]
            public string endTime;
            [XmlElement("day")]
            public Day day;
        }

        public class Advertisement
        {
            [XmlElement("name")]
            public string name;
            [XmlElement("imageName")]
            public string imageName;
            [XmlElement("link")]
            public string link;
            [XmlElement("description")]
            public string description;
            [XmlElement("timeInfo")]
            public TimeInfo timeInfo;
        }

        [XmlRoot("info"), XmlType("info")]
        public class Advertisements
        {
            [XmlElement("intervalTime")]
            public int intervalTime;
            [XmlElement("imagePath")]
            public string imagePath;
            [XmlArray("advertisements")]
            [XmlArrayItem("advertisement")]
            public List<Advertisement> advertisements;
        }
    }
}
