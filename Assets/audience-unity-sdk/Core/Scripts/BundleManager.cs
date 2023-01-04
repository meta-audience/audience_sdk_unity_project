using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AudienceSDK {
    public class BundleManager : MonoBehaviour {
        private Dictionary<EmojiType, GameObject> _emojiList;

        public void ShowDefaultEmoji(EmojiType type) {
            switch (type) {
                case EmojiType.Heart:
                case EmojiType.Balloon:
                case EmojiType.Star:
                case EmojiType.Candy:
                case EmojiType.Smile:
                    var emojiModel = this._emojiList[type];
                    Debug.Log(emojiModel == null);
                    if (emojiModel != null) {
                        GameObject heart = GameObject.Instantiate(emojiModel);
                        DefaultEmojiBehaviour emojiBehaviour = heart.AddComponent<DefaultEmojiBehaviour>();
                    }

                    break;
                default:
                    break;
            }
        }

        public GameObject GetEmoji(EmojiType type) {
            var emojiModel = this._emojiList[type];
            return emojiModel;
        }

        private void Awake() {
            this._emojiList = new Dictionary<EmojiType, GameObject>();
            this.PreloadEmojiModel();
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }

        private void PreloadEmojiModel() {
            /*
             * audience-unity-sdk.csproj would define DLL_BUILD
             * dll will load resources from embeded resources.
             * AudienceSDK-Assembly won't define DLL_BUILD
             * it will load resouces from Resources folder.
             */
#if DLL_BUILD
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("AudienceSDK.Resources.Art.audience_sdk_art_resource");
            var audienceSDKBundle = AssetBundle.LoadFromStream(stream);

            var heartPrefab = audienceSDKBundle.LoadAsset<GameObject>("EMOJI_01.prefab");
            this._emojiList.Add(EmojiType.Heart, heartPrefab);

            var ballonPrefab = audienceSDKBundle.LoadAsset<GameObject>("EMOJI_02.prefab");
            this._emojiList.Add(EmojiType.Balloon, ballonPrefab);

            var starPrefab = audienceSDKBundle.LoadAsset<GameObject>("EMOJI_03.prefab");
            this._emojiList.Add(EmojiType.Star, starPrefab);

            var candyPrefab = audienceSDKBundle.LoadAsset<GameObject>("EMOJI_04.prefab");
            this._emojiList.Add(EmojiType.Candy, candyPrefab);

            var smilePrefab = audienceSDKBundle.LoadAsset<GameObject>("EMOJI_05.prefab");
            this._emojiList.Add(EmojiType.Smile, smilePrefab);

            audienceSDKBundle.Unload(false);
            stream.Close();
#else
            var heartPrefab = Resources.Load<GameObject>("Audience/Emoji/EMOJI_01");
            this._emojiList.Add(EmojiType.Heart, heartPrefab);

            var ballonPrefab = Resources.Load<GameObject>("Audience/Emoji/EMOJI_02");
            this._emojiList.Add(EmojiType.Balloon, ballonPrefab);

            var starPrefab = Resources.Load<GameObject>("Audience/Emoji/EMOJI_03");
            this._emojiList.Add(EmojiType.Star, starPrefab);

            var candyPrefab = Resources.Load<GameObject>("Audience/Emoji/EMOJI_04");
            this._emojiList.Add(EmojiType.Candy, candyPrefab);

            var smilePrefab = Resources.Load<GameObject>("Audience/Emoji/EMOJI_05");
            this._emojiList.Add(EmojiType.Smile, smilePrefab);
#endif
        }
    }
}
