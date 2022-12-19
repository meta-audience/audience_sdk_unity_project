using UnityEngine;
using UnityEngine.Rendering;

namespace AudienceSDK {

    /// <summary>
    /// Double Eye 360 Capture Half.
    /// Only render Front/Left/Right face of cubemap.
    /// </summary>
    public class StereoHalfCameraCapture : PanoramicCameraCapture {
        private RenderTexture _equirect;
        private RenderTexture _cubemapLeft;
        private RenderTexture _cubemapRight;
        private bool _isUnityEnable360StereoCapture;

        /// <summary>
        /// Initialize capture parameters.
        /// </summary>
        /// <param name="width">scene setting width.</param>
        /// <param name="height">scene setting height.</param>
        public override void StartCapture(int width, int height) {
            this.StopCapture();

            this._targetEquirectSize = new Vector2Int(width, height);
            this._faceMask = (1 << (int)CubemapFace.PositiveZ) | (1 << (int)CubemapFace.PositiveX) | (1 << (int)CubemapFace.NegativeX);

            // NeedToBeFxied
            this._isUnityEnable360StereoCapture = UserConfig.IsUnityEnable360StereoCapture;
            /*
            BS_Utils.Utilities.Config config = new BS_Utils.Utilities.Config("Audience");
            this.isUnityEnable360StereoCapture = config.GetBool("UserData", "isUnityEnable360StereoCapture", false, true);
            */

            var cubemapSize = this.GetCubemapSize(this._targetEquirectSize.x, this._targetEquirectSize.y);
            if (cubemapSize <= 0) {

                Debug.LogWarning("Get cubemap size fail, value is :" + cubemapSize.ToString());
                return;
            }

            this._cubemapLeft = new RenderTexture(cubemapSize, cubemapSize, 0);
            this._cubemapLeft.dimension = TextureDimension.Cube;
            this._cubemapLeft.hideFlags = HideFlags.HideAndDontSave;
            this._cubemapLeft.Create();

            this._cubemapRight = new RenderTexture(cubemapSize, cubemapSize, 0);
            this._cubemapRight.dimension = TextureDimension.Cube;
            this._cubemapRight.hideFlags = HideFlags.HideAndDontSave;
            this._cubemapRight.Create();

            this._equirect = new RenderTexture(this._targetEquirectSize.x, this._targetEquirectSize.y, 24, RenderTextureFormat.BGRA32);
            this._equirect.hideFlags = HideFlags.HideAndDontSave;
            this._equirect.Create();
        }

        /// <summary>
        /// Call camera render.
        /// </summary>
        public override void StartRender() {
            this.TargetCamera.stereoSeparation = 0.064f;
            this.TargetCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            if (!this._isUnityEnable360StereoCapture) {

                this.TargetCamera.transform.localPosition = Vector3.right * -0.032f;
            }

            this.TargetCamera.RenderToCubemap(this._cubemapLeft, this._faceMask, UnityEngine.Camera.MonoOrStereoscopicEye.Left);

            this.TargetCamera.stereoSeparation = 0.064f;
            this.TargetCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            if (!this._isUnityEnable360StereoCapture) {

                this.TargetCamera.transform.localPosition = Vector3.right * 0.032f;
            }

            this.TargetCamera.RenderToCubemap(this._cubemapRight, this._faceMask, UnityEngine.Camera.MonoOrStereoscopicEye.Right);
        }

        /// <summary>
        /// Output render result.
        /// </summary>
        /// <returns>RenderTexture.</returns>
        public override RenderTexture GetRenderTexture() {
            this._cubemapLeft.ConvertToEquirect(this._equirect, UnityEngine.Camera.MonoOrStereoscopicEye.Left);
            this._cubemapRight.ConvertToEquirect(this._equirect, UnityEngine.Camera.MonoOrStereoscopicEye.Right);
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

            if (this._cubemapLeft != null) {

                this._cubemapLeft.Release();
                UnityEngine.Object.Destroy(this._cubemapLeft);
            }

            if (this._cubemapRight != null) {

                this._cubemapRight.Release();
                UnityEngine.Object.Destroy(this._cubemapRight);
            }
        }
    }
}
