using System;
using System.Collections;
using UnityEngine;

namespace AudienceSDK {
    public class SceneManager : MonoBehaviour {

        /// <summary>
        /// State of Scene Manager process scene objects.
        /// </summary>
        public enum SceneManagerState {

            /// <summary>
            /// State Unload
            /// </summary>
            Unload = 0,

            /// <summary>
            /// State Loading
            /// </summary>
            Loading = 1,

            /// <summary>
            /// State Loaded
            /// </summary>
            Loaded = 2,

            /// <summary>
            /// State Unloading
            /// </summary>
            Unloading = 3,
        }

        public Action<SceneManagerState> CurrentSceneManagerStateChanged;

        public SceneManagerState CurrentSceneManagerState {
            get {
                return this._sceneManagerState;
            }

            private set {
                this._sceneManagerState = value;
                this.CurrentSceneManagerStateChanged?.Invoke(this._sceneManagerState);
            }
        }

        private SceneManagerState _sceneManagerState = SceneManagerState.Unload;

        public SceneManager() {
            // Plugin.Log.Info("SceneManager Construct");
        }

        public void LoadScene(Scene sceneObject) {
            this.CurrentSceneManagerState = SceneManagerState.Loading;
            this.StartCoroutine(this.LoadSceneCoroutine(sceneObject));
        }

        public void UnloadScene() {
            this.CurrentSceneManagerState = SceneManagerState.Unloading;
            this.StartCoroutine(this.UnloadSceneCoroutine());
        }

        public void HandlePeerMessageDataReceived(string streamSessionId, NativePeerMessageData peerMessageData) {
            Debug.Log("HandlePeerMessageDataReceived peerId =" + peerMessageData.PeerId);
            Debug.Log("HandlePeerMessageDataReceived msg =" + peerMessageData.Msg);
        }

        private void Awake() {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }

        private IEnumerator LoadSceneCoroutine(Scene scene) {
            var cs = Resources.FindObjectsOfTypeAll<AudienceCameraBehaviourBase>();
            foreach (var c in cs) {

                CameraUtilities.DestroyCamera(c);
                yield return null;
            }

            if (scene.cameras != null) {

                foreach (var c in scene.cameras) {

                    CameraUtilities.CreateCamera(c);
                    yield return null;
                }
            }

            this.CurrentSceneManagerState = SceneManagerState.Loaded;
        }

        private IEnumerator UnloadSceneCoroutine() {
            var cs = Resources.FindObjectsOfTypeAll<AudienceCameraBehaviourBase>();
            foreach (var c in cs) {

                CameraUtilities.DestroyCamera(c);
                yield return null;
            }

            this.CurrentSceneManagerState = SceneManagerState.Unload;
        }
    }
}
