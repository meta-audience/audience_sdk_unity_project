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

        private void Update()
        {
            this.DockLookAtTarget();
        }

        private void DockLookAtTarget()
        {
            if (this._lookAtTarget)
            {
                this.transform.eulerAngles = this._lookAtTarget.eulerAngles;
                this.transform.position = this._lookAtTarget.position;
            }
            else
            {
                this.transform.eulerAngles = Vector3.zero;
                this.transform.position = Vector3.zero;
            }
        }
    }
}
