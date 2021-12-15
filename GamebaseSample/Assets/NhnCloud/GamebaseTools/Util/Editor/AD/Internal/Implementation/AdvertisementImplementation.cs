using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Util.Ad.Internal
{
    public class AdvertisementImplementation
    {
        private const int KOREA_STANDARD_TIME = 9;

        private class AdvertisementData
        {
            public string link;
            public string adName;
            public string fileName;
            public string textureKey;

            public AdvertisementData(string adName, string fileName, string link, string textureKey)
            {
                this.link = link;
                this.adName = adName;
                this.fileName = fileName;
                this.textureKey = textureKey;
            }
        }

        private class DownloadImagePath
        {
            public string remoteUrl;
            public string localPath;
            public string fileName;

            public DownloadImagePath(string remoteUrl, string localPath, string fileName)
            {
                this.remoteUrl = remoteUrl;
                this.localPath = localPath;
                this.fileName = fileName;
            }
        }

        private const int OFFSET_DARK = 2;
        private const int OFFSET_GRAY = 1;

        private static readonly AdvertisementImplementation instance = new AdvertisementImplementation();
                
        private AdvertisementConfigurations advertisementConfigurations;
        private EditorWindow window;
        private Action<string, string> selectAdvertisementInfoCallback;
        private string languageCode;

        private AdvertisementVO.Advertisements advertisements;
        private List<DownloadImagePath> downloadImageList = new List<DownloadImagePath>();
        private Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        private List<AdvertisementData> drawAdvertisementList = new List<AdvertisementData>();
        private int advertisementIndex= 0;

        private Rect drawRect;
        private Rect darkRect;
        private Rect grayRect;

        private Texture2D bgTextureDark;
        private Texture2D bgTextureGray;

        private bool isInitialize = false;
                
        private double lastTimeSinceStartup;
        private double elapseTime;

        public static AdvertisementImplementation Instance
        {
            get { return instance; }
        }

        private AdvertisementImplementation()
        {
            bgTextureDark = new Texture2D(1,1);
            bgTextureDark.SetPixel(0, 0, new Color(0.15f, 0.15f, 0.15f));
            bgTextureDark.Apply();

            bgTextureGray = new Texture2D(1, 1);
            bgTextureGray.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
            bgTextureGray.Apply();
        }

        public void Initialize(EditorWindow window, Rect drawRect, AdvertisementConfigurations advertisementConfigurations, string languageCode)
        {
            lastTimeSinceStartup = 0;
            elapseTime = 0;

            downloadImageList.Clear();
            textureDict.Clear();
            drawAdvertisementList.Clear();

            this.drawRect = drawRect;
            this.darkRect = new Rect(
                drawRect.x - OFFSET_DARK, 
                drawRect.y - OFFSET_DARK, 
                drawRect.width + OFFSET_DARK * 2, 
                drawRect.height + OFFSET_DARK * 2);
            this.grayRect = new Rect(
                drawRect.x - OFFSET_GRAY,
                drawRect.y - OFFSET_GRAY,
                drawRect.width + OFFSET_GRAY * 2,
                drawRect.height + OFFSET_GRAY * 2);

            this.advertisementConfigurations = advertisementConfigurations;
            this.languageCode = languageCode;
            
            this.window = window;

            LoadAdvertisement(
                () =>
                {
                    DownloadImage(
                        ()=>
                        {
                            TrimLanguages();
                            LoadTexture();
                            MakeAdvertisementList();
                            EditorApplication.update += Update;
                            AssetDatabase.Refresh();
                            window.Repaint();
                            isInitialize = true;
                        });
                });           
        }

        public void SetLanguageCode(string languageCode)
        {
            if(HasLanguageCode(languageCode) == false)
            {
                return;
            }

            this.languageCode = languageCode;
            MakeAdvertisementList();
        }

        public void Draw()
        {
            var advertisementData = GetAdvertisementData();
            if(advertisementData == null)
            {
                return;
            }

            var drawTexture = GetTexture(advertisementData.textureKey);
            if (drawTexture == null)
            {
                return;
            }

            if (advertisementConfigurations.isActiveBG == true)
            {
                GUI.DrawTexture(darkRect, bgTextureDark);
                GUI.DrawTexture(grayRect, bgTextureGray);
            }

            if (GUI.Button(drawRect, "") == true)
            {
                if (selectAdvertisementInfoCallback != null)
                {
                    selectAdvertisementInfoCallback(advertisementData.adName, advertisementData.link);
                }
                Application.OpenURL(advertisementData.link);
            }

            GUI.DrawTexture(drawRect, drawTexture);
        }
        
        public void OnDestroy()
        {
            window = null;
            EditorApplication.update -= Update;
        }
        
        public void SetSelectAdvertisementInfoCallback(Action<string, string> selectAdvertisementInfoCallback)
        {
            this.selectAdvertisementInfoCallback = selectAdvertisementInfoCallback;
        }

        private bool HasLanguageCode(string languageCode)
        {
            foreach(var language in advertisementConfigurations.languages)
            {
                if(language.Equals(languageCode) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private void Update()
        {
            if (isInitialize == false)
            {
                return;
            }

            if (drawAdvertisementList.Count == 0)
            {
                return;
            }

            NextAdvertisementIndex();
        }

        private void LoadAdvertisement(Action callback)
        {
            FileManager.DownloadFileToString(
                string.Format("{0}{1}", advertisementConfigurations.remoteUrl, advertisementConfigurations.xmlFileName),
                (stateCode, message, src) =>
                {
                    if (FileManager.StateCode.SUCCESS == stateCode)
                    {
                        XMLManager.LoadXMLFromText<AdvertisementVO.Advertisements>(
                        src,
                        (stateCodeXML, dataXML, messageXML) =>
                        {
                            if (XMLManager.ResponseCode.SUCCESS == stateCodeXML)
                            {
                                advertisements = dataXML;

                                foreach (var ad in advertisements.advertisements)
                                {
                                    foreach (var language in advertisementConfigurations.languages)
                                    {
                                        string remoteUrl = string.Format(
                                            "{0}{1}{2}/",
                                            advertisementConfigurations.remoteUrl,
                                            advertisements.imagePath,
                                            language
                                            );

                                        string localPath = string.Format(
                                            "{0}{1}{2}/",
                                            advertisementConfigurations.imageDownloadPath,
                                            advertisements.imagePath,
                                            language
                                            );

                                        downloadImageList.Add(new DownloadImagePath(
                                            remoteUrl,
                                            localPath,
                                            ad.imageName
                                            ));
                                    }
                                }

                                callback();
                            }
                        });                        
                    }
                },
                null);
        }

        private void DownloadImage(Action callback)
        {
            DownloadImage(0, callback);
        }

        private void DownloadImage(int index, Action callback)
        {
            if (downloadImageList.Count <= index)
            {
                callback();
                return;
            }

            var downloadImage = downloadImageList[index++];

            if(Directory.Exists(downloadImage.localPath) == false)
            {
                Directory.CreateDirectory(downloadImage.localPath);
            }

            string localFile = string.Format("{0}{1}", downloadImage.localPath, downloadImage.fileName);

            if(File.Exists(localFile) == true)
            {
                DownloadImage(index, callback);
                return;
            }

            FileManager.DownloadFileToLocal(
                string.Format("{0}{1}", downloadImage.remoteUrl, downloadImage.fileName),
                localFile,
                (stateCode, message) =>
                {
                    DownloadImage(index, callback);
                },
                null
                );
        }

        private void TrimLanguages()
        {
            foreach (var language in advertisementConfigurations.languages)
            {
                string directoryPath = string.Format(
                    "{0}{1}{2}",
                    advertisementConfigurations.imageDownloadPath,
                    advertisements.imagePath,
                    language
                    );

                string[] images = Directory.GetFiles(directoryPath);

                if(images == null || images.Length == 0)
                {
                    //advertisementConfigurations.languages = advertisementConfigurations.languages.Where(w => w != language).ToArray();
                }
            }

            foreach (var language in advertisementConfigurations.languages)
            {
                if(language.Equals(languageCode) == true)
                {
                    return;
                }
            }

            if(advertisementConfigurations.languages.Length > 0)
            {
                languageCode = advertisementConfigurations.languages[0];
            }            
        }        

        private void LoadTexture()
        {
            foreach (var ad in advertisements.advertisements)
            {
                foreach (var language in advertisementConfigurations.languages)
                {
                    string filePath = string.Format(
                        "{0}{1}{2}/{3}",
                        advertisementConfigurations.imageDownloadPath,
                        advertisements.imagePath,
                        language,
                        ad.imageName
                        );
                    
                    textureDict.Add(filePath, LoadTexture(filePath));
                }
            }
        }

        private Texture2D LoadTexture(string file)
        {            
            Texture2D texture = new Texture2D((int)drawRect.width, (int)drawRect.height);
            texture.LoadImage(File.ReadAllBytes(file));         
            return texture;
        }

        private void NextAdvertisementIndex()
        {
            if (lastTimeSinceStartup == 0)
            {
                lastTimeSinceStartup = EditorApplication.timeSinceStartup;
            }
            elapseTime += EditorApplication.timeSinceStartup - lastTimeSinceStartup;
            lastTimeSinceStartup = EditorApplication.timeSinceStartup;

            if (elapseTime >= advertisements.intervalTime)
            {
                elapseTime = 0;

                advertisementIndex++;
                if (advertisementIndex > drawAdvertisementList.Count)
                {
                    advertisementIndex = 0;
                }

                advertisementIndex %= drawAdvertisementList.Count;

                MakeAdvertisementList();

                if (window != null)
                {
                    window.Repaint();
                }
            }
        }

        private Texture2D GetTexture(string key)
        {
            Texture2D texture;
            if (textureDict.TryGetValue(key, out texture) == false)
            {
                return null;
            }

            return texture;
        }

        private AdvertisementData GetAdvertisementData()
        {
            if(drawAdvertisementList == null || drawAdvertisementList.Count == 0)
            {
                return null;
            }

            if (drawAdvertisementList.Count <= advertisementIndex)
            {
                return null;
            }

            return drawAdvertisementList[advertisementIndex];
        }

        private void MakeAdvertisementList()
        {
            if(advertisements == null)
            {
                return;
            }

            drawAdvertisementList.Clear();
            foreach (var ad in advertisements.advertisements)
            {
                DateTime dateTimeStart = DateTime.ParseExact(ad.timeInfo.startTime, "yyyy-MM-dd HH:mm", null).AddHours(-KOREA_STANDARD_TIME);
                DateTime dateTimeEnd = DateTime.ParseExact(ad.timeInfo.endTime, "yyyy-MM-dd HH:mm", null).AddHours(-KOREA_STANDARD_TIME);

                int utcNowHour = (DateTime.UtcNow.Hour + KOREA_STANDARD_TIME) % 24;

                if (DateTime.UtcNow.Ticks >= dateTimeStart.Ticks
                    && DateTime.UtcNow.Ticks <= dateTimeEnd.Ticks)
                {
                    if (utcNowHour >= int.Parse(ad.timeInfo.day.start)
                        && utcNowHour <= int.Parse(ad.timeInfo.day.end))
                    {
                        string key = string.Format(
                        "{0}{1}{2}/{3}",
                        advertisementConfigurations.imageDownloadPath,
                        advertisements.imagePath,
                        languageCode,
                        ad.imageName
                        );

                        drawAdvertisementList.Add(
                            new AdvertisementData(
                                ad.name,
                                ad.imageName,
                                ad.link,
                                key
                                ));                               
                    }
                }
            }

            if(drawAdvertisementList.Count <= advertisementIndex)
            {
                advertisementIndex = 0;
            }
        }
    }
}
