using System;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class PlatformData
    {
        public string name;
        public string displayName;
        public string buildTargetName;
        public InstallInfo install;

        public class RunEvent
        {
            public const string CHECK_TYPE = "check_type";
            public const string IS_OVER_VALUE = "is_over_value";
            public const string RUN = "run";
            public const string SWITCH_RUN = "switch_run";

            public string type;
            public string typeName;
            public string methodName;
            public object[] parameters;
            public string value;
            public string message;

            public bool IsNeedSwitchPlatform()
            {
                if (RunEvent.SWITCH_RUN.Equals(type) &&
                    string.IsNullOrEmpty(typeName) == false &&
                    string.IsNullOrEmpty(methodName) == false)
                {
                    return true;
                }

                return false;
            }
        }

        public RunEvent[] runEvents;

        public bool IsNeedCheck()
        {
            if (runEvents != null)
            {
                foreach (var runEvent in runEvents)
                {
                    if (RunEvent.CHECK_TYPE.Equals(runEvent.type))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Check()
        {
            if(runEvents != null)
            {
                foreach (var runEvent in runEvents)
                {
                    if (RunEvent.CHECK_TYPE.Equals(runEvent.type))
                    {
                        if(Type.GetType(runEvent.typeName) == null)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        

        public bool IsNeedSwitchPlatform()
        {
            if (runEvents != null)
            {
                foreach (var runEvent in runEvents)
                {
                    if (runEvent.IsNeedSwitchPlatform())
                    {
                        if (name.Equals(UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString()) == false)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool CheckCanRun()
        {
            if (runEvents != null)
            {
                foreach (var runEvent in runEvents)
                {
                    if (RunEvent.IS_OVER_VALUE.Equals(runEvent.type))
                    {
                        int value = 0;
                        if (string.IsNullOrEmpty(runEvent.typeName) == false &&
                            string.IsNullOrEmpty(runEvent.methodName) == false &&
                            string.IsNullOrEmpty(runEvent.value) == false &&
                            int.TryParse(runEvent.value, out value))
                        {
                            var type = Type.GetType(runEvent.typeName);
                            if (type != null)
                            {
                                var method = type.GetMethod(runEvent.methodName,
                                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                                if (method != null)
                                {
                                    object[] parametersArray = null;

                                    if (runEvent.parameters == null)
                                    {
                                        parametersArray = new object[] { };
                                    }
                                    else
                                    {
                                        parametersArray = runEvent.parameters;
                                    }

                                    int result = 0;
                                    if (int.TryParse(method.Invoke(null, parametersArray).ToString(), out result))
                                    {
                                        if (value > result)
                                        {
                                            if (string.IsNullOrEmpty(runEvent.message) == false)
                                            {
                                                if (EditorUtility.DisplayDialog(
                                                        Multilanguage.GetString("POPUP_SETTING_TITLE"),
                                                        Multilanguage.GetString(runEvent.message),
                                                        Multilanguage.GetString("POPUP_OK")) == true)
                                                {
                                                }
                                            }

                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        
        public bool Run()
        {
            if (runEvents != null)
            {
                foreach (var runEvent in runEvents)
                {
                    if (RunEvent.RUN.Equals(runEvent.type) &&
                        string.IsNullOrEmpty(runEvent.typeName) == false &&
                        string.IsNullOrEmpty(runEvent.methodName) == false)
                    {
                        var type = Type.GetType(runEvent.typeName);
                        if (type != null)
                        {
                            var method = type.GetMethod(runEvent.methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                            if (method != null)
                            {
                                object[] parametersArray = null;

                                if (runEvent.parameters == null)
                                {
                                    parametersArray = new object[] { };
                                }
                                else
                                {
                                    parametersArray = runEvent.parameters;
                                }
                                method.Invoke(null, parametersArray);
                            }
                        }
                    }
                }
            }

            return true;
        }

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(displayName) == false)
            {
                return Multilanguage.GetString(displayName);
            }

            return name;
        }
    }
}