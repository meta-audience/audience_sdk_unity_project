using UnityEngine;

namespace AudienceSDK {
    public class MonoCameraCapture : BaseCameraCapture {
        private RenderTexture _renderTarget;

        public override void StartCapture(int width, int height) {
            this.StopCapture();
            RenderTexture texture = new RenderTexture(width, height, 24, RenderTextureFormat.BGRA32);
            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.Create();

            this.SetRenderTarget(texture);
        }

        public override void StartRender() {
            this.TargetCamera?.Render();
        }

        public override RenderTexture GetRenderTexture() {
            return this.GetRenderTarget();
        }

        public override void StopCapture() {
            var texture = this.GetRenderTarget();
            if (texture != null) {

                this.SetRenderTarget(null);

                texture.Release();
                UnityEngine.Object.Destroy(texture);
            }
        }

        private RenderTexture GetRenderTarget() {
            return this._renderTarget;
        }

        private void SetRenderTarget(RenderTexture texture) {
            this._renderTarget = texture;
            if (this.TargetCamera != null) {

                this.TargetCamera.targetTexture = texture;
            }
        }
    }
}
