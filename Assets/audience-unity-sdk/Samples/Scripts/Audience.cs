using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK.Sample 
{
    public class Audience : MonoBehaviour
    {
        public Action<bool> onAudienceInitStateChanged;

        private static Audience instance = null;
        private bool audienceInited = false;

        public static Audience Instance {
            get {
                return instance;
            }
            private set {
                instance = value;
            }
        }

        public bool AudienceInited {
            get {
                return this.audienceInited;
            }

            private set {
                this.audienceInited = value;
                this.onAudienceInitStateChanged?.Invoke(this.audienceInited);
            }
        }

        private void Awake()
        {
            if (Audience.Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Audience.Instance = this;
        }

        private void Start()
        {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            CameraUtilities.PostCreateCamera = PostCreateCameraFunc;
            AudienceSDK.Audience.Initialize();
            this.AudienceInited = true;
        }

        private void OnApplicationQuit()
        {
            this.AudienceInited = false;
            Debug.Log("OnApplicationQuit()");
            AudienceSDK.Audience.Context.Stop();
            NativeMethods.DeInit();
        }

        private static void PostCreateCameraFunc(AudienceCameraInstance instance)
        {
            instance.Instance = instance.GameObj.AddComponent<AudienceCameraBehaviour>();
            instance.Instance.Init(instance.Camera);
        }
    }
}