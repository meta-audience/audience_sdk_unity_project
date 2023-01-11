using System;
using UnityEngine;

namespace AudienceSDK
{
    public class EmojiAvatarsRootBehaviour : MonoBehaviour
    {
        public Action OnEmojiAvatarsRootDestroy { get; set; }

        private void OnDestroy()
        {
            // When destroy, notify listener move all avatars to other place.
            this.OnEmojiAvatarsRootDestroy?.Invoke();
        }
    }
}