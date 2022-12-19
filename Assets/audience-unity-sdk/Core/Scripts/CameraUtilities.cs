using UnityEngine;

namespace AudienceSDK {
    public class CameraUtilities {
        public delegate void PostCreateCameraDel(AudienceCameraInstance behaviour);

        public static PostCreateCameraDel PostCreateCamera { get; set; }

        public static void DestroyCamera(AudienceCameraBehaviourBase instance) {
            UnityEngine.Object.Destroy(instance.gameObject);
        }

        public static void CreateCamera(Camera camera) {
            AudienceCameraInstance audienceCameraInstance = new AudienceCameraInstance(camera);
            PostCreateCamera(audienceCameraInstance);
        }
    }
}
