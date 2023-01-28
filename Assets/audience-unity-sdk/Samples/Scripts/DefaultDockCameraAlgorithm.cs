using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AudienceSDK.Sample
{
    public class DefaultDockCameraAlgorithm : CameraMoveAlgorithmBase
    {
        [SerializeField]
        public Transform _dockTarget = null;

        protected override void Start() {
            base.Start();
            if (this._dockTarget)
            {
                this.MoveCameras(this._dockTarget.rotation, this._dockTarget.position);
            }
        }

        private void OnEnable()
        {
            if (this._dockTarget)
            {
                this.MoveCameras(this._dockTarget.rotation, this._dockTarget.position);
            }
        }

        private void Update()
        {
            if (this._dockTarget)
            {
                this.MoveCameras(this._dockTarget.rotation, this._dockTarget.position);
            }
        }
    }
}