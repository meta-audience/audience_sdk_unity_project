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
        private GameObject[] _disableGameObjects;

        [SerializeField]
        private GameObject[] _enableGameObjects;

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

            foreach (var obj in this._disableGameObjects) 
            {
                obj.SetActive(false);
            }

            foreach (var obj in this._enableGameObjects)
            {
                obj.SetActive(true);
            }
        }
    }
}