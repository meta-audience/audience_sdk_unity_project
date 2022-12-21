using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AudienceSDK {
    // UserConfig should be renamed!!, it's not for user, it's for SDK class config.
    public static class UserConfig {
        public static bool IsUnityEnable360StereoCapture { get; set; } = false;

        public static int CubemapSize { get; set; } = -1;

        public static string DefaultCamAvatarShader { get; set; } = "audience/color";

        public static string DefaultEmojiShader { get; set; } = "audience/emoji";

        public static string DefaultPreviewQuadShader { get; set; } = "Particles/Standard Unlit";

        public static int EmojiLimitPerMessage { get; set; } = 20;

        public static int EmojiMaxAuthors { get; set; } = 500;

        public static int EmojiMaxSentencesInOneAuthor { get; set; } = 2;

        public static int EmojiMaxSceneEmojis { get; set; } = 1000;
        private static string _userConfigFileName = "audience_user_config.json";
        private static string _userConfigFilePath = "/Plugins/";
        public static void LoadUserConfig()
        {
            var configPath = Application.dataPath + _userConfigFilePath + _userConfigFileName;
            StreamReader reader = new StreamReader(configPath);

            if (reader != null)
            {
                var content = reader.ReadToEnd();
                DeserializeStaticClass(content, typeof(AudienceSDK.UserConfig));

                reader.Close();
            }
            else
            {
                Debug.LogError("Can't find " + _userConfigFileName);
            }
        }

        // A util function to for deserialize a static class, can move to other scripts if there's more util functions in the future
        private static void DeserializeStaticClass(string json, Type staticClassType)
        {
            if (!staticClassType.IsClass)
            {
                throw new ArgumentException("Type must be a class", nameof(staticClassType));
            }

            if (!staticClassType.IsAbstract || !staticClassType.IsSealed)
            {
                throw new ArgumentException("Type must be static", nameof(staticClassType));
            }

            var document = JObject.Parse(json);

            var propertyFields = staticClassType.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

            foreach (var field in propertyFields)
            {
                var documentField = document[field.Name];
                if (documentField == null)
                {
                    Debug.Log($"Redundant json key in file, ignore it: {field.Name}");
                }
                else
                {
                    field.SetValue(null, documentField.ToObject(field.PropertyType));
                }
            }
        }
    }
}
