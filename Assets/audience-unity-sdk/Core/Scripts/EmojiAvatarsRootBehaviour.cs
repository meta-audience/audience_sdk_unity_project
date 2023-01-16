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

        private void Update()
        {
            this.DockFollowTarget();
        }

        private void DockFollowTarget() {
            if (this._followTarget)
            {
                this.transform.eulerAngles = this._followTarget.eulerAngles;
                this.transform.position = this._followTarget.position;
            }
            else
            {
                this.transform.eulerAngles = Vector3.zero;
                this.transform.position = Vector3.zero;
            }
        }
    }
}