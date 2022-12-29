using UnityEngine;

namespace AudienceSDK {
    public class AudienceCameraInstance {
        public AudienceCameraBehaviourBase Instance { get; set; }

        public Camera Camera {
            get {
                return this._camera;
            }
        }

        public GameObject GameObj {
            get {
                return this._gameObj;
            }
        }

        private Camera _camera;
        private GameObject _gameObj;

        public AudienceCameraInstance(Camera camera) {
            this._gameObj = new GameObject(string.Format("AudienceCamera_{0}", camera.mapping_id));
            this._camera = camera;
        }

        public void Init() {
            this.Instance.Init(this._camera);
        }
    }
}