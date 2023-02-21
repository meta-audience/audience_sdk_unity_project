using UnityEngine;

namespace AudienceSDK {
    public class CameraAvatar : MonoBehaviour {
        private const float _cameraAvatarPreviewSize = 0.15f;

        private GameObject _cameraAvatarGO = null;
        private GameObject _cameraAvatarShapeGO = null;
        private GameObject _cameraAvatarFrontPreviewGO = null;
        private GameObject _cameraAvatarBackPreviewGO = null;

        private Material _cameraAvatarShapeMaterial;
        private Material _cameraAvatarPreviewMaterial;

        public void InitCameraAvatar(Camera camera) {

            var avatarPrefab = Resources.Load<GameObject>("Audience/CameraAvatar/CameraAvatarPrefab");
            if (avatarPrefab != null)
            {
                this._cameraAvatarGO = GameObject.Instantiate(avatarPrefab);
                this._cameraAvatarGO.transform.SetParent(this.transform, false);
                switch ((CaptureType)camera.capture_type)
                {
                    case CaptureType._3D_360:
                    case CaptureType._3D_180:
                    case CaptureType._2D_360:
                    case CaptureType._2D_180:
                        this._cameraAvatarShapeGO = this._cameraAvatarGO.transform.Find("Shapes/Sphere").gameObject;
                        break;
                    case CaptureType._3D_Flat:
                    case CaptureType._2D_Flat:
                    default:
                        this._cameraAvatarShapeGO = this._cameraAvatarGO.transform.Find("Shapes/Cube").gameObject;
                        break;
                }

                this._cameraAvatarShapeGO.SetActive(true);
                this._cameraAvatarShapeMaterial = this._cameraAvatarShapeGO.GetComponent<MeshRenderer>().material;
            }

            this.InitCameraAvatarPreview(camera);
        }

        public void DeinitCameraAvatar() {
            if (this._cameraAvatarGO != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarGO);
            }

            if (this._cameraAvatarShapeGO != null)
            {

                UnityEngine.Object.Destroy(this._cameraAvatarShapeGO);
            }

            if (this._cameraAvatarFrontPreviewGO != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarFrontPreviewGO);
            }

            if (this._cameraAvatarBackPreviewGO != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarBackPreviewGO);
            }

            if (this._cameraAvatarShapeMaterial != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarShapeMaterial);
            }

            if (this._cameraAvatarPreviewMaterial != null) {

                UnityEngine.Object.Destroy(this._cameraAvatarPreviewMaterial);
            }
        }

        public void SwitchCameraAvatarShapeCollider(bool enable) {
            if (this._cameraAvatarShapeGO == null) {

                Debug.LogError("Camera Avatar Shape not init.");
                return;
            }

            this._cameraAvatarShapeGO.GetComponent<Collider>().enabled = enable;
        }

        public void SetCameraAvatarPreviewTexture(Texture previewTexture) {
            if (this._cameraAvatarPreviewMaterial == null) {

                Debug.LogError("Camera Avatar Preview Material not init.");
                return;
            }

            this._cameraAvatarPreviewMaterial.SetTexture("_MainTex", previewTexture);
        }

        public void SetCameraAvatarMaterialColor(Color color) {
            if (this._cameraAvatarShapeMaterial == null)
            {

                Debug.LogError("Camera Avatar Shape Material not init.");
                return;
            }

            this._cameraAvatarShapeMaterial.SetColor("_Color", color);
        }

        public Transform GetCameraAvatarTransform() {
            if (this._cameraAvatarGO != null) {

                return this._cameraAvatarGO.transform;
            }

            return null;
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
                this._cameraAvatarPreviewMaterial = new Material(Resources.Load<Material>("Audience/CameraAvatar/camera_avatar_preview"));
            }

            this._cameraAvatarFrontPreviewGO = this._cameraAvatarGO.transform.Find("Previews/Front").gameObject;
            this._cameraAvatarFrontPreviewGO.GetComponent<MeshRenderer>().material = this._cameraAvatarPreviewMaterial;

            this._cameraAvatarBackPreviewGO = this._cameraAvatarGO.transform.Find("Previews/Back").gameObject;
            this._cameraAvatarBackPreviewGO.GetComponent<MeshRenderer>().material = this._cameraAvatarPreviewMaterial;

            this.SetAvatarPreviewSize((float)camera.texture_width / (float)camera.texture_height);
        }
    }
}
