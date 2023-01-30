using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK
{
    // if game developers want to move audience camera, you could extend CameraMoveAlgorithmBase,
    // then calculate the world coordinates and set the information to the audience camera by
    // calling MoveCameras(rotation, positon).
    // Game developers can do these calculations in Update() LateUpdate() or FixedUpdate() etc...
    // You can refer to example : DefaultDockCameraAlgorithm.cs or DefaultFollowCameraAlgorithm.cs
    public abstract class CameraMoveAlgorithmBase : MonoBehaviour
    {
        // audience camera is property, game developer can't set transform directly.
        private List<AudienceCameraBehaviourBase> _cameraBehaviourList = null;

        /// <summary>
        /// Move cameras to specific world space rotation and position.
        /// </summary>
        /// <param name="rotation">target world space rotation.</param>
        /// <param name="position">target world space position.</param>
        /// <param name="allowSceneSettingsOffset">
        /// Move the camera to a specific position and rotation and let the camera retain the scene setting offset (rotation and position) on the audience Websiste after it has been moved.</param>
        protected void MoveCameras(Quaternion rotation, Vector3 position, bool allowSceneSettingsOffset = true) {
            foreach (var cameraBehaviour in this._cameraBehaviourList) {
                if (cameraBehaviour)
                {
                    if (allowSceneSettingsOffset)
                    {
                        cameraBehaviour.transform.rotation = rotation * Quaternion.Euler(cameraBehaviour.SceneSettingEuler);
                        cameraBehaviour.transform.position = position + rotation * cameraBehaviour.SceneSettingPosition;
                    }
                    else
                    {
                        cameraBehaviour.transform.rotation = rotation;
                        cameraBehaviour.transform.position = position;
                    }
                }
            }
        }

        protected virtual void Awake()
        {
            this._cameraBehaviourList = new List<AudienceCameraBehaviourBase>();
        }

        protected virtual void Start()
        {
            if (Audience.AudienceInited)
            {
                this.OnAudienceInitStateChanged(true);
            }

            Audience.AudienceInitStateChanged += this.OnAudienceInitStateChanged;
        }

        protected virtual void OnDestroy()
        {
            if (Audience.AudienceInited)
            {
                this.OnAudienceInitStateChanged(false);
            }

            Audience.AudienceInitStateChanged -= this.OnAudienceInitStateChanged;
        }

        // Listen audience is initialize or not. register callback to scenemanager.
        private void OnAudienceInitStateChanged(bool audienceInit)
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
                this._cameraBehaviourList.Clear();
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
                        this._cameraBehaviourList.Clear();
                        break;
                    }
                default:
                    break;
            }
        }

        // collect camera's transform.
        private void CollectCameras()
        {
            var audienceCameraBehaviours = Resources.FindObjectsOfTypeAll<AudienceCameraBehaviourBase>();
            foreach (var audienceCameraBehaviour in audienceCameraBehaviours)
            {
                this._cameraBehaviourList.Add(audienceCameraBehaviour);
            }
        }

    }
}