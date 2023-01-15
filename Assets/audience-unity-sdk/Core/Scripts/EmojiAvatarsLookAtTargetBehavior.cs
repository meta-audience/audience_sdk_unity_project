using System;
using UnityEngine;

namespace AudienceSDK
{
    public class EmojiAvatarsLookAtTargetBehavior : MonoBehaviour
    {
        public Action OnEmojiAvatarsLookAtTargetDestroy { get; set; }

        private void OnDestroy()
        {
            // When destroy, notify listener move all avatars to other place.
            this.OnEmojiAvatarsLookAtTargetDestroy?.Invoke();
        }
    }
}
