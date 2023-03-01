using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace AudienceSDK {
    public class EmojiAvatarManager : MonoBehaviour {

        public float AvatarTotalLifeTime { get; private set; } = 15.0f;

        public GameObject EmojiAvatarsRoot {
            get {
                return this._emojiAvatarsRoot;
            }
        }

        public GameObject EmojiAvatarsLookAtTarget {
            get {
                return this._emojiLookAtTarget;
            }
        }

        private const string avatarResourcesPath = "Audience/Avatar/";
        private const string avatarSingleKey = "single";
        private const string avatarSingleFileName = "Avatar_Single";
        private const string avatarMultipleKey = "multiple";
        private const string avatarMultipleFileName = "Avatar_Single";
        private const string prefabExtension = ".prefab";

        private Dictionary<string, GameObject> _emojiAvatarPrefabList;
        private LinkedList<EmojiAvatarBehaviourBase> _avatarList;

        private GameObject _emojiAvatarsRoot = null;
        private EmojiAvatarsRootBehaviour _rootBehaviour = null;
        private GameObject _emojiLookAtTarget = null;
        private EmojiAvatarsLookAtTargetBehavior _lookAtTargetBehaviour = null;
        private IEmojiAvatarPositionGenerateAlgorithmBase emojiAvatarPositionGenerateAlgorithm;

        private float _avatarColliderRadius = 0.3f;
        private int _avatarColliderRetryTimes = 10;

        public AudienceReturnCode SetEmojiAvatarFollowTarget(Transform target) {
            this._rootBehaviour.FollowTarget = target;
            return AudienceReturnCode.AudienceSDKOk;
        }

        public AudienceReturnCode SetEmojiAvatarLookAtTarget(Transform target)
        {
            this._lookAtTargetBehaviour.LookAtTarget = target;
            foreach (EmojiAvatarBehaviourBase avatar in this._avatarList)
            {
                avatar.transform.LookAt(this._emojiLookAtTarget.transform);
            }

            return AudienceReturnCode.AudienceSDKOk;
        }

        public AudienceReturnCode SetEmojiAvatarPositionGenerateAlgorithm(IEmojiAvatarPositionGenerateAlgorithmBase algo) {
            this.emojiAvatarPositionGenerateAlgorithm = algo;
            return AudienceReturnCode.AudienceSDKOk;
        }

        internal AudienceReturnCode GetAvatar(ChatAuthor author, ref EmojiAvatarBehaviourBase avatar) {
            var targetAuthors = new List<ChatAuthor>();
            targetAuthors.Add(author);

            var rc = this.FindExistAvatars(targetAuthors, ref avatar);
            if (rc != AudienceReturnCode.AudienceSDKOk) {
                return rc;
            }

            if (avatar == null) {
                rc = this.CreateAvatar(targetAuthors, ref avatar);
                if (rc != AudienceReturnCode.AudienceSDKOk) {
                    return rc;
                }
            }

            if (avatar == null) {
                Debug.LogError("CreateEmoji Fail, avatar access fail.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            return AudienceReturnCode.AudienceSDKOk;
        }

        internal AudienceReturnCode RearrangeAvatarList(EmojiAvatarBehaviourBase avatar) {
            if (this._avatarList == null) {
                Debug.LogError("RearrangeAvatarList error, class Not Inited.");
                return AudienceReturnCode.AudienceSDKNotInited;
            }

            if (avatar == null || !this._avatarList.Contains(avatar)) {
                Debug.LogError("RearrangeAvatarList error, RearrangeAvatarList but not found.");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            this._avatarList.Remove(avatar);
            this._avatarList.AddLast(avatar);
            return AudienceReturnCode.AudienceSDKOk;
        }

        private void Awake() {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this._avatarList = new LinkedList<EmojiAvatarBehaviourBase>();
            this._emojiAvatarPrefabList = new Dictionary<string, GameObject>();
            this.PreloadEmojiAvatar();

            this._emojiAvatarsRoot = new GameObject("EmojiAvatarsRoot");
            this._emojiAvatarsRoot.transform.SetParent(this.transform);
            this._rootBehaviour = this._emojiAvatarsRoot.AddComponent<EmojiAvatarsRootBehaviour>();

            this._emojiLookAtTarget = new GameObject("EmojiAvatarLookAtTarget");
            this._emojiLookAtTarget.transform.SetParent(this.transform);
            this._lookAtTargetBehaviour = this._emojiLookAtTarget.AddComponent<EmojiAvatarsLookAtTargetBehavior>();

            this.emojiAvatarPositionGenerateAlgorithm = new DefaultEmojiAvatarPositionGenerateAlgorithm();
        }

        private void Start() {
        }

        private void OnDestroy() {
        }

        private AudienceReturnCode FindExistAvatars(List<ChatAuthor> targetAuthors, ref EmojiAvatarBehaviourBase emojiAvatar) {
            if (this._avatarList == null) {
                Debug.LogError("FindExistAvatars fail, list not init.");
                return AudienceReturnCode.AudienceSDKNotInited;
            }

            if (targetAuthors == null) {
                Debug.LogError("FindExistAvatars fail, targetAuthors is null.");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            foreach (EmojiAvatarBehaviourBase avatar in this._avatarList) {
                if (avatar == null || !avatar.IsAlive()) {
                    continue;
                }

                var avatarAuthors = new List<ChatAuthor>(avatar.GetAvatarAuthors());

                if (avatarAuthors.Count != targetAuthors.Count || avatarAuthors.Count == 0) {
                    continue;
                }

                foreach (ChatAuthor targetAuthor in targetAuthors) {
                    var matchedAuthor = avatarAuthors.Find(x => x.platform == targetAuthor.platform && x.user_id == targetAuthor.user_id);
                    if (matchedAuthor != null) {
                        avatarAuthors.Remove(matchedAuthor);
                    }
                }

                if (avatarAuthors.Count == 0) {
                    emojiAvatar = avatar;
                    return AudienceReturnCode.AudienceSDKOk;
                }
            }

            return AudienceReturnCode.AudienceSDKOk;
        }

        private AudienceReturnCode CreateAvatar(List<ChatAuthor> avatarAuthors, ref EmojiAvatarBehaviourBase avatar) {
            var rc = AudienceReturnCode.AudienceSDKOk;

            if (this._emojiAvatarPrefabList == null || this._emojiAvatarPrefabList.Count <= 0) {
                Debug.LogError("CreateAvatar fail, emojiAvatarPrefabList is empty");
                return AudienceReturnCode.AudienceSDKNotInited;
            }

            if (this._emojiAvatarsRoot == null)
            {
                Debug.LogError("_emojiAvatarsRoot is null, init incomplete.");
                return AudienceReturnCode.AudienceSDKNotInited;
            }

            if (this._emojiLookAtTarget == null)
            {
                Debug.LogError("_emojiLookAtTarget is null, init incomplete.");
                return AudienceReturnCode.AudienceSDKNotInited;
            }

            if (this.emojiAvatarPositionGenerateAlgorithm == null)
            {
                Debug.LogError("CreateAvatar fail, emojiAvatarPositionGenerateAlgorithm not assigned.");
                return AudienceReturnCode.AudienceSDKNotInited;
            }

            var avatarPositon = Vector3.zero;
            rc = this.GenerateAvatarPosition(out avatarPositon);
            if (rc != AudienceReturnCode.AudienceSDKOk) {
                return rc;
            }

            if (avatarAuthors == null || avatarAuthors.Count <= 0) {
                Debug.LogError("CreateAvatar fail, avatarAuthors is empty");
                return AudienceReturnCode.AudienceInvalidParams;
            } else if (avatarAuthors.Count == 1) {
                if (this._emojiAvatarPrefabList.ContainsKey(avatarSingleKey) && this._emojiAvatarPrefabList[avatarSingleKey] != null) {
                    var avatarObject = Instantiate(this._emojiAvatarPrefabList[avatarSingleKey]);
                    avatarObject.transform.SetParent(this._emojiAvatarsRoot.transform);
                    avatarObject.transform.position = avatarPositon;
                    avatarObject.transform.LookAt(this._emojiLookAtTarget.transform);
                    var avatarCollider = avatarObject.AddComponent<SphereCollider>();
                    avatarCollider.radius = this._avatarColliderRadius;
                    var avatarBehavior = avatarObject.AddComponent<EmojiAvatarSingleAuthorBehaviour>();
                    avatarBehavior.OnAvatarFinished += this.HandleAvatarFinished;
                    rc = avatarBehavior.SetAuthors(avatarAuthors);
                    if (rc != AudienceReturnCode.AudienceSDKOk) {
                        UnityEngine.Object.DestroyImmediate(avatarObject);
                        return rc;
                    }

                    rc = avatarBehavior.Init();
                    if (rc != AudienceReturnCode.AudienceSDKOk) {
                        UnityEngine.Object.DestroyImmediate(avatarObject);
                        return rc;
                    }

                    avatar = avatarBehavior;
                    this._avatarList.AddLast(avatarBehavior);
                    return AudienceReturnCode.AudienceSDKOk;
                } else {
                    Debug.LogError("CreateAvatar fail, target AvatarPrefab not found");
                    return AudienceReturnCode.AudienceSDKMapKeyNotFound;
                }
            } else {
                if (this._emojiAvatarPrefabList.ContainsKey(avatarMultipleKey) && this._emojiAvatarPrefabList[avatarMultipleKey] != null) {
                    // TODO this sprint interact not support
                    // Instantiate avatar, add behavior
                    // set authors and init behavior.
                    Debug.LogError("CreateEmoji Fail, interact emoji not support.");
                    return AudienceReturnCode.AudienceSDKInternalError;
                } else {
                    Debug.LogError("CreateAvatar fail, target AvatarPrefab not found");
                    return AudienceReturnCode.AudienceSDKMapKeyNotFound;
                }
            }
        }

        private AudienceReturnCode GenerateAvatarPosition(out Vector3 avatarPos) {

            var candidatePosition = Vector3.zero;
            for (int i = 0; i < this._avatarColliderRetryTimes; ++i) {

                // use algorithm to get relative position
                var relativePos = this.emojiAvatarPositionGenerateAlgorithm.GenerateAvatarPosition();

                // compute world coordinate to check overlap with other avatar.
                candidatePosition = this._emojiAvatarsRoot.transform.position +
                    (this._emojiAvatarsRoot.transform.rotation * relativePos);
                if (!Physics.CheckSphere(candidatePosition, this._avatarColliderRadius)) {
                    avatarPos = candidatePosition;
                    return AudienceReturnCode.AudienceSDKOk;
                }
            }

            if (this._avatarList != null && this._avatarList.Count > 0) {
                Debug.Log("GenerateAvatarPosition, retry too many times, use the oldest avatar position.");
                var oldestAvatar = this._avatarList.First.Value;
                avatarPos = oldestAvatar.transform.position;
                this._avatarList.RemoveFirst();
                UnityEngine.Object.DestroyImmediate(oldestAvatar.gameObject);
                return AudienceReturnCode.AudienceSDKOk;
            }

            Debug.LogWarning("GenerateAvatarPosition fail, no position for new coming avatar.");
            avatarPos = candidatePosition;
            return AudienceReturnCode.AudienceSDKOk;
        }

        private void PreloadEmojiAvatar() {
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

            var avatarSingle = audienceSDKBundle.LoadAsset<GameObject>(avatarSingleFileName + prefabExtension);
            this._emojiAvatarPrefabList.Add(avatarSingleKey, avatarSingle);

            var avatarMultiple = audienceSDKBundle.LoadAsset<GameObject>(avatarMultipleFileName + prefabExtension);
            this._emojiAvatarPrefabList.Add(avatarMultipleKey, avatarMultiple);

            audienceSDKBundle.Unload(false);
            stream.Close();
#else
            var avatarSingle = Resources.Load<GameObject>(avatarResourcesPath + avatarSingleFileName);
            this._emojiAvatarPrefabList.Add(avatarSingleKey, avatarSingle);

            var avatarMultiple = Resources.Load<GameObject>(avatarResourcesPath + avatarMultipleFileName);
            this._emojiAvatarPrefabList.Add(avatarMultipleKey, avatarMultiple);
#endif
        }

        private void HandleAvatarFinished(EmojiAvatarBehaviourBase avatar) {
            if (avatar != null && this._avatarList != null && this._avatarList.Contains(avatar)) {
                this._avatarList.Remove(avatar);
            }
        }
    }
}
