using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace AudienceSDK {
    public class EmojiAvatarManager : MonoBehaviour {

        public float AvatarTotalLifeTime { get; private set; } = 15.0f;

        private const string avatarResourcesPath = "Audience/Avatar/";
        private const string avatarSingleKey = "single";
        private const string avatarSingleFileName = "Avatar_Single";
        private const string avatarMultipleKey = "multiple";
        private const string avatarMultipleFileName = "Avatar_Single";
        private const string prefabExtension = ".prefab";

        private Dictionary<string, GameObject> _emojiAvatarPrefabList;
        private LinkedList<EmojiAvatarBehaviourBase> _avatarList;

        private GameObject emojiAvatarsRoot = null;
        private Transform emojiLookAtTarget = null;
        private EmojiAvatarPositionGenerateAlgorithmBase emojiAvatarPositionGenerateAlgorithm;

        private float _avatarColliderRadius = 0.3f;
        private int _avatarColliderRetryTimes = 10;

        public AudienceReturnCode SetEmojiAvatarFollowTarget(Transform target) {
            if (this.emojiAvatarsRoot == this.gameObject) {
                this.CreateAvatarsRoot();
                this.MoveAvatarsToNewAvatarsRoot();
            }

            this.emojiAvatarsRoot.transform.SetParent(target, false);
            return AudienceReturnCode.AudienceSDKOk;
        }

        public AudienceReturnCode SetEmojiAvatarLookAtTarget(Transform target)
        {
            if (this.emojiLookAtTarget == this.transform) {
                this.CreateLookAtTarget();
            }

            this.emojiLookAtTarget.SetParent(target, false);
            foreach (EmojiAvatarBehaviourBase avatar in this._avatarList)
            {
                avatar.transform.LookAt(this.emojiLookAtTarget);
            }

            return AudienceReturnCode.AudienceSDKOk;
        }

        public AudienceReturnCode SetEmojiAvatarPositionGenerateAlgorithm(EmojiAvatarPositionGenerateAlgorithmBase algo) {
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

            this.emojiAvatarsRoot = this.gameObject;
            this.emojiLookAtTarget = this.transform;
            this.emojiAvatarPositionGenerateAlgorithm = new DefaultEmojiAvatarPositionGenerateAlgorithm();
        }

        private void Start() {
        }

        private void OnDestroy() {
            if (this.emojiAvatarsRoot != null) {
                var rootBehaviour = this.emojiAvatarsRoot.GetComponent<EmojiAvatarsRootBehaviour>();
                if (rootBehaviour != null) {
                    rootBehaviour.OnEmojiAvatarsRootDestroy -= this.OnEmojiAvatarsRootDestroy;
                }
            }

            if (this.emojiLookAtTarget != null) {
                var lookAtTargetBehaviour = this.emojiLookAtTarget.GetComponent<EmojiAvatarsLookAtTargetBehavior>();
                if (lookAtTargetBehaviour != null) {
                    lookAtTargetBehaviour.OnEmojiAvatarsLookAtTargetDestroy -= this.OnEmojiAvatarsLookAtTargetDestroy;
                }
            }
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

            if (this.emojiAvatarsRoot == null)
            {
                Debug.LogError("emojiAvatarsRoot is null, create a new one while last one had been destroyed.");
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
                    avatarObject.transform.SetParent(this.emojiAvatarsRoot.transform);
                    avatarObject.transform.position = avatarPositon;
                    avatarObject.transform.LookAt(this.emojiLookAtTarget);
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

            for (int i = 0; i < this._avatarColliderRetryTimes; ++i) {

                // use algorithm to get relative position
                var relativePos = this.emojiAvatarPositionGenerateAlgorithm.GenerateAvatarPosition();

                // compute world coordinate to check overlap with other avatar.
                var candidatePosition = this.emojiAvatarsRoot.transform.position + this.emojiAvatarsRoot.transform.rotation * relativePos;
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
            avatarPos = Vector3.zero;
            return AudienceReturnCode.AudienceSDKInternalError;
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

        private void CreateAvatarsRoot() {
            this.emojiAvatarsRoot = new GameObject("EmojiAvatarsRoot");

            var rootBehaviour = this.emojiAvatarsRoot.AddComponent<EmojiAvatarsRootBehaviour>();
            rootBehaviour.OnEmojiAvatarsRootDestroy += this.OnEmojiAvatarsRootDestroy;
        }

        private void CreateLookAtTarget()
        {
            var lookAtTarget = new GameObject("EmojiAvatarsLookAtTarget");
            this.emojiLookAtTarget = lookAtTarget.transform;

            var lookAtTargetBehaviour = lookAtTarget.AddComponent<EmojiAvatarsLookAtTargetBehavior>();
            lookAtTargetBehaviour.OnEmojiAvatarsLookAtTargetDestroy += this.OnEmojiAvatarsLookAtTargetDestroy;
        }

        private void MoveAvatarsToNewAvatarsRoot()
        {
            foreach (EmojiAvatarBehaviourBase avatar in this._avatarList)
            {
                avatar.transform.SetParent(this.emojiAvatarsRoot.transform, false);
            }
        }

        private void AvatarsLookAtNewTarget()
        {
            foreach (EmojiAvatarBehaviourBase avatar in this._avatarList)
            {
                avatar.transform.LookAt(this.emojiLookAtTarget);
            }
        }

        private void HandleAvatarFinished(EmojiAvatarBehaviourBase avatar) {
            if (avatar != null && this._avatarList != null && this._avatarList.Contains(avatar)) {
                this._avatarList.Remove(avatar);
            }
        }

        private void OnEmojiAvatarsRootDestroy() {
            this.emojiAvatarsRoot = this.gameObject;
            this.MoveAvatarsToNewAvatarsRoot();
        }

        private void OnEmojiAvatarsLookAtTargetDestroy()
        {
            this.emojiLookAtTarget = this.transform;
            this.AvatarsLookAtNewTarget();
        }
    }
}
