using UnityEngine;

namespace AudienceSDK {
    public class DefaultEmojiBehaviour : MonoBehaviour {
        private Vector3 _startPosition;
        private float _duration = 2.0f;

        private float _startTime;

        private void Start() {
            this._startTime = Time.time;
            var mainCamera = UnityEngine.Camera.main;
            if (mainCamera != null) {

                var playerPos = new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z);
                Vector3 forward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
                Vector3 right = Vector3.Cross(Vector3.up, forward);
                this.transform.position = playerPos + (Random.Range(-1.5f, 1.5f) * right) + ((1 * forward) + (Random.Range(0.0f, 1.5f) * Vector3.up));
                this.transform.Rotate(Vector3.up, 90);
            } else {
                this.transform.localPosition += new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(0.0f, 1.5f), 1);
                this.transform.localEulerAngles = new Vector3(0, 90, 0);
            }

            this._startPosition = this.transform.position;

            this.SetMaterial();

            UnityEngine.Object.Destroy(this.gameObject, this._duration);
        }

        private void Update() {
            this.transform.position = this._startPosition + new Vector3(0, Mathf.Sin((Time.time - this._startTime) * Mathf.PI / 2), 0);
            if (Time.time - this._startTime > 1) {

                this.transform.localEulerAngles += new Vector3(0, 5, 0);
            }
        }

        private void SetMaterial() {
            var shader = Shader.Find(UserConfig.DefaultEmojiShader);
            Material mat;
            if (shader != null) {

                mat = new Material(shader);
                mat.SetColor("_Color", new Color(1.0f, 0f, 0f, 0.25f));
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            } else {
                shader = Shader.Find("Standard");
                mat = new Material(shader);
                mat.SetColor("_Color", new Color(1.0f, 0f, 0f, 0.25f));
            }

            this.gameObject.GetComponent<MeshRenderer>().material = mat;
            this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = mat;
        }
    }
}
