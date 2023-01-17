using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK
{
    public class CameraDirector : MonoBehaviour
    {
        private static CameraDirector instance = null;

        private bool _cameraDirectorInited = false;

        public static CameraDirector Instance
        {
            get
            {
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        private void Awake()
        {
            if (CameraDirector.Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            CameraDirector.Instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (Audience.AudienceInited)
            {
                this.SetupCameraDirector(true);
            }

            Audience.AudienceInitStateChanged += this.SetupCameraDirector;
        }

        private void OnDestroy()
        {
            if (Audience.AudienceInited)
            {
                Audience.Context.SceneManager.CurrentSceneManagerStateChanged -= this.OnSceneManagerStateChanged;
            }

            Audience.AudienceInitStateChanged -= this.SetupCameraDirector;
        }

        private void SetupCameraDirector(bool audienceInit) {
            if (audienceInit)
            {
                Audience.Context.SceneManager.CurrentSceneManagerStateChanged += this.OnSceneManagerStateChanged;
            }
            else 
            {
                Audience.Context.SceneManager.CurrentSceneManagerStateChanged -= this.OnSceneManagerStateChanged;
            }
        }

        private void OnSceneManagerStateChanged(SceneManager.SceneManagerState state) {
            switch (state) {
                case SceneManager.SceneManagerState.Loaded:
                    {
                        break;
                    }
                case SceneManager.SceneManagerState.Unload:
                    {
                        break;
                    }
                default:
                    break;
            }
        }
    }
}