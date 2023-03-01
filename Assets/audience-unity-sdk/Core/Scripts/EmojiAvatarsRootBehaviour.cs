using System;
using UnityEngine;

namespace AudienceSDK
{
    public class EmojiAvatarsRootBehaviour : MonoBehaviour
    {
        public Transform FollowTarget {
            get
            {
                return this._followTarget;
            }

            set
            {
                this._followTarget = value;
                this.DockFollowTarget();
            }
        }

        private Transform _followTarget = null;

        private void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
        }

        private void FixedUpdate()
        {
            this.DockFollowTarget();
        }

        private void DockFollowTarget() {
            if (this._followTarget)
            {
                this.transform.rotation = this._followTarget.rotation;
                this.transform.position = this._followTarget.position;
            }
            else
            {
                this.transform.rotation = Quaternion.identity;
                this.transform.position = Vector3.zero;
            }
        }
    }
}