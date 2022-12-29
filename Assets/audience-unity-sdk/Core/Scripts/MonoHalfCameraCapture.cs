using UnityEngine;
using UnityEngine.Rendering;

namespace AudienceSDK {

    /// <summary>
    /// Single Eye 360 Capture Half.
    /// Only render Front/Left/Right face of cubemap.
    /// </summary>
    public class MonoHalfCameraCapture : PanoramicCameraCapture {
        private RenderTexture _equirect;
        private RenderTexture _cubemap;

        /// <summary>
        /// Initialize capture parameters.
        /// </summary>
        /// <param name="width">scene setting width.</param>
        /// <param name="height">scene setting height.</param>
        public override void StartCapture(int width, int height) {
            this.StopCapture();

            this._targetEquirectSize = new Vector2Int(width, height);
            this._faceMask = (1 << (int)CubemapFace.PositiveZ) | (1 << (int)CubemapFace.PositiveX) | (1 << (int)CubemapFace.NegativeX);

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
            this._cubemap.ConvertToEquirect(this._equirect, UnityEngine.Camera.MonoOrStereoscopicEye.Mono);
            return this._equirect;
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
