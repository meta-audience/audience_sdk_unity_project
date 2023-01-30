using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK.Sample
{
    public class DefaultFollowCameraAlgorithm : CameraMoveAlgorithmBase
    {
        [SerializeField]
        private Transform _followTarget = null;

        [SerializeField]
        private Vector3 _relativePosition = Vector3.zero;

        [SerializeField]
        [Range(0, 2)]
        private float _followSpeed = 1;

        private Quaternion _lastRotation = Quaternion.identity;

        private Vector3 _lastPosition = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            if (this._followTarget) {
                this.MoveCameras(this._followTarget.rotation, this._followTarget.position, false);
                this._lastRotation = this._followTarget.rotation;
                this._lastPosition = this._followTarget.position;
            }
        }

        private void OnEnable()
        {
            if (this._followTarget)
            {
                this.MoveCameras(this._followTarget.rotation, this._followTarget.position, false);
                this._lastRotation = this._followTarget.rotation;
                this._lastPosition = this._followTarget.position;
            }
        }

        private void FixedUpdate()
        {
            if (this._followTarget)
            {
                var destinationPos = this._followTarget.position + this._followTarget.rotation * this._relativePosition;
                var position = Vector3.Lerp(this._lastPosition, destinationPos, this._followSpeed);
                var detinationRot = Quaternion.LookRotation(this._followTarget.forward);
                var rotation = Quaternion.Lerp(this._lastRotation, detinationRot, this._followSpeed);
                this.MoveCameras(rotation, position, false);
                this._lastRotation = rotation;
                this._lastPosition = position;
            }
        }
    }
}