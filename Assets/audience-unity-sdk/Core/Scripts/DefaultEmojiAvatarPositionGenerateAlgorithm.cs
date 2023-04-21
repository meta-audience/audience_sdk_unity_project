using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AudienceSDK
{
    // this algorithm is only suited for regular box collider as avatar area.
    // if developers want use unregular shape colliders as avatar area, create another algorithm
    // extend EmojiAvatarPositionGenerateAlgorithmBase to provide avatar positions.
    public class DefaultEmojiAvatarPositionGenerateAlgorithm : IEmojiAvatarPositionGenerateAlgorithmBase
    {
        private const string avatarAreaResourcesPath = "Audience/Avatar/";
        private const string avatarGenerateArea = "DefaultGenerateArea";
        private List<BoxCollider> _avatarGenerateColliders = new List<BoxCollider>();
        private GameObject _generateArea = null;

        public DefaultEmojiAvatarPositionGenerateAlgorithm()
        {
            Debug.Log("DefaultEmojiAvatarPositionGenerateAlgorithm constructor.");
            this._avatarGenerateColliders = new List<BoxCollider>();
        }

        ~DefaultEmojiAvatarPositionGenerateAlgorithm()
        {
            Debug.Log("DefaultEmojiAvatarPositionGenerateAlgorithm destructor.");
            this._avatarGenerateColliders = null;
            UnityEngine.Object.DestroyImmediate(this._generateArea);
        }

        public Vector3 GenerateAvatarPosition() {

            var avatarArea = this.GetGenerateArea();
            if (avatarArea == null) {
                Debug.LogError("Generate avatar position failed, area object isnull, return origin.");
                return Vector3.zero;
            }

            if (this._avatarGenerateColliders == null || this._avatarGenerateColliders.Count <= 0)
            {
                Debug.LogError("Generate avatar position failed, area collider is empty, return origin.");
                return Vector3.zero;
            }

            var randomListIndex = UnityEngine.Random.Range(0, this._avatarGenerateColliders.Count);
            var randomColliderInList = this._avatarGenerateColliders[randomListIndex];

            Vector3 extents = randomColliderInList.size / 2f;
            Vector3 randomPoint = new Vector3(
                UnityEngine.Random.Range(-extents.x, extents.x),
                UnityEngine.Random.Range(-extents.y, extents.y),
                UnityEngine.Random.Range(-extents.z, extents.z));

            return randomColliderInList.transform.position + (randomColliderInList.transform.rotation * randomPoint);
        }

        private GameObject GetGenerateArea()
        {
            if (this._generateArea == null)
            {
                /*
                 * audience-unity-sdk.csproj would define DLL_BUILD
                 * dll will load resources from embeded resources.
                 * AudienceSDK-Assembly won't define DLL_BUILD
                 * it will load resouces from Resources folder.
                 */
                // DefaultGenerateArea.prefab
                GameObject area = null;
#if DLL_BUILD
                var assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("AudienceSDK.Resources.Art.audience_sdk_art_resource");
                if (stream == null) {
                    Debug.LogError("Assembly get Manifest Resource Stream fail.");
                    return null;
                }

                var audienceSDKBundle = AssetBundle.LoadFromStream(stream);
                if (audienceSDKBundle == null) {
                    Debug.LogError("Stream load asset bundle fail.");
                    stream.Close();
                    return null;
                }

                area = audienceSDKBundle.LoadAsset<GameObject>("DefaultGenerateArea.prefab");
                if (area == null) {
                    Debug.LogError("Load Generate Area resource fail.");
                }

                audienceSDKBundle.Unload(false);
                stream.Close();
#else
                area = Resources.Load<GameObject>(avatarAreaResourcesPath + avatarGenerateArea);
                if (area == null) {
                    Debug.LogError("Load Generate Area resource fail.");
                }
#endif
                if (area != null)
                {
                    this._generateArea = GameObject.Instantiate(area);
                    this._generateArea.name = avatarGenerateArea;

                    this._avatarGenerateColliders.Clear();

                    var colliders = this._generateArea.GetComponentsInChildren<BoxCollider>();
                    foreach (BoxCollider col in colliders)
                    {
                        this._avatarGenerateColliders.Add(col);
                    }
                }
            }

            return this._generateArea;
        }
    }
}