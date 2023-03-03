using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;

namespace AudienceSDK {
    public class EmojiEffectManager : MonoBehaviour {

        private class EmojiInteractUnit {
            // TODO this sprint not support interact
            // need define - keyword, pairKeyword, emojiAuthors, behavior, emojiInteract
        }

        private List<EmojiInteractUnit> _interactList;
        private List<EmojiBehaviourBase> _emojiList;
        private bool _isEmojiProcessing = false;
        private bool _isEmojiAttachEmojiSpawner = false;
        private Material _shared2DMaterial = null;

        public AudienceReturnCode SetIsEmojiAttachEmojiSpawner(bool value) {
            this._isEmojiAttachEmojiSpawner = value;
            return AudienceReturnCode.AudienceSDKOk;
        }

        public void ReplaceEmojiShared2DMaterial(Material material) {
            if (material == null)
            {
                Debug.LogError("Input material is null.");
                return;
            }

            this._shared2DMaterial = material;
        }

        public AudienceReturnCode CreateEmoji(ChatAuthor author, EmojiMessage message) {
            EmojiAvatarBehaviourBase avatar = null;
            EmojiInteractUnit pairedInteractUnit = null;
            if (this._emojiList.Count >= UserConfig.EmojiMaxSceneEmojis) {
                Debug.Log("[Info][Emoji]CreateEmoji Fail, emoji out of limit.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            if (message.asset_list == null && message.url == null) {
                Debug.LogError("[Error][Emoji]CreateEmoji Fail, emoji asset list and url are empty.");
                return AudienceReturnCode.AudienceSDKNetworkDBDataError;
            }

            if (message.animation == null || message.animation.asset_list == null ||
                message.animation.asset_list.Count <= 0) {
                Debug.LogError("[Error][Emoji]CreateEmoji Fail, emoji animation list is empty.");
                return AudienceReturnCode.AudienceSDKNetworkDBDataError;
            }

            var rc = this.FindPairedInteract(message.text, ref pairedInteractUnit);
            if (rc != AudienceReturnCode.AudienceSDKOk) {
                return rc;
            }

            if (pairedInteractUnit != null) {

                // TODO: sprint not support interact
                // confirm pairedInteractUnit.emojiInteract.animations.Count must > 0
                // confirm spawner authors: interact author + current emoji author
                // use spawner authors to find correspond spawner or create one
                // decide animation
                // fulfill Emoji Info
                Debug.LogWarning("[Emoji]CreateEmoji Fail, interact emoji not support.");
                return AudienceReturnCode.AudienceSDKInternalError;
            } else {

                rc = Audience.Context.EmojiAvatarManager.GetAvatar(author, ref avatar);
                if (rc != AudienceReturnCode.AudienceSDKOk || avatar == null) {
                    Debug.LogError("[Error][Emoji]CreateEmoji Fail, spawner access fail.");
                    return AudienceReturnCode.AudienceSDKInternalError;
                }
            }

            var emojiKeyword = message.text;
            object emojiAsset = null;
            EmojiAsset emojiAnimationAsset = null;

            if (message.asset_list != null && message.asset_list.Count > 0) {
                var fittedObj = message.asset_list.Find(x => x.engine == "Unity" && x.render_type == XRSettings.stereoRenderingMode.ToString());
                if (fittedObj != null) {
                    emojiAsset = fittedObj.asset;
                } else {
                    // if can't find correct render_type, just find correct engine.
                    fittedObj = message.asset_list.Find(x => x.engine == "Unity");
                    if (fittedObj != null) {
                        emojiAsset = fittedObj.asset;
                    }
                }
            } else {
                emojiAsset = message.url;
            }

            if (emojiAsset == null) {
                Debug.LogError("[Error][Emoji]CreateEmoji Fail, emoji asset list and url are empty.");
                return AudienceReturnCode.AudienceSDKNetworkDBDataError;
            }

            var fittedAnimaObj =
                    message.animation.asset_list.Find(x => x.engine == "Unity" && x.render_type == XRSettings.stereoRenderingMode.ToString());
            if (fittedAnimaObj != null) {
                emojiAnimationAsset = fittedAnimaObj.asset;
            } else {
                // if can't find correct render_type, just find correct engine.
                fittedAnimaObj = message.animation.asset_list.Find(x => x.engine == "Unity");
                if (fittedAnimaObj != null) {
                    emojiAnimationAsset = fittedAnimaObj.asset;
                }
            }

            if (emojiAnimationAsset == null) {
                Debug.LogError("[Error][Emoji]CreateEmoji Fail, no suited animation.");
                return AudienceReturnCode.AudienceSDKNetworkDBDataError;
            }

            EmojiBehaviourBase eBehaviour = null;
            if (message is Emoji2DMessage) {
                eBehaviour = this.CreateChatMessageBehaviourObject<Emoji2DBehaviour>(avatar);
            } else if (message is Emoji3DMessage) {
                eBehaviour = this.CreateChatMessageBehaviourObject<Emoji3DBehaviour>(avatar);
            } else {
                Debug.LogError("[Error][Emoji]CreateEmoji Fail, Invalid message type.");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            if (eBehaviour != null) {
                eBehaviour.SetBehaviorAsset(emojiKeyword, emojiAsset, emojiAnimationAsset);
                eBehaviour.OnBehaviourFinished += this.HandleBehaviourFinished;
                rc = avatar.SpawnEmoji();
                if (rc != AudienceReturnCode.AudienceSDKOk) {
                    UnityEngine.Object.DestroyImmediate(eBehaviour.gameObject);
                    return rc;
                }

                this._emojiList.Add(eBehaviour);
                return AudienceReturnCode.AudienceSDKOk;
            } else {
                Debug.LogError("[Error][Emoji]CreateEmoji Fail, EmojiEffectManager didn't create anything");
                return AudienceReturnCode.AudienceSDKInternalError;
            }
        }

        public Material GetShared2DMaterial() {
            if (this._shared2DMaterial == null)
            {
                /*
                 * audience-unity-sdk.csproj would define DLL_BUILD
                 * dll will load resources from embeded resources.
                 * AudienceSDK-Assembly won't define DLL_BUILD
                 * it will load resouces from Resources folder.
                 */
                Material mat = null;
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

                var resourceMaterial = audienceSDKBundle.LoadAsset<Material>("2Demoji_origin.mat");
                if (resourceMaterial == null) {
                    Debug.LogError("Load meterial resource fail.");
                    audienceSDKBundle.Unload(false);
                    stream.Close();
                    return null;
                }

                mat = new Material(resourceMaterial);
                audienceSDKBundle.Unload(false);
                stream.Close();
#else
                var resourceMaterial = Resources.Load<Material>("Audience/Emoji/2Demoji_origin");
                if (resourceMaterial == null) {
                    Debug.LogError("Load meterial resource fail.");
                    return null;
                }

                mat = new Material(resourceMaterial);
#endif
                this._shared2DMaterial = mat;
            }

            return this._shared2DMaterial;
        }

        private void Awake() {
            Debug.Log(XRSettings.stereoRenderingMode);
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this._interactList = new List<EmojiInteractUnit>();
            this._emojiList = new List<EmojiBehaviourBase>();
        }

        private void Start() {
            this._isEmojiProcessing = true;
            this.StartCoroutine(this.ProcessEmojis());
        }

        private void OnDestroy() {
            this._isEmojiProcessing = false;
        }

        private T CreateChatMessageBehaviourObject<T>(EmojiAvatarBehaviourBase avatar)
            where T : EmojiBehaviourBase {
            GameObject go = new GameObject();
            T eBehaviour = go.AddComponent<T>();
            go.transform.SetParent(avatar.transform, false);

            // this.transform or EmojiAvatarsRoot is don't destroy, emoji won't destroy when scene unloaded.
            if (this._isEmojiAttachEmojiSpawner) {
                if (Audience.Context.EmojiAvatarManager.EmojiAvatarsRoot != null) {
                    go.transform.SetParent(Audience.Context.EmojiAvatarManager.EmojiAvatarsRoot.transform);
                } else {
                    Debug.LogWarning("EmojiAvatarsRoot should exist, check EmojiAvatarManager is inited.");
                    go.transform.SetParent(this.transform);
                }
            } else {
                go.transform.SetParent(this.transform);
            }

            return eBehaviour;
        }

        private AudienceReturnCode FindPairedInteract(string keyword, ref EmojiInteractUnit interactUnit) {
            // TODO sprint not support interact
            // confirm _interactList not null
            // _interactList find pairKeyword.
            interactUnit = null;
            return AudienceReturnCode.AudienceSDKOk;
        }

        private void HandleBehaviourFinished(EmojiBehaviourBase eBehaviour) {
            // Do nothing actually, just make sure behaviour finished playing.
            // Debug.Log("[ChatMessageManager - HandleBehaviourFinished] Finished playing!");
            if (eBehaviour != null && this._emojiList != null && this._emojiList.Contains(eBehaviour)) {
                this._emojiList.Remove(eBehaviour);
            }
        }

        private IEnumerator ProcessEmojis() {
            while (this._isEmojiProcessing) {
                var playList = Audience.Context.EmojiAuthorManager.ExtractCandidateEmojis();
                foreach (KeyValuePair<ChatAuthor, EmojiMessage> item in playList) {
                    this.CreateEmoji(item.Key, item.Value);
                }

                yield return null;
            }
        }
    }
}
