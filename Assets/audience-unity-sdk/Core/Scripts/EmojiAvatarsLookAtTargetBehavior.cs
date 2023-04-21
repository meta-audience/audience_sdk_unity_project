using System;
using UnityEngine;

namespace AudienceSDK
{
    public class EmojiAvatarsLookAtTargetBehavior : MonoBehaviour
    {
        public Transform LookAtTarget
        {
            get
            {
                return this._lookAtTarget;
            }

            set
            {
                this._lookAtTarget = value;
                this.DockLookAtTarget();
            }
        }

        private Transform _lookAtTarget = null;

        private void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
        }

        private void FixedUpdate()
        {
            this.DockLookAtTarget();
        }

        private void DockLookAtTarget()
        {
            if (this._lookAtTarget)
            {
                this.transform.rotation = this._lookAtTarget.rotation;
                this.transform.position = this._lookAtTarget.position;
            }
            else
            {
                this.transform.rotation = Quaternion.identity;
                this.transform.position = Vector3.zero;
            }
        }
    }
}
