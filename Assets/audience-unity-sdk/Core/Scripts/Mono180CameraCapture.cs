using UnityEngine;
using UnityEngine.Rendering;

namespace AudienceSDK {

    /// <summary>
    /// Single Eye 180 Capture.
    /// </summary>
    public class Mono180CameraCapture : PanoramicCameraCapture {
        private RenderTexture _equirect;
        private RenderTexture _cubemap;

        /// <summary>
        /// Initialize capture parameters.
        /// </summary>
        /// <param name="width">scene setting width.</param>
        /// <param name="height">scene setting height.</param>
        public override void StartCapture(int width, int height) {
            this.StopCapture();

            this._faceMask = 63 ^ (1 << (int)CubemapFace.NegativeZ);
            this._targetEquirectSize = new Vector2Int(width * 2, height * 2);

            var cubemapSize = this.GetCubemapSize(this._targetEquirectSize.x, this._targetEquirectSize.y);
            if (cubemapSize <= 0) {

                Debug.LogWarning("Get cubemap size fail, value is :" + cubemapSize.ToString());
                return;
            }

            this._cubemap = new RenderTexture(cubemapSize, cubemapSize, 0);
            this._cubemap.dimension = TextureDimension.Cube;
            this._cubemap.hideFlags = HideFlags.HideAndDontSave;
            this._cubemap.Create();

            this._equirect = new RenderTexture(this._targetEquirectSize.x, this._targetEquirectSize.y, 24, RenderTextureFormat.BGRA32);
            this._equirect.hideFlags = HideFlags.HideAndDontSave;
            this._equirect.Create();
        }

        /// <summary>
        /// Call camera render.
        /// </summary>
        public override void StartRender() {
            this.TargetCamera.RenderToCubemap(this._cubemap, this._faceMask, UnityEngine.Camera.MonoOrStereoscopicEye.Left);
        }

        /// <summary>
        /// Output render result.
        /// </summary>
        /// <returns>RenderTexture.</returns>
        public override RenderTexture GetRenderTexture() {
            var targetWidth = this._targetEquirectSize.x / 2;
            var targetHeight = this._targetEquirectSize.y / 2;
            var equirectStartX = this._targetEquirectSize.x / 4;
            var equirectStartY = this._targetEquirectSize.y / 2;
            var targetCropX = this._targetEquirectSize.x / 2;
            var targetCropY = this._targetEquirectSize.y / 2;

            RenderTexture temp = RenderTexture.GetTemporary(targetWidth, targetHeight, 24, RenderTextureFormat.BGRA32);
            this._cubemap.ConvertToEquirect(this._equirect, UnityEngine.Camera.MonoOrStereoscopicEye.Left);
            Graphics.CopyTexture(this._equirect, 0, 0, equirectStartX, equirectStartY, targetCropX, targetCropY, temp, 0, 0, 0, 0);
            RenderTexture.ReleaseTemporary(temp);
            return temp;
        }

        /// <summary>
        /// Destroy capture.
        /// </summary>
        public override void StopCapture() {
            if (this._equirect != null) {

                this._equirect.Release();
                UnityEngine.Object.Destroy(this._equirect);
            }

            if (this._cubemap != null) {

                this._cubemap.Release();
                UnityEngine.Object.Destroy(this._cubemap);
            }
        }
    }
}
