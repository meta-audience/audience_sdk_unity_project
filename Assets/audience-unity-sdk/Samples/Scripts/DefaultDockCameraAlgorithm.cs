using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AudienceSDK.Sample
{
    public class DefaultDockCameraAlgorithm : CameraMoveAlgorithmBase
    {
        [SerializeField]
        public Transform _dockTarget = null;

        private void Update()
        {
            if (this._dockTarget)
            {
                this.MoveCameras(this._dockTarget.rotation, this._dockTarget.position, true);
            }
        }
    }
}