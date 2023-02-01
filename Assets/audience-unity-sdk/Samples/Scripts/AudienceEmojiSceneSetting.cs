using UnityEngine;

namespace AudienceSDK.Sample
{
    public class AudienceEmojiSceneSetting : MonoBehaviour
    {
        public Transform FollowPlayer
        {
            get 
            {
                return this._followPlayer;
            }
            private set
            {
                this._followPlayer = value;
                if (this._audienceInited)
                {
                    ApplyEmojiSceneSetting();
                }
            }
        }

        public bool EmojiAttachEmojiSpawner
        {
            get
            {
                return this._emojiAttachEmojiSpawner;
            }
            private set
            {
                this._emojiAttachEmojiSpawner = value;
                if (this._audienceInited)
                {
                    ApplyEmojiSceneSetting();
                }
            }
        }

        [SerializeField]
        private Transform _followPlayer = null;

        [SerializeField]
        private bool _emojiAttachEmojiSpawner = false;

        private bool _audienceInited = false;

        // Start is called before the first frame update
        private void Start()
        {
            if (AudienceSDK.Audience.AudienceInited)
            {
                this.OnAudienceInitedChanged(true);
            }

            AudienceSDK.Audience.AudienceInitStateChanged += this.OnAudienceInitedChanged;
        }

        protected virtual void OnDestroy()
        {
            AudienceSDK.Audience.AudienceInitStateChanged -= this.OnAudienceInitedChanged;
        }

        private void ApplyEmojiSceneSetting() {

            AudienceSDK.Audience.Context.EmojiAvatarManager.SetEmojiAvatarFollowTarget(this._followPlayer);
            AudienceSDK.Audience.Context.EmojiAvatarManager.SetEmojiAvatarLookAtTarget(this._followPlayer);

            AudienceSDK.Audience.Context.EmojiEffectManager.SetIsEmojiAttachEmojiSpawner(this._emojiAttachEmojiSpawner);
        }

        private void OnAudienceInitedChanged(bool inited) {
            this._audienceInited = inited;
            if (this._audienceInited) {
                ApplyEmojiSceneSetting();
            }
        }

        private void OnValidate()
        {
            // OnValidate will run in runtime and editor time. add if isPlaying to constrain.
            if (Application.isPlaying) {
                this.FollowPlayer = this._followPlayer;
                this.EmojiAttachEmojiSpawner = this._emojiAttachEmojiSpawner;
            }
        }
    }
}