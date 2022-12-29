using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using UnityEngine;

namespace AudienceSDK {
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "this class is a base class that contains several protected variables to inherit")]
    public abstract class EmojiBehaviourBase : MonoBehaviour {

        protected GameObject _emojiModel;
        protected AnimationClip _emojiAnimationClip;

        public Action<EmojiBehaviourBase> OnBehaviourFinished { get; set; }

        public void SetBehaviorAsset(string emojiKeyword, object emojiAsset, EmojiAsset animationAsset) {
            this.StartCoroutine(this.ShowEmojiBehavior(emojiKeyword, emojiAsset, animationAsset));
        }

        protected IEnumerator ShowEmojiBehavior(string emojiKeyword, object emojiAsset, EmojiAsset animationAsset) {
            yield return this.StartCoroutine(this.LoadEmojiModel(emojiKeyword, emojiAsset));
            if (this._emojiModel == null) {
                Debug.LogWarning("CombineEmoji Failed, model is null");
                GameObject.Destroy(this.gameObject);
                yield break;
            }

            // hide before animation ready
            this._emojiModel.SetActive(false);

            yield return this.StartCoroutine(this.LoadAnimation(animationAsset));
            if (this._emojiAnimationClip == null) {
                Debug.LogWarning("CombineEmoji Failed, animation is null");
                GameObject.Destroy(this.gameObject);
                yield break;
            }

            this._emojiModel.SetActive(true);

            var rc = this.CombineEmoji();
            if (rc != AudienceReturnCode.AudienceSDKOk) {
                Debug.LogWarning("CombineEmoji Failed, failed animation is null");
                GameObject.Destroy(this.gameObject);
                yield break;
            }

            // wait animation state to be playing
            yield return null;

            var emojiAnimation = this._emojiModel.GetComponent<Animation>();
            while (emojiAnimation.isPlaying) {
                yield return null;
            }

            // Finish the behaviour
            this.OnBehaviourFinished?.Invoke(this);
            GameObject.Destroy(this.gameObject);
        }

        protected abstract IEnumerator LoadEmojiModel(string keyword, object asset);

        protected IEnumerator LoadAnimation(EmojiAsset animationAsset) {
            var animation = AssetLoader.GetCachedAnimationClip(animationAsset.keyword, animationAsset.version);
            if (animation == null) {
                AssetLoader.LoadedAssetBundle data = AssetLoader.LoadAssetBundle(animationAsset.url);
                while (!data.Finished) {
                    yield return null;
                }

                if (data.Result != AudienceReturnCode.AudienceSDKOk) {
                    Debug.LogWarning("LoadAnimation Failed to load Asset data");
                    yield break;
                }

                if (data.AssetBundle == null) {
                    Debug.LogWarning("LoadAnimation Failed to download Asset bundle data");
                    yield break;
                }

                animation = data.AssetBundle.LoadAsset<AnimationClip>(animationAsset.keyword);
                if (animation == null) {
                    Debug.LogWarning(" LoadAnimation missing clip.");
                    yield break;
                }

                AssetLoader.SetCachedAnimationClip(animationAsset.keyword, animationAsset.version, animation);
                data.AssetBundle.Unload(false);
            }

            this._emojiAnimationClip = Instantiate<AnimationClip>(animation);
        }

        protected AudienceReturnCode CombineEmoji() {
            if (this._emojiModel == null) {
                Debug.LogWarning("CombineEmoji Failed, model is null");
                return AudienceReturnCode.AudienceSDKNullPtr;
            }

            if (this._emojiAnimationClip == null) {
                Debug.LogWarning("CombineEmoji Failed, animation is null");
                return AudienceReturnCode.AudienceSDKNullPtr;
            }

            var emojiAnimation = this._emojiModel.GetComponent<Animation>();
            if (emojiAnimation == null) {
                Debug.Log("CombineEmoji Failed, emoji Animation component is null");
                return AudienceReturnCode.AudienceSDKNullPtr;
            }

            emojiAnimation.AddClip(this._emojiAnimationClip, "animation");
            emojiAnimation.PlayQueued("animation");

            return AudienceReturnCode.AudienceSDKOk;
        }
    }
}
