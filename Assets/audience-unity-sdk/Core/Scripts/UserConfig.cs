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

        public static string ReplacedEmoji2DShader { get; set; } = null;

        public static string DefaultEmojiShader { get; set; } = "audience/emoji";

        public static int EmojiLimitPerMessage { get; set; } = 20;

        public static int EmojiMaxAuthors { get; set; } = 500;

        public static int EmojiMaxSentencesInOneAuthor { get; set; } = 2;

        public static int EmojiMaxSceneEmojis { get; set; } = 1000;

#if DLL_BUILD
        private static string _userConfigFileName = "audience_user_config.json";
        private static string _userConfigEmbededResourcePath = "AudienceSDK.Resources.Config.audience_sdk_config_resource";

#else
        private static string _userConfigResourcePath = "Audience/Config/audience_user_config";
#endif
        public static void LoadUserConfig()
        {
            /*
            * audience-unity-sdk.csproj would define DLL_BUILD
            * dll will load resources from embeded resources.
            * AudienceSDK-Assembly won't define DLL_BUILD
            * it will load resouces from Resources folder.
            */
#if DLL_BUILD
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(_userConfigEmbededResourcePath);
            var audienceConfigResource = AssetBundle.LoadFromStream(stream);
            var userConfigText = audienceConfigResource.LoadAsset<TextAsset>(_userConfigFileName);

            audienceConfigResource.Unload(false);
            stream.Close();
#else
            var userConfigText = Resources.Load<TextAsset>(_userConfigResourcePath);
#endif
            if (userConfigText != null)
            {
                DeserializeStaticClass(userConfigText.text, typeof(AudienceSDK.UserConfig));
            }
            else
            {
                Debug.LogError("Can't find config resource");
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
