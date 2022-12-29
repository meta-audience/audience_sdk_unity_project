using UnityEngine;

namespace AudienceSDK {
    public class SimpleSBSCameraCapture : BaseCameraCapture {
        private RenderTexture _renderTarget;
        private RenderTexture _tempRenderTarget;

        public override void StartCapture(int width, int height) {
            this.StopCapture();
            this._tempRenderTarget = new RenderTexture(width / 2, height, 24, RenderTextureFormat.BGRA32);
            this._tempRenderTarget.Create();

            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.BGRA32);
            rt.wrapMode = TextureWrapMode.Repeat;
            this.SetRenderTarget(rt);
            this.GetRenderTarget().Create();
            this.GetRenderTarget().hideFlags = HideFlags.HideAndDontSave;

            this.TargetCamera.aspect = width / height;
            this.TargetCamera.targetTexture = this._tempRenderTarget;
        }

        public override void StartRender() {
            RenderTexture lastActive = RenderTexture.active;
            this.SetCameraTransform(0);
            this.TargetCamera.rect = new Rect(0, 0, 1, 1);
            this.TargetCamera.Render();
            Graphics.CopyTexture(this._tempRenderTarget, 0, 0, 0, 0, this._tempRenderTarget.width, this._tempRenderTarget.height, this.GetRenderTarget(), 0, 0, 0, 0);
            this.SetCameraTransform(1);
            this.TargetCamera.rect = new Rect(0, 0, 1, 1);
            this.TargetCamera.Render();
            Graphics.CopyTexture(this._tempRenderTarget, 0, 0, 0, 0, this._tempRenderTarget.width, this._tempRenderTarget.height, this.GetRenderTarget(), 0, 0, this._tempRenderTarget.width, 0);
            RenderTexture.active = lastActive;
        }

        public override RenderTexture GetRenderTexture() {
            return this.GetRenderTarget();
        }

        public override void StopCapture() {
            if (this.GetRenderTarget()) {

                UnityEngine.Object.Destroy(this.GetRenderTarget());
                this.SetRenderTarget(null);
            }

            if (this._tempRenderTarget != null) {

                this._tempRenderTarget.Release();
                UnityEngine.Object.Destroy(this._tempRenderTarget);
            }
        }

        private void SetCameraTransform(int eye) {
            var pos = Vector3.right * this.TargetCamera.stereoSeparation * (eye == 0 ? -1 : 1);
            this.TargetCamera.transform.localPosition = pos;
        }

        private RenderTexture GetRenderTarget() {
            return this._renderTarget;
        }

        private void SetRenderTarget(RenderTexture value) {
            this._renderTarget = value;
        }
    }
}
