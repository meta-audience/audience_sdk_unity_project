using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK
{
    public class ColliderSwitchCameraMode : MonoBehaviour
    {
        [SerializeField]
        private CameraMoveAlgorithmBase _appliedAlgorithm;

        [SerializeField]
        private Collider[] _disableColliders;

        [SerializeField]
        private Collider[] _enableColliders;

        private void OnTriggerEnter(Collider other)
        {
            if (this._appliedAlgorithm != null) {
                var cameraAlgos = Resources.FindObjectsOfTypeAll<CameraMoveAlgorithmBase>();
                foreach (var cameraAlgo in cameraAlgos)
                {
                    cameraAlgo.enabled = false;
                }
                this._appliedAlgorithm.enabled = true;
            }

            foreach (var collider in this._disableColliders) 
            {
                collider.enabled = false;
            }

            foreach (var collider in this._enableColliders)
            {
                collider.enabled = true;
            }
        }
    }
}