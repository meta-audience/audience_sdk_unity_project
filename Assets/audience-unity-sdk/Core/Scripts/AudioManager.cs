using UnityEngine;

namespace AudienceSDK {
    public class AudioManager : MonoBehaviour {
        private string _preOldDeviceId = string.Empty;
        private string _preNewDeviceId = string.Empty;

        public void DefaultAudioDeviceChanged(string oldDeviceId, string newDeviceId) {
            Debug.Log("pre Old:" + this._preOldDeviceId + " ,Old:" + oldDeviceId);
            Debug.Log("pre New:" + this._preNewDeviceId + " ,New:" + newDeviceId);

            // prevent same informations sent back to back.
            if (this._preOldDeviceId != oldDeviceId || this._preNewDeviceId != newDeviceId) {

                Audience.Context.OnDefaultAudioDeviceChangedAlert?.Invoke();
                this._preOldDeviceId = oldDeviceId;
                this._preNewDeviceId = newDeviceId;
            }
        }

        public void OnUnloadScene() {
            this._preOldDeviceId = string.Empty;
            this._preNewDeviceId = string.Empty;
        }

        private void Awake() {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }
    }
}
