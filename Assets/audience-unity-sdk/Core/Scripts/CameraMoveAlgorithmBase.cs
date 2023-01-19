using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK
{
    // if game developers want to move audience camera, you could extend CameraMoveAlgorithmBase,
    // then calculate the world coordinates and set the information to the audience camera by
    // calling MoveCameras(rotation, positon).
    // You can refer to example : DefaultDockCameraAlgorithm.cs or DefaultFollowCameraAlgorithm.cs
    public abstract class CameraMoveAlgorithmBase : MonoBehaviour
    {
        // audience camera is property, game developer can't set transform directly.
        private List<Transform> _cameraTransformList = null;
        private Quaternion _rootRotation = Quaternion.identity;
        private Vector3 _rootPosition = Vector3.zero;

        protected void MoveCameras(Quaternion rotation, Vector3 position) {
            foreach (var cameraTransform in this._cameraTransformList) {
                if (cameraTransform)
                {
                    var localRotation = Quaternion.Inverse(this._rootRotation) * cameraTransform.rotation;
                    var localPosition = Quaternion.Inverse(this._rootRotation) * (cameraTransform.position - this._rootPosition);
                    this._rootRotation = rotation;
                    this._rootPosition = position;
                    cameraTransform.rotation = this._rootRotation * localRotation;
                    cameraTransform.position = this._rootPosition + this._rootRotation * localPosition;
                }
            }
        }

        protected virtual void Awake()
        {
            this._cameraTransformList = new List<Transform>();
        }

        protected virtual void Start()
        {
            if (Audience.AudienceInited)
            {
                this.RegisterCameraDirectorCallback(true);
            }

            Audience.AudienceInitStateChanged += this.RegisterCameraDirectorCallback;
        }

        protected virtual void OnDestroy()
        {
            if (Audience.AudienceInited)
            {
                this.RegisterCameraDirectorCallback(false);
            }

            Audience.AudienceInitStateChanged -= this.RegisterCameraDirectorCallback;
        }

        // Listen audience is initialize or not. regeist listener to scenemanager.
        private void RegisterCameraDirectorCallback(bool audienceInit)
        {
            if (audienceInit)
            {
                Audience.Context.SceneManager.CurrentSceneManagerStateChanged += this.OnSceneManagerStateChanged;
                if (Audience.Context.SceneManager.CurrentSceneManagerState == SceneManager.SceneManagerState.Loaded)
                {
                    this.CollectCameras();
                }
            }
            else
            {
                this._cameraTransformList.Clear();
                Audience.Context.SceneManager.CurrentSceneManagerStateChanged -= this.OnSceneManagerStateChanged;
            }
        }

        // Listen scene is loaded or not, and collect cameras or clear list.
        private void OnSceneManagerStateChanged(SceneManager.SceneManagerState state)
        {
            switch (state)
            {
                case SceneManager.SceneManagerState.Loaded:
                    {
                        this.CollectCameras();
                        break;
                    }
                case SceneManager.SceneManagerState.Unload:
                    {
                        this._cameraTransformList.Clear();
                        break;
                    }
                default:
                    break;
            }
        }

        // collect camera's transform.
        private void CollectCameras()
        {
            var audienceCameras = Resources.FindObjectsOfTypeAll<AudienceCameraBehaviourBase>();
            foreach (var audienceCamera in audienceCameras)
            {
                this._cameraTransformList.Add(audienceCamera.transform);
            }
        }

    }
}