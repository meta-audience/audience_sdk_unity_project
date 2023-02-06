using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK.Sample 
{
    public class Audience : MonoBehaviour
    {
        private static Audience instance = null;
        public static Audience Instance {
            get {
                return instance;
            }
            private set {
                instance = value;
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
        }

        private void OnApplicationQuit()
        {
            Debug.Log("OnApplicationQuit()");
            AudienceSDK.Audience.Deinitialize();
        }

        private static void PostCreateCameraFunc(AudienceCameraInstance instance)
        {
            instance.Instance = instance.GameObj.AddComponent<AudienceCameraBehaviour>();
            instance.Instance.Init(instance.Camera);
        }
    }
}