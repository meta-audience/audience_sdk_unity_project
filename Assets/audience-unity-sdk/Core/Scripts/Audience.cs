using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using AudienceSDK.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.XR;

namespace AudienceSDK {

    public class Audience {
        private class CallbackObject {
            public Action Callback { get; set; }

            public CallbackObject(Action callback) {
                this.Callback = callback;
            }
        }

        public static Action<bool> AudienceInitStateChanged;

        public static bool AudienceInited
        {
            get
            {
                return _audienceInited;
            }

            private set
            {
                if (_audienceInited != value)
                {
                    _audienceInited = value;
                    AudienceInitStateChanged?.Invoke(_audienceInited);
                }
            }
        }

        public static Context Context => _context;

        internal static WeakReferenceTable _table => _context?._table;

        private static bool _audienceInited = false;

        private static Context _context = null;

        private static SynchronizationContext _syncContext;

        public static void Initialize() {
            _context = Context.Create();
            _syncContext = SynchronizationContext.Current;

            NativeMethods.Init();

            NativeMethods.RegisterErrorOccured(OnErrorOccured);
            NativeMethods.RegisterProfileListStateChanged(OnSceneListStateChanged);
            NativeMethods.RegisterRefreshProfileListCompleted(OnRefreshSceneListCompleted);
            NativeMethods.RegisterStreamStateChanged(OnStreamStateChanged);
            NativeMethods.RegisterSignalingConnectionStateChanged(OnSignalingConnectionStateChanged);
            NativeMethods.RegisterAudioStateChanged(OnAudioStateChanged);
            NativeMethods.RegisterDefaultAudioDeviceChanged(OnDefaultAudioDeviceChanged);
            NativeMethods.RegisterLoadProfileCompleted(OnLoadSceneCompleted);
            NativeMethods.RegisterUnloadProfileCompleted(OnUnloadSceneCompleted);
            NativeMethods.RegisterSessionStateChanged(OnSessionStateChanged);
            NativeMethods.RegisterPeerInfoListChanged(OnPeerInfoListChanged);
            NativeMethods.RegisterPeerMessageReceived(OnPeerMessageReceived);
            NativeMethods.RegisterProfileStateChanged(OnSceneStateChanged);
            NativeMethods.RegisterProfileChanged(OnSceneChanged);
            NativeMethods.RegisterLogonStateChanged(LogonStateChanged);
            NativeMethods.RegisterLoginCompleted(LoginCompleted);
            NativeMethods.RegisterLogoutCompleted(LogoutCompleted);
            NativeMethods.RegisterSaveProfileCompleted(SaveSceneCompleted);
            NativeMethods.RegisterChatConnectionStateChanged(ChatConnectionStateChanged);
            NativeMethods.RegisterChatMessageReceived(ChatMessageReceived);
            NativeMethods.RegisterPlatformConnectionChanged(PlatformConnectionStateChanged);
            NativeMethods.RegisterChatMuteEmojiChanged(ChatMuteEmojiChanged);

            var contextObject = new GameObject("Audience_Runtime");
            Context.SceneManager = contextObject.AddComponent<SceneManager>();
            Context.AudioManager = contextObject.AddComponent<AudioManager>();
            Context.BundleManager = contextObject.AddComponent<BundleManager>();
            Context.ChatMessageManager = contextObject.AddComponent<ChatMessageManager>();
            Context.EmojiEffectManager = contextObject.AddComponent<EmojiEffectManager>();
            Context.EmojiAvatarManager = contextObject.AddComponent<EmojiAvatarManager>();
            Context.EmojiSentencesInAuthorFactory = new EmojiSentencesInAuthorFactory();
            Context.EmojiAuthorManager = new EmojiAuthorManager(Context.EmojiSentencesInAuthorFactory);
            Context.LogonStateChanged += Context.SetCurrentLogonState;
            Context.SceneListStateChanged += Context.SetCurrentSceneListState;
            Context.StreamStateChanged += Context.SetCurrentStreamState;
            Context.SignalingConnectionStateChanged += Context.SetCurrentSignalingConnectionState;
            Context.SceneStateChanged += Context.SetCurrentSceneState;
            Context.ChatConnectionStateChanged += Context.SetCurrentChatConnectionState;
            Context.PlatformConnectionStateChanged += Context.SetCurrentPlatformConnectionState;
            Context.ChatMessageReceived += Context.ChatMessageManager.HandleChatMessageReceived;
            Context.ChatMuteEmojiChanged += Context.EmojiAuthorManager.OnMuteEmojiChanged;
            Context.LoadSceneCompleted += Context.SceneManager.LoadScene;
            Context.UnloadSceneCompleted += Context.SceneManager.UnloadScene;
            Context.PeerMessageReceived += Context.SceneManager.HandlePeerMessageDataReceived;
            Context.UnloadSceneCompleted += Context.AudioManager.OnUnloadScene;
            Context.DefaultAudioDeviceChanged += Context.AudioManager.DefaultAudioDeviceChanged;

            // set default language "en-us".
            Context.SetCurrentCultureName(Context.GetCurrentCultureName());

            var executingAssembly = Assembly.GetExecutingAssembly();
            var audienceUnityVersion = executingAssembly.GetName().Version;

            Debug.Log("Audience ipa version:" + audienceUnityVersion);
            Context.AudienceUnityVersion = audienceUnityVersion.ToString();

            int versionBufferSize = 20;
            var audiencePluginVersion = new StringBuilder(versionBufferSize);

            var rc = NativeMethods.GetPluginVersion(audiencePluginVersion, ref versionBufferSize);
            if (rc == (int)AudienceReturnCode.AudienceBufferSizeNotEnough) {

                Debug.Log("Plugin version buffer not enough, resize to " + versionBufferSize);
                rc = NativeMethods.GetPluginVersion(audiencePluginVersion, ref versionBufferSize);
            }

            if (rc == (int)AudienceReturnCode.AudienceSDKOk) {

                Debug.Log("Plugin version:" + audiencePluginVersion);
                Context.AudiencePluginVersion = audiencePluginVersion.ToString();
            } else {
                Debug.LogError("Get plugin version fail, rc = {}" + rc);
            }

            // Set user behavior info.
            var inputDevices = new List<InputDevice>();
            UnityEngine.XR.InputDevices.GetDevices(inputDevices);
            var headsetDeviceIndex = inputDevices.FindIndex(x =>
                (x.characteristics & InputDeviceCharacteristics.HeadMounted) == InputDeviceCharacteristics.HeadMounted);
            if (headsetDeviceIndex != -1) {
                NativeMethods.SetUserBehaviorInfo(
                    UserBehaviorInfoType.HeadsetDevice,
                    inputDevices[headsetDeviceIndex].name.ToString());
            } else {
                NativeMethods.SetUserBehaviorInfo(UserBehaviorInfoType.HeadsetDevice, "Play without headset.");
            }

            UserConfig.LoadUserConfig();
            AudienceInited = true;
        }

        public static void Deinitialize()
        {
            AudienceInited = false;
            AudienceSDK.Audience.Context.Stop();
            NativeMethods.DeInit();
        }

        public static void Dispose() {
            Context.LogonStateChanged -= Context.SetCurrentLogonState;
            Context.SceneListStateChanged -= Context.SetCurrentSceneListState;
            Context.StreamStateChanged -= Context.SetCurrentStreamState;
            Context.SignalingConnectionStateChanged -= Context.SetCurrentSignalingConnectionState;
            Context.SceneStateChanged -= Context.SetCurrentSceneState;
            Context.ChatConnectionStateChanged -= Context.SetCurrentChatConnectionState;
            Context.PlatformConnectionStateChanged -= Context.SetCurrentPlatformConnectionState;
            Context.ChatMessageReceived -= Context.ChatMessageManager.HandleChatMessageReceived;
            Context.ChatMuteEmojiChanged -= Context.EmojiAuthorManager.OnMuteEmojiChanged;
            Context.LoadSceneCompleted -= Context.SceneManager.LoadScene;
            Context.UnloadSceneCompleted -= Context.SceneManager.UnloadScene;
            Context.PeerMessageReceived -= Context.SceneManager.HandlePeerMessageDataReceived;
            Context.UnloadSceneCompleted -= Context.AudioManager.OnUnloadScene;
            Context.DefaultAudioDeviceChanged -= Context.AudioManager.DefaultAudioDeviceChanged;

            NativeMethods.DeInit();

            if (_context != null) {

                _context.Dispose();
                _context = null;
            }

            _syncContext = null;
        }

        public static void Sync(Action callback) {
            _syncContext.Post(SendOrPostCallback, new CallbackObject(callback));
        }

        private static void SendOrPostCallback(object state) {
            var obj = state as CallbackObject;
            if (_context == null) {

                return;
            }

            obj.Callback();
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateErrorOccured))]
        private static void OnErrorOccured(int errorCode, string errorMessage) {
            Audience.Sync(() => {
                string errorMessageDesc = Audience.Context.GetLanguageDisplayString(errorMessage);
                Debug.Log("code = " + errorCode + ", message = " + errorMessage + ", description = " + errorMessageDesc);
                Context.ErrorOccured?.Invoke(errorCode, errorMessageDesc);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateSceneListStateChanged))]
        private static void OnSceneListStateChanged(SceneListState state) {
            Audience.Sync(() => {
                Debug.Log("Audience SceneListStateChanged Callback state = " + state);
                Context.SceneListStateChanged?.Invoke(state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateRefreshSceneListCompleted))]
        private static void OnRefreshSceneListCompleted(IntPtr list, int size) {
            List<NativeSceneSummaryData> dataList = new List<NativeSceneSummaryData>();
            for (int i = 0; i < size; i++) {

                // Plugin.Log.Info("Audience OnRefreshSceneListCompleted i:" + i);
                NativeSceneSummaryData x;
                IntPtr ptr = Marshal.ReadIntPtr(list, Marshal.SizeOf(typeof(IntPtr)) * i);
                x = (NativeSceneSummaryData)Marshal.PtrToStructure(ptr, typeof(NativeSceneSummaryData));
                dataList.Add(x);
            }

            Audience.Sync(() => {
                Debug.Log("Audience RefreshSceneListCompleted Callback");
                Context.RefreshSceneListCompleted?.Invoke(dataList);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateStreamStateChanged))]
        private static void OnStreamStateChanged(StreamState state) {
            Audience.Sync(() => {
                Debug.Log("Stream_state = " + state);
                Context.StreamStateChanged?.Invoke(state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateSignalingConnectionStateChanged))]
        private static void OnSignalingConnectionStateChanged(string streamSessionId, SignalingConnectionState state) {
            Audience.Sync(() => {
                Debug.Log("streamSessionId = " + streamSessionId + ", Signaling Connection State = " + state);
                Context.SignalingConnectionStateChanged?.Invoke(state);

                string stateString = Audience.Context.GetLanguageDisplayString(state.GetDescription());
                string messageContent = Audience.Context.GetLanguageDisplayString("SIGNALING_CONNECTION_STATE") + stateString + ". ";
                if (state == SignalingConnectionState.Failed) {

                    Context.SignalingServerConnectionFailed?.Invoke(streamSessionId, messageContent);
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateAudioStateChanged))]
        private static void OnAudioStateChanged(string device_id, AudioState state) {
            Audience.Sync(() => {
                Debug.Log("audio device id : [" + device_id + "]  state = " + state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateDefaultAudioDeviceChanged))]
        private static void OnDefaultAudioDeviceChanged(string oldDeviceId, string newDeviceId) {
            Audience.Sync(() => {
                Context.DefaultAudioDeviceChanged?.Invoke(oldDeviceId, newDeviceId);

                // Audience.Context.AddMessageInvoke("INFO", "UILOG_DEFAULT_AUDIO_CHANGED");
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateLoadSceneCompleted))]
        private static void OnLoadSceneCompleted(string output) {
            Audience.Sync(() => {
                Context.CurrentSceneObject = JsonConvert.DeserializeObject<Scene>(output);
                Context.LoadSceneCompleted?.Invoke(Context.CurrentSceneObject);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateUnloadSceneCompleted))]
        private static void OnUnloadSceneCompleted() {
            Audience.Sync(() => {
                Debug.Log("UnloadCompleted");
                Context.UnloadSceneCompleted?.Invoke();
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateSessionStateChanged))]
        private static void OnSessionStateChanged(string streamSessionId, SessionState state) {
            Audience.Sync(() => {
                Debug.Log("OnSessionStateChanged: " + streamSessionId + "," + state);
                Context.SessionStateChanged?.Invoke(streamSessionId, state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegatePeerInfoListChanged))]
        private static void OnPeerInfoListChanged(string streamSessionId, PeerInfoListChangedType peerInfoListChangedType, IntPtr peerInfo) {
            NativePeerInfo peerInformation = (NativePeerInfo)Marshal.PtrToStructure(peerInfo, typeof(NativePeerInfo));
            Audience.Sync(() => {
                Debug.Log("OnPeerInfoListChanged: " + streamSessionId + "," + peerInfoListChangedType);
                Debug.Log("OnPeerInfoListChanged: " + peerInformation.PeerId + "," + peerInformation.PeerName + "," + peerInformation.VideoId);
                Context.PeerInfoListChanged?.Invoke(streamSessionId, peerInfoListChangedType, peerInformation);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegatePeerMessageReceived))]
        private static void OnPeerMessageReceived(string streamSessionId, PeerMessageType peerMessageType, IntPtr peerMessage) {
            Audience.Sync(() => {
                Debug.Log("OnPeerMessageReceived: streamSessionId = " + streamSessionId + ", peerMessageType = " + peerMessageType);
                switch (peerMessageType) {
                    case PeerMessageType.Camera:
                        NativePeerCameraData peerCameraData = (NativePeerCameraData)Marshal.PtrToStructure(peerMessage, typeof(NativePeerCameraData));
                        Debug.Log("PeerMessageType.Camera:" + peerCameraData.PeerId + ", " + peerCameraData.VideoId + "," + peerCameraData.Position + "," + peerCameraData.Rotation);
                        Context.PeerCameraDataReceived?.Invoke(streamSessionId, peerCameraData);
                        break;
                    case PeerMessageType.Emoji:
                        NativePeerEmojiData peerEmojiData = (NativePeerEmojiData)Marshal.PtrToStructure(peerMessage, typeof(NativePeerEmojiData));
                        Debug.Log("PeerMessageType.Emoji: " + peerEmojiData.PeerId + ", " + peerEmojiData.VideoId + "," + peerEmojiData.EmojiType);
                        Context.PeerEmojiDataReceived?.Invoke(streamSessionId, peerEmojiData);
                        break;
                    case PeerMessageType.Message:
                    default:
                        NativePeerMessageData peerMessageData = (NativePeerMessageData)Marshal.PtrToStructure(peerMessage, typeof(NativePeerMessageData));
                        Debug.Log("PeerMessageType.Message: " + peerMessageData.PeerId + ", " + peerMessageData.Msg);
                        Context.PeerMessageReceived?.Invoke(streamSessionId, peerMessageData);
                        break;
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateSceneStateChanged))]
        private static void OnSceneStateChanged(SceneState state) {
            Audience.Sync(() => {
                Debug.Log("OnSceneStateChanged: scene_state = " + state);
                Context.SceneStateChanged?.Invoke(state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateSceneChanged))]
        private static void OnSceneChanged() {
            Audience.Sync(() => {
                Debug.Log("OnSceneChanged: scene change");
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateLogonStateChanged))]
        private static void LogonStateChanged(LogonState state) {
            Audience.Sync(() => {
                Debug.Log("Audience LogonStateChanged: state =" + state);
                Context.LogonStateChanged?.Invoke(state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateLoginCompleted))]
        private static void LoginCompleted(NativeUserData userData) {

            Audience.Sync(() => {
                Debug.Log("Audience LoginCompleted Callback");
                Context.CurrentUserData.Nickname = userData.Nickname;
                Context.CurrentUserData.LoginUId = userData.LoginUId;
                Context.LoginCompleted?.Invoke(userData);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateLogoutCompleted))]
        private static void LogoutCompleted() {
            Audience.Sync(() => {
                Debug.Log("Audience LogoutCompleted Callback");
                Context.LogoutCompleted?.Invoke();
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateSaveSceneCompleted))]
        private static void SaveSceneCompleted(string output) {
            Audience.Sync(() => {
                Debug.Log("Audience SaveSceneCompleted Callback");
                var sceneObj = JsonConvert.DeserializeObject<Scene>(output);
                Context.SaveSceneCompleted?.Invoke(sceneObj);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateChatConnectionStateChanged))]
        private static void ChatConnectionStateChanged(ChatConnectionState state) {
            Audience.Sync(() => {
                Context.ChatConnectionStateChanged?.Invoke(state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateChatMessageReceived))]
        private static void ChatMessageReceived(string output) {
            Audience.Sync(() => {
                Context.ChatMessageReceived?.Invoke(output);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegatePlatformConnectionStateChanged))]
        private static void PlatformConnectionStateChanged(string platform, PlatformConnectionState state) {
            Audience.Sync(() => {
                Context.PlatformConnectionStateChanged?.Invoke(platform, state);
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DelegateChatEmojiMuteChanged))]
        private static void ChatMuteEmojiChanged(bool enable) {
            Audience.Sync(() => {
                Context.ChatMuteEmojiChanged?.Invoke(enable);
            });
        }
    }

    // UserData comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class UserData {
        public string Nickname { get; set; }

        public string LoginUId { get; set; }
    }

    // It will catch JsonConvert.DeserializeObject, naming will follow source json data.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]

    // AudioDevice comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class AudioDevice {
        public string audio_device_id { get; set; }

        public string device_id { get; set; }

        public string device_name { get; set; }

        public bool is_mute { get; set; }

        public double volume { get; set; }
    }

    // It will catch JsonConvert.DeserializeObject, naming will follow source json data.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]

    // Audios comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Audios {
        public List<AudioDevice> audio_devices { get; set; }

        public string audio_id { get; set; }

        public int channel_type { get; set; }

        public bool is_mute { get; set; }

        public int sample_rate { get; set; }

        public double volume { get; set; }
    }

    // It will catch JsonConvert.DeserializeObject, naming will follow source json data.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]

    // Camera comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Camera {
        public int capture_type { get; set; }

        public int mapping_id { get; set; }

        public string camera_name { get; set; }

        public float position_x { get; set; }

        public float position_y { get; set; }

        public float position_z { get; set; }

        public float rotation_x { get; set; }

        public float rotation_y { get; set; }

        public float rotation_z { get; set; }

        public int texture_height { get; set; }

        public int texture_width { get; set; }
    }

    // It will catch JsonConvert.DeserializeObject, naming will follow source json data.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]

    // CameraMapping comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class CameraMapping {
        public int mapping_id { get; set; }
    }

    // It will catch JsonConvert.DeserializeObject, naming will follow source json data.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]

    // StreamSession comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class StreamSession {
        public string stream_session_id { get; set; }

        public string stream_session_name { get; set; }

        public int stream_session_type { get; set; }

        public List<CameraMapping> cameras_mapping { get; set; }
    }

    // It will catch JsonConvert.DeserializeObject, naming will follow source json data.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]

    // SceneObject comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Scene {
        public List<Audios> audios { get; set; }

        public List<Camera> cameras { get; set; }

        public List<StreamSession> stream_sessions { get; set; }

        public int encoder { get; set; }

        public int encoder_codec { get; set; }

        public string last_modify { get; set; }

        public string scene_id { get; set; }

        public string scene_name { get; set; }

        public string room_title { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class SceneList {
        public List<Scene> scenes { get; set; }
    }

    public enum ChatMessageType {
        /// <summary>
        /// Text
        /// </summary>
        Text = 0,

        /// <summary>
        /// Emoji_2D
        /// </summary>
        Emoji_2D = 1,

        /// <summary>
        /// Emoji_3D
        /// </summary>
        Emoji_3D = 2,
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ChatMessage {
        public int type { get; set; }

        public string text { get; set; }

        public string url { get; set; }

        public List<EmojiRawObject> asset_list { get; set; }

        public EmojiAnimation animation { get; set; }

        public EmojiInteract interaction { get; set; }

        // public string url { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class EmojiRawObject {
        public string engine { get; set; }

        public string render_type { get; set; }

        public EmojiAsset asset { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class EmojiAsset {
        public string keyword { get; set; }

        public int version { get; set; }

        public string url { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class EmojiAnimation {
        public List<EmojiRawObject> asset_list { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class EmojiInteract {
        public string keyword_0 { get; set; }

        public string keyword_1 { get; set; }

        public string interaction_type { get; set; }

        public List<EmojiRawObject> animation_asset_list { get; set; }

        public List<EmojiRawObject> interaction_asset_list { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ChatAuthor {
        public string platform { get; set; }

        public string name { get; set; }

        public string user_id { get; set; }

        public bool is_mod { get; set; }

        public bool is_sub { get; set; }

        public string badges { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ChatData {
        public string chat_id { get; set; }

        public ChatAuthor author { get; set; }

        public string original_text { get; set; }

        public List<ChatMessage> message { get; set; }

        public string utc_timestamp { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ChatSchema {
        public string chat_group_id { get; set; }

        public ChatData data { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ChatChannel {
        public string channel_id { get; set; }

        public string title { get; set; }

        public int published_time { get; set; }

        public int scheduled_start_time { get; set; }

        public int actual_start_time { get; set; }

        public int status { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ChatChannelList {
        public List<ChatChannel> channels { get; set; }

        public string platform { get; set; }
    }
}
