using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AudienceSDK {
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "this class is a base class that contains several protected variables to inherit")]
    public class AudienceCameraBehaviourBase : MonoBehaviour {
        protected int _mappingId;

        protected UnityEngine.Camera _cam;

        protected int _emojiHeartCount = 0;
        protected int _emojiUnknownCount = 0;

        protected CameraAvatar _cameraAvatar = null;
        protected UnityEngine.Camera _streamingCamera;

        private readonly WaitUntil _waitForMainCamera = new WaitUntil(() => UnityEngine.Camera.main);
        private UnityEngine.Camera _mainCamera = null;

        private BaseCameraCapture _streamingCameraCapture;
        private RenderTexture _previewRenderTexture;
        private RenderTexture _streamingRenderTexture;
        private RenderTexture _streamingRenderTextureDest;

        // Minimum resolution side length we support
        private float _minResolutionSideLength = 640.0f;

        private Transform _streamingCameraTransform;

        // State record
        private StreamState _streamState = StreamState.Unload;
        private SessionState _sessionState = SessionState.Inactive;

        public Vector3 ThirdPersonPos { get; set; }

        public Vector3 ThirdPersonRot { get; set; }

        public int MappingId {
            get {
                return this._mappingId;
            }
        }

        public int RenderFrames { get; set; } = 30;

        private float _accumulatedTime = 0.0f;
        private int _lastFrameCount = -1;

        public virtual void Init(Camera camera) {
        }

        public void HandleStreamStateChanged(StreamState state) {
            Debug.Log("HandleStreamStateChanged state =" + state);
            this._streamState = state;
        }

        public void HandleSessionStateChanged(string streamSessionId, SessionState state) {
            Debug.Log("HandleSessionStateChanged streamSessionId =" + streamSessionId + " + state =" + state);

            var session = Audience.Context.CurrentSceneObject.stream_sessions.Find(x => x.stream_session_id == streamSessionId);
            if (session == null) {
                Debug.LogError("[AudienceCameraBehaviourBase] HandleSessionStateChanged - invalid streamSessionId: " + streamSessionId);
                return;
            }

            if (session.cameras_mapping.FindIndex(x => x.mapping_id == this.MappingId) < 0) {
                // Not related to this camera, just ignore.
                return;
            }

            this._sessionState = state;
            this.UpdateCameraAvatarColor();
        }

        public void HandlePeerCameraDataReceived(string streamSessionId, NativePeerCameraData peerCameraData) {
            if (!this._cam.name.Equals(peerCameraData.VideoId)) {

                return;
            }

            Debug.Log("HandlePeerCameraDataReceived peerId =" + peerCameraData.PeerId);
            Debug.Log("HandlePeerCameraDataReceived videoId =" + peerCameraData.VideoId);
            Debug.Log("HandlePeerCameraDataReceived position =" + peerCameraData.Position);
            Debug.Log("HandlePeerCameraDataReceived rotation =" + peerCameraData.Rotation);
        }

        public void HandlePeerEmojiDataReceived(string streamSessionId, NativePeerEmojiData peerEmojiData) {
            if (!this._cam.name.Equals(peerEmojiData.VideoId)) {

                return;
            }

            Debug.Log("HandlePeerEmojiDataReceived videoId =" + peerEmojiData.VideoId);

            switch (peerEmojiData.EmojiType) {
                case EmojiType.Heart:
                    this._emojiHeartCount++;
                    break;
                default:
                    this._emojiUnknownCount++;
                    break;
            }

            Debug.Log("HandlePeerEmojiDataReceived EmojiHeartCount =" + this._emojiHeartCount);
            Debug.Log("HandlePeerEmojiDataReceived EmojiUnknownCount =" + this._emojiUnknownCount);
        }

        internal virtual void CreateRenderTexture(AudienceSDK.Camera camera) {
            if (this._previewRenderTexture == null) {

                this._previewRenderTexture = new RenderTexture(1, 1, 24, RenderTextureFormat.BGRA32);
            } else {
                this._cam.targetTexture = null;
                this._previewRenderTexture.Release();
            }

            this._previewRenderTexture.width = Mathf.RoundToInt(((float)camera.texture_width / (float)camera.texture_height) * this._minResolutionSideLength);
            this._previewRenderTexture.height = Mathf.RoundToInt(this._minResolutionSideLength);
            this._previewRenderTexture.useDynamicScale = false;
            this._previewRenderTexture.antiAliasing = 1;
            this._previewRenderTexture.Create();

            this._cam.targetTexture = this._previewRenderTexture;
            this._cameraAvatar.SetCameraAvatarPreviewTexture(this._previewRenderTexture);
        }

        protected virtual void DelayedInit(AudienceSDK.Camera camera) {
            this._mainCamera = UnityEngine.Camera.main;
            var gameObj = Instantiate(this._mainCamera.gameObject);
            gameObj.SetActive(false);
            gameObj.name = "Audience Camera";
            gameObj.tag = "Untagged";

            while (gameObj.transform.childCount > 0) {

                UnityEngine.Object.DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
            }

            UnityEngine.Object.DestroyImmediate(gameObj.GetComponent("CameraRenderCallbacksManager"));
            UnityEngine.Object.DestroyImmediate(gameObj.GetComponent("AudioListener"));
            UnityEngine.Object.DestroyImmediate(gameObj.GetComponent("MeshCollider"));

            if (SteamVRCompatibility.IsAvailable) {

                UnityEngine.Object.DestroyImmediate(gameObj.GetComponent(SteamVRCompatibility.SteamVRCamera));
                UnityEngine.Object.DestroyImmediate(gameObj.GetComponent(SteamVRCompatibility.SteamVRFade));
            }

            foreach (var c in gameObj.GetComponents<MonoBehaviour>()) {

                // Plugin.Log.Debug("c.type.Namespace =" + c.GetType().Namespace + " c.type.name =" + c.GetType().Name);
            }

            this._cam = gameObj.GetComponent<UnityEngine.Camera>();
            this._cam.stereoTargetEye = StereoTargetEyeMask.None;

            // preview camera could see 1st Person Exclusion avatar part.
            this._cam.enabled = false;
            this._cam.name = camera.mapping_id.ToString();
            this._mappingId = camera.mapping_id;

            var streamingCameraGameObject = Instantiate(this._mainCamera.gameObject);
            streamingCameraGameObject.tag = "Untagged";
            UnityEngine.Object.DontDestroyOnLoad(streamingCameraGameObject);
            this._streamingCamera = streamingCameraGameObject.GetComponent<UnityEngine.Camera>();
            while (streamingCameraGameObject.transform.childCount > 0) {

                UnityEngine.Object.DestroyImmediate(streamingCameraGameObject.transform.GetChild(0).gameObject);
            }

            UnityEngine.Object.DestroyImmediate(streamingCameraGameObject.GetComponent("CameraRenderCallbacksManager"));
            UnityEngine.Object.DestroyImmediate(streamingCameraGameObject.GetComponent("AudioListener"));
            UnityEngine.Object.DestroyImmediate(streamingCameraGameObject.GetComponent("MeshCollider"));
            if (SteamVRCompatibility.IsAvailable) {

                UnityEngine.Object.DestroyImmediate(streamingCameraGameObject.GetComponent(SteamVRCompatibility.SteamVRCamera));
                UnityEngine.Object.DestroyImmediate(streamingCameraGameObject.GetComponent(SteamVRCompatibility.SteamVRFade));
            }

            this._streamingCameraTransform = this._streamingCamera.gameObject.transform;
            this._streamingCameraTransform.parent = this.transform;
            this._streamingCameraTransform.localPosition = Vector3.zero;
            this._streamingCameraTransform.localRotation = Quaternion.identity;

            this._streamingCameraCapture = this.AddCameraCapture(streamingCameraGameObject, camera);

            // streaming camera could see 1st Person Exclusion avatar part.
            this._streamingCamera.enabled = false;
            gameObj.SetActive(true);

            var cameraTransform = this._mainCamera.transform;
            this.transform.position = cameraTransform.position;
            this.transform.rotation = cameraTransform.rotation;

            gameObj.transform.parent = this.transform;
            gameObj.transform.localPosition = Vector3.zero;
            gameObj.transform.localRotation = Quaternion.identity;
            gameObj.transform.localScale = Vector3.one;

            this.ThirdPersonPos = new Vector3(camera.position_x, camera.position_y, camera.position_z);
            this.ThirdPersonRot = new Vector3(camera.rotation_x, camera.rotation_y, camera.rotation_z);

            var cameraAvatarRoot = new GameObject();
            cameraAvatarRoot.transform.parent = this.transform;
            cameraAvatarRoot.transform.localRotation = Quaternion.identity;
            cameraAvatarRoot.transform.localPosition = Vector3.zero;
            this._cameraAvatar = cameraAvatarRoot.AddComponent<CameraAvatar>();
            this._cameraAvatar.InitCameraAvatar(camera);

            this._emojiHeartCount = 0;
            AudienceSDK.Audience.Context.StreamStateChanged += this.HandleStreamStateChanged;
            AudienceSDK.Audience.Context.SessionStateChanged += this.HandleSessionStateChanged;
            AudienceSDK.Audience.Context.PeerEmojiDataReceived += this.HandlePeerEmojiDataReceived;
            AudienceSDK.Audience.Context.PeerCameraDataReceived += this.HandlePeerCameraDataReceived;

            this._streamingCameraCapture?.StartCapture(camera.texture_width, camera.texture_height);
            this._streamingRenderTextureDest = new RenderTexture(camera.texture_width, camera.texture_height, 24, RenderTextureFormat.BGRA32);
            this.CreateRenderTexture(camera);
        }

        protected virtual void OnGUI() {
            if (this._lastFrameCount != Time.frameCount) {
                // Make sure it won't be called once per frame
                this._lastFrameCount = Time.frameCount;
                this._accumulatedTime += Time.deltaTime;
                float frameTime = 1.0f / (float)this.RenderFrames;

                this.transform.position = this.ThirdPersonPos;
                this.transform.eulerAngles = this.ThirdPersonRot;

                if (this._accumulatedTime > frameTime) {
                    this._cam.Render();
                    if (AudienceSDK.Audience.Context.SendCameraFrameNow()) {
                        this._streamingCameraCapture.StartRender();
                        this.StartCoroutine(this.DelaySendCameraFrame());
                    }

                    this._accumulatedTime -= frameTime * Mathf.Floor(this._accumulatedTime / frameTime);
                }
            }
        }

        protected virtual void OnDestroy() {
            this._previewRenderTexture.Release();

            this._streamingRenderTextureDest.Release();
            if (this._streamingRenderTextureDest) {

                UnityEngine.Object.Destroy(this._streamingRenderTextureDest);
            }

            if (this._streamingCameraTransform) {

                UnityEngine.Object.Destroy(this._streamingCameraTransform.gameObject);
            }

            if (this._cameraAvatar != null) {

                this._cameraAvatar.DeinitCameraAvatar();
                UnityEngine.Object.Destroy(this._cameraAvatar.gameObject);
            }

            AudienceSDK.Audience.Context.StreamStateChanged -= this.HandleStreamStateChanged;
            AudienceSDK.Audience.Context.SessionStateChanged -= this.HandleSessionStateChanged;
            AudienceSDK.Audience.Context.PeerCameraDataReceived -= this.HandlePeerCameraDataReceived;
            AudienceSDK.Audience.Context.PeerEmojiDataReceived -= this.HandlePeerEmojiDataReceived;
        }

        protected BaseCameraCapture AddCameraCapture(GameObject cameraObj, AudienceSDK.Camera camera) {
            BaseCameraCapture baseCameraCapture;
            switch ((CaptureType)camera.capture_type) {
                case CaptureType._3D_Flat:
                    baseCameraCapture = cameraObj.AddComponent<SimpleSBSCameraCapture>();
                    break;
                case CaptureType._3D_360:
                    baseCameraCapture = cameraObj.AddComponent<Stereo360CameraCapture>();
                    break;
                case CaptureType._3D_180:
                    baseCameraCapture = cameraObj.AddComponent<Stereo180CameraCapture>();
                    break;
                case CaptureType._2D_Flat:
                    baseCameraCapture = cameraObj.AddComponent<MonoCameraCapture>();
                    break;
                case CaptureType._2D_360:
                    baseCameraCapture = cameraObj.AddComponent<Mono360CameraCapture>();
                    break;
                case CaptureType._2D_180:
                    baseCameraCapture = cameraObj.AddComponent<Mono180CameraCapture>();
                    break;
                case CaptureType._3D_CULLBACK:
                    baseCameraCapture = cameraObj.AddComponent<StereoCullbackCameraCapture>();
                    break;
                case CaptureType._3D_HALF:
                    baseCameraCapture = cameraObj.AddComponent<StereoHalfCameraCapture>();
                    break;
                case CaptureType._2D_CULLBACK:
                    baseCameraCapture = cameraObj.AddComponent<MonoCullbackCameraCapture>();
                    break;
                case CaptureType._2D_HALF:
                    baseCameraCapture = cameraObj.AddComponent<MonoHalfCameraCapture>();
                    break;
                default:
                    Debug.LogError("Receive unknown capture type from scene");
                    baseCameraCapture = cameraObj.AddComponent<SimpleSBSCameraCapture>();
                    break;
            }

            baseCameraCapture.TargetCamera = cameraObj.GetComponent<UnityEngine.Camera>();
            return baseCameraCapture;
        }

        private void UpdateCameraAvatarColor() {

            Color newColor = Color.white;
            switch (this._sessionState) {
                case SessionState.Inactive:
                case SessionState.Inited:
                    newColor = Color.gray;
                    break;

                case SessionState.Starting:
                case SessionState.Stopping:
                case SessionState.UpdateNeeded:
                case SessionState.Updating:
                    newColor = Color.yellow;
                    break;

                case SessionState.Ready:
                    newColor = Color.green;
                    break;

                case SessionState.Unstable:
                    newColor = Color.red;
                    break;

                default:
                    break;
            }

            this._cameraAvatar.SetCameraAvatarMaterialColor(newColor);
        }

        private IEnumerator DelaySendCameraFrame() {
            yield return new WaitForEndOfFrame();
            this._streamingRenderTexture = this._streamingCameraCapture?.GetRenderTexture();
            Graphics.Blit(this._streamingRenderTexture, this._streamingRenderTextureDest, new Vector2(1, -1), new Vector2(0, 1));
            int returnCode = AudienceSDK.Audience.Context.SendCameraTexture(this._mappingId, this._streamingRenderTextureDest.GetNativeTexturePtr());
        }
    }
}
