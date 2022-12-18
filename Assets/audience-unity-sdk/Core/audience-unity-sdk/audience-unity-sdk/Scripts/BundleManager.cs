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
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("AudienceSDK.audience_sdk");
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
        }
    }
}
