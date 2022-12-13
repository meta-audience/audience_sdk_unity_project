using UnityEngine;

namespace AudienceSDK {
    public class CameraAvatar : MonoBehaviour {
        private const float _cameraAvatarPreviewSize = 0.15f;

        private GameObject _cameraAvatarGO;
        private GameObject _cameraAvatarFrontPreviewGO;
        private GameObject _cameraAvatarBackPreviewGO;
        private Material _cameraAvatarMaterial;
        private Material _cameraAvatarPreviewMaterial;

        public void InitCameraAvatar(Camera camera) {
            this.InitCameraAvatarMaterial();
            switch ((CaptureType)camera.capture_type) {
                case CaptureType._3D_360:
                case CaptureType._3D_180:
                case CaptureType._2D_360:
                case CaptureType._2D_180:
                    this.SetAvatarPrimitive(PrimitiveType.Sphere);
                    break;
                case CaptureType._3D_Flat:
                case CaptureType._2D_Flat:
                default:
                    this.SetAvatarPrimitive(PrimitiveType.Cube);
                    break;
            }

            this.InitCameraAvatarPreview(camera);
        }

        public void DeinitCameraAvatar() {
            if (this._cameraAvatarGO != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarGO);
            }

            if (this._cameraAvatarFrontPreviewGO != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarFrontPreviewGO);
            }

            if (this._cameraAvatarBackPreviewGO != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarBackPreviewGO);
            }

            if (this._cameraAvatarMaterial != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarMaterial);
            }

            if (this._cameraAvatarPreviewMaterial != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarPreviewMaterial);
            }
        }

        public void SetCameraAvatarPreviewTexture(Texture previewTexture) {
            if (this._cameraAvatarPreviewMaterial == null) {

                Debug.LogError("Camera Avatar Preview Material not init.");
                return;
            }

            this._cameraAvatarPreviewMaterial.SetTexture("_MainTex", previewTexture);
        }

        public void SetCameraAvatarMaterialColor(Color color) {
            if (this._cameraAvatarGO == null || this._cameraAvatarGO.GetComponent<MeshRenderer>() == null || this._cameraAvatarGO.GetComponent<MeshRenderer>().material == null) {

                Debug.LogError("Camera Avatar Material not init.");
                return;
            }

            this._cameraAvatarGO.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }

        public Transform GetCameraAvatarTransform() {
            if (this._cameraAvatarGO != null) {

                return this._cameraAvatarGO.transform;
            }

            return null;
        }

        internal void SetAvatarPrimitive(PrimitiveType avatarPrimitiveTypeType) {
            if (this._cameraAvatarGO != null) {

                UnityEngine.Object.DestroyImmediate(this._cameraAvatarGO);
            }

            var avatarSize = Vector3.one;
            switch (avatarPrimitiveTypeType) {
                case PrimitiveType.Cube:
                    avatarSize = new Vector3(0.15f, 0.15f, 0.22f);
                    break;
                case PrimitiveType.Sphere:
                    avatarSize = new Vector3(0.15f, 0.15f, 0.15f);
                    break;
                default:
                    Debug.LogWarning("Unsupported PrimitiveType");
                    break;
            }

            this._cameraAvatarGO = GameObject.CreatePrimitive(avatarPrimitiveTypeType);
            this._cameraAvatarGO.name = "CameraAvatar";
            UnityEngine.Object.DontDestroyOnLoad(this._cameraAvatarGO);
            this._cameraAvatarGO.SetActive(true);
            var avatarTransform = this._cameraAvatarGO.transform;
            avatarTransform.parent = this.transform;
            avatarTransform.localScale = avatarSize;
            avatarTransform.localPosition = Vector3.zero;
            avatarTransform.localRotation = Quaternion.identity;
            avatarTransform.GetComponent<MeshRenderer>().material = this._cameraAvatarMaterial;
        }

        internal void SetAvatarPreviewSize(float ratio) {
            this._cameraAvatarFrontPreviewGO.transform.localScale = new Vector3(ratio, 1, 1) * _cameraAvatarPreviewSize;
            this._cameraAvatarFrontPreviewGO.transform.localPosition = new Vector3(-1f * (((ratio - 1) / 2) + 1) * _cameraAvatarPreviewSize, 0, 0);
            this._cameraAvatarFrontPreviewGO.transform.localEulerAngles = Vector3.zero;

            this._cameraAvatarBackPreviewGO.transform.localScale = new Vector3(ratio, 1, 1) * _cameraAvatarPreviewSize;
            this._cameraAvatarBackPreviewGO.transform.localPosition = new Vector3(-1f * (((ratio - 1) / 2) + 1) * _cameraAvatarPreviewSize, 0, 0.001f);
            this._cameraAvatarBackPreviewGO.transform.localEulerAngles = new Vector3(0, 180, 0);

            var filter = this._cameraAvatarBackPreviewGO.GetComponent<MeshFilter>();
            if (filter != null && filter.mesh != null) {

                filter.mesh.uv = new Vector2[] { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) };
            } else {
                Debug.LogError("Camera Avatar Preview mesh unusual.");
            }
        }

        private void Awake() {
        }

        private void InitCameraAvatarPreview(Camera camera) {
            if (this._cameraAvatarPreviewMaterial == null) {
                var shader = Shader.Find(UserConfig.DefaultPreviewQuadShader);
                if (shader != null) {

                    this._cameraAvatarPreviewMaterial = new Material(shader);
                } else {
                    shader = Shader.Find("Standard");
                    this._cameraAvatarPreviewMaterial = new Material(shader);
                }
            }

            this._cameraAvatarFrontPreviewGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
            UnityEngine.Object.DontDestroyOnLoad(this._cameraAvatarFrontPreviewGO);
            UnityEngine.Object.DestroyImmediate(this._cameraAvatarFrontPreviewGO.GetComponent<Collider>());
            this._cameraAvatarFrontPreviewGO.GetComponent<MeshRenderer>().material = this._cameraAvatarPreviewMaterial;
            this._cameraAvatarFrontPreviewGO.transform.parent = this.transform;

            this._cameraAvatarBackPreviewGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
            UnityEngine.Object.DontDestroyOnLoad(this._cameraAvatarBackPreviewGO);
            UnityEngine.Object.DestroyImmediate(this._cameraAvatarBackPreviewGO.GetComponent<Collider>());
            this._cameraAvatarBackPreviewGO.GetComponent<MeshRenderer>().material = this._cameraAvatarPreviewMaterial;
            this._cameraAvatarBackPreviewGO.transform.parent = this.transform;

            this.SetAvatarPreviewSize((float)camera.texture_width / (float)camera.texture_height);
        }

        private void InitCameraAvatarMaterial() {
            var shader = Shader.Find(UserConfig.DefaultCamAvatarShader);
            if (shader != null) {

                this._cameraAvatarMaterial = new Material(shader);
                this._cameraAvatarMaterial.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1.0f));
                this._cameraAvatarMaterial.SetFloat("_Mode", 1);
                this._cameraAvatarMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                this._cameraAvatarMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                this._cameraAvatarMaterial.SetInt("_ZWrite", 0);
                this._cameraAvatarMaterial.DisableKeyword("_ALPHATEST_ON");
                this._cameraAvatarMaterial.EnableKeyword("_ALPHABLEND_ON");
                this._cameraAvatarMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                this._cameraAvatarMaterial.renderQueue = 3000;
            } else {
                shader = Shader.Find("Standard");
                this._cameraAvatarMaterial = new Material(shader);
                this._cameraAvatarMaterial.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1.0f));
            }
        }
    }
}
