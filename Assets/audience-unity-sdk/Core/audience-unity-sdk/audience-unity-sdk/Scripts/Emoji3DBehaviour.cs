using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace AudienceSDK {
    public class Emoji3DBehaviour : EmojiBehaviourBase {

        protected override IEnumerator LoadEmojiModel(string keyword, object asset) {

            // Type check
            if (!(asset is EmojiAsset)) {
                Debug.LogWarning("ShowModelFromUrl Failed to cast asset to EmojiAsset");
                UnityEngine.Object.DestroyImmediate(this.gameObject);
                yield break;
            }

            var assetObject = (EmojiAsset)asset;
            var prefab = AssetLoader.GetCachedPrefab(assetObject.keyword, assetObject.version);
            if (prefab == null) {
                AssetLoader.LoadedAssetBundle data = AssetLoader.LoadAssetBundle(assetObject.url);
                while (!data.Finished) {
                    yield return null;
                }

                if (data.Result != AudienceReturnCode.AudienceSDKOk) {
                    Debug.LogWarning("ShowModelFromUrl Failed to load Asset data");
                    UnityEngine.Object.DestroyImmediate(this.gameObject);
                    yield break;
                }

                if (data.AssetBundle == null) {
                    Debug.LogWarning("ShowModelFromUrl Failed to download Asset bundle data");
                    UnityEngine.Object.DestroyImmediate(this.gameObject);
                    yield break;
                }

                prefab = data.AssetBundle.LoadAsset<GameObject>(assetObject.keyword + ".prefab");
                if (prefab == null) {
                    Debug.LogWarning(" ShowModelFromUrl missing prefab.");
                    UnityEngine.Object.DestroyImmediate(this.gameObject);
                    yield break;
                }

                AssetLoader.SetCachedPrefab(assetObject.keyword, assetObject.version, prefab);
                data.AssetBundle.Unload(false);
            }

            this._emojiModel = Instantiate(prefab, this.transform);
            this._emojiModel.transform.localEulerAngles = Vector3.zero;
            this._emojiModel.transform.localPosition = Vector3.zero;
            this._emojiModel.AddComponent<Animation>();

            yield return null;
        }
    }
}
