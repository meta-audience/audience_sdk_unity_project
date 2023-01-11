using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK
{
    // this algorithm is only suited for regular box collider as avatar area.
    // if developers want use unregular shape colliders as avatar area, create another algorithm
    // extend EmojiAvatarPositionGenerateAlgorithmBase to provide avatar positions.
    public class DefaultEmojiAvatarPositionGenerateAlgorithm : EmojiAvatarPositionGenerateAlgorithmBase
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

        public AudienceReturnCode GenerateAvatarPosition(ref Vector3 relativeAvatarPositon) {

            var avatarArea = this.GetGenerateArea();
            if (avatarArea == null) {
                relativeAvatarPositon = Vector3.zero;
                Debug.LogError("Generate avatar position failed, area object isnull, return origin.");
                return AudienceReturnCode.AudienceSDKNullPtr;
            }

            if (this._avatarGenerateColliders == null || this._avatarGenerateColliders.Count <= 0)
            {
                relativeAvatarPositon = Vector3.zero;
                Debug.LogError("Generate avatar position failed, area collider is empty, return origin.");
                return AudienceReturnCode.AudienceSDKNullPtr;
            }

            var randomListIndex = UnityEngine.Random.Range(0, _avatarGenerateColliders.Count);
            var randomColliderInList = _avatarGenerateColliders[randomListIndex];

            Vector3 extents = randomColliderInList.size / 2f;
            Vector3 randomPoint = new Vector3(
                UnityEngine.Random.Range(-extents.x, extents.x),
                UnityEngine.Random.Range(-extents.y, extents.y),
                UnityEngine.Random.Range(-extents.z, extents.z)
                );

            relativeAvatarPositon = randomColliderInList.transform.position + randomPoint;
            return AudienceReturnCode.AudienceSDKOk;
        }

        private GameObject GetGenerateArea()
        {
            if (this._generateArea == null)
            {
                var area = Resources.Load<GameObject>(avatarAreaResourcesPath + avatarGenerateArea);
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