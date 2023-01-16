using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AudienceSDK {
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "this class is a base class that contains several protected variables to inherit")]
    public abstract class EmojiAvatarBehaviourBase : MonoBehaviour {

        public abstract AudienceReturnCode SetAuthors(List<ChatAuthor> authors);

        public Action<EmojiAvatarBehaviourBase> OnAvatarFinished { get; set; }

        protected List<ChatAuthor> _authors;

        private float _lifeTime = 0f;

        public bool IsAlive() {
            return this._lifeTime > 0f;
        }

        public List<ChatAuthor> GetAvatarAuthors() {
            return this._authors;
        }

        public AudienceReturnCode SpawnEmoji() {
            if (!this.IsAlive()) {
                Debug.LogError("SpawnerEmoji fail, spawner is not alive.");
                return AudienceReturnCode.AudienceSDKInvalidState;
            }

            var rc = Audience.Context.EmojiAvatarManager.RearrangeAvatarList(this);
            if (rc != AudienceReturnCode.AudienceSDKOk) {
                return rc;
            }

            this.ResetLifeTime();
            return AudienceReturnCode.AudienceSDKOk;
        }

        public AudienceReturnCode Init() {
            // TODO: read config to set _spawnerIdleTime
            // if (Audience.Context.CurrentUserConfig.EmojiLimitPerMessage > 0) {
            //    this._spawnerIdleTime = Audience.Context.CurrentUserConfig.EmojiLimitPerMessage;
            // }
            this.ResetLifeTime();
            this.StartCoroutine(this.UpdateLifeTime());
            this.StartCoroutine(this.UpdateDirection());
            return AudienceReturnCode.AudienceSDKOk;
        }

        private void Awake() {
            this._authors = new List<ChatAuthor>();
        }

        private void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        private void ResetLifeTime() {
            this._lifeTime = Audience.Context.EmojiAvatarManager.AvatarTotalLifeTime;
        }

        private IEnumerator UpdateLifeTime() {
            while (true) {
                this._lifeTime -= Time.deltaTime;
                if (this._lifeTime <= 0f) {
                    this._lifeTime = 0f;
                    this.OnAvatarFinished?.Invoke(this);
                    break;
                }

                yield return null;
            }

            UnityEngine.Object.DestroyImmediate(this.gameObject);
        }

        private IEnumerator UpdateDirection() {
            while (true)
            {
                if (Audience.Context.EmojiAvatarManager.EmojiAvatarsLookAtTarget == null)
                {
                    Debug.LogWarning("EmojiAvatarsLookAtTarget should exist, check EmojiAvatarManager is inited.");
                    break;
                }
                this.transform.LookAt(Audience.Context.EmojiAvatarManager.EmojiAvatarsLookAtTarget.transform);
                yield return null;
            }
        }
    }
}
