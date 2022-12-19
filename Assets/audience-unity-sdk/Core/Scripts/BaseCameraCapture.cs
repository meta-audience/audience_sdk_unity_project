using UnityEngine;

namespace AudienceSDK {
    // [RequireComponent(typeof(UnityEngine.Camera))]
    public abstract class BaseCameraCapture : MonoBehaviour {
        public UnityEngine.Camera TargetCamera { get; set; }

        public abstract void StartCapture(int width, int height);

        public abstract void StartRender();

        public abstract RenderTexture GetRenderTexture();

        public abstract void StopCapture();

        protected virtual void Awake() {
        }
    }
}
