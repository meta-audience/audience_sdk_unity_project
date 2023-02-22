using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using AudienceSDK.Scripts;
using Newtonsoft.Json;
using UnityEngine;

namespace AudienceSDK {

    /// <summary>
    /// Types of localization options.
    /// </summary>
    public enum LanguageEnum {

        /// <summary>
        /// Localization English
        /// </summary>
        English,

        /// <summary>
        /// Localization TraditionalChinese
        /// </summary>
        TraditionalChinese,

        /// <summary>
        /// Localization SimplifiedChinese
        /// </summary>
        SimplifiedChinese,
    }

    public class Context : IDisposable {

        public ResourceManager ResourceManager { get; set; }

        public string AudienceUnityVersion { get; set; }

        public string AudiencePluginVersion { get; set; }

        public UserData CurrentUserData { get; set; }

        public Scene CurrentSceneObject { get; set; }

        public SceneManager SceneManager { get; set; }

        public AudioManager AudioManager { get; set; }

        public BundleManager BundleManager { get; set; }

        public EmojiAuthorManager EmojiAuthorManager { get; set; }

        public EmojiSentencesInAuthorFactory EmojiSentencesInAuthorFactory { get; set; }

        public ChatMessageManager ChatMessageManager { get; set; }

        public EmojiEffectManager EmojiEffectManager { get; set; }

        public EmojiAvatarManager EmojiAvatarManager { get; set; }

        public Action<LogonState> LogonStateChanged { get; set; }

        public Action<int, string> ErrorOccured { get; set; }

        public Action<NativeUserData> LoginCompleted { get; set; }

        public Action LogoutCompleted { get; set; }

        public Action<Scene> LoadSceneCompleted { get; set; }

        public Action UnloadSceneCompleted { get; set; }

        public Action<SceneListState> SceneListStateChanged { get; set; }

        public Action<List<NativeSceneSummaryData>> RefreshSceneListCompleted { get; set; }

        public Action<StreamState> StreamStateChanged { get; set; }

        public Action<string, SessionState> SessionStateChanged { get; set; }

        public Action<SignalingConnectionState> SignalingConnectionStateChanged { get; set; }

        public Action<string, string> SignalingServerConnectionFailed { get; set; }

        public Action<Scene> SaveSceneCompleted { get; set; }

        public Action<SceneState> SceneStateChanged { get; set; }

        public Action<string, PeerInfoListChangedType, NativePeerInfo> PeerInfoListChanged { get; set; }

        public Action<string, NativePeerCameraData> PeerCameraDataReceived { get; set; }

        public Action<string, NativePeerEmojiData> PeerEmojiDataReceived { get; set; }

        public Action<string, NativePeerMessageData> PeerMessageReceived { get; set; }

        public Action<ChatConnectionState> ChatConnectionStateChanged { get; set; }

        public Action<string, PlatformConnectionState> PlatformConnectionStateChanged { get; set; }

        public Action<bool> ChatMuteEmojiChanged { get; set; }

        public Action<string> ChatMessageReceived { get; set; }

        public Action<int, string> LoginFailed { get; set; }

        public Action<int, string> LogoutFailed { get; set; }

        public Action<int, string> LoadSceneFailed { get; set; }

        public Action<int, string> UnloadSceneFailed { get; set; }

        public Action<string, string> DefaultAudioDeviceChanged { get; set; }

        public Action OnDefaultAudioDeviceChangedAlert { get; set; }

        public Action<string> OnScanSceneObjectError { get; set; }

        internal IntPtr _self { get; set; }

        internal WeakReferenceTable _table { get; set; }

        private bool _sendCameraFrame;
        private LogonState _logonState;
        private SceneListState _sceneListState;
        private StreamState _streamState;
        private SignalingConnectionState _signalingConnectionState;
        private SceneState _sceneState;
        private ChatConnectionState _chatConnectionState;
        private Dictionary<string, PlatformConnectionState> _platformConnectionState;
        private string _currentCultureName = "en-us";
        private ResourceSet _currentCultureResourceSet;

        public static Context Create() {
            return new Context();
        }

        public void Dispose() {
        }

        public int Login(string username, string password) {
            return NativeMethods.Login(username, password);
        }

        public int Logout() {
            return NativeMethods.Logout();
        }

        public int Init() {
            return NativeMethods.Init();
        }

        public int DeInit() {
            return NativeMethods.DeInit();
        }

        public int RegisterErrorOccured(DelegateErrorOccured callback) {
            return NativeMethods.RegisterErrorOccured(callback);
        }

        public int RegisterSceneListStateChanged(DelegateSceneListStateChanged callback) {
            return NativeMethods.RegisterProfileListStateChanged(callback);
        }

        public int RegisterRefreshSceneListCompleted(DelegateRefreshSceneListCompleted callback) {
            return NativeMethods.RegisterRefreshProfileListCompleted(callback);
        }

        public int RefreshSceneList() {
            return NativeMethods.RefreshProfileList();
        }

        public int RegisterStreamStateChanged(DelegateStreamStateChanged callback) {
            return NativeMethods.RegisterStreamStateChanged(callback);
        }

        public int RegisterAudioStateChanged(DelegateAudioStateChanged callback) {
            return NativeMethods.RegisterAudioStateChanged(callback);
        }

        public int RegisterDefaultAudioDeviceChanged(DelegateDefaultAudioDeviceChanged callback) {
            return NativeMethods.RegisterDefaultAudioDeviceChanged(callback);
        }

        public int RegisterLoadSceneCompleted(DelegateLoadSceneCompleted callback) {
            return NativeMethods.RegisterLoadProfileCompleted(callback);
        }

        public int RegisterUnloadSceneCompleted(DelegateUnloadSceneCompleted callback) {
            return NativeMethods.RegisterUnloadProfileCompleted(callback);
        }

        public int RegisterSessionStateChanged(DelegateSessionStateChanged callback) {
            return NativeMethods.RegisterSessionStateChanged(callback);
        }

        public int RegisterPeerInfoListChanged(DelegatePeerInfoListChanged callback) {
            return NativeMethods.RegisterPeerInfoListChanged(callback);
        }

        public int RegisterPeerMessageReceived(DelegatePeerMessageReceived callback) {
            return NativeMethods.RegisterPeerMessageReceived(callback);
        }

        public int LoadScene(string scene_id) {
            return NativeMethods.LoadProfile(scene_id);
        }

        public int UnloadScene() {
            return NativeMethods.UnloadProfile();
        }

        public int GetChatChannel(string platform, out AudienceSDK.ChatChannelList chatChannelList) {
            var channelListLength = 0;
            var channelListSize = 1024;
            var channelList = new StringBuilder(channelListSize);

            int rc = NativeMethods.GetChatChannel(platform, channelListSize, channelList, ref channelListLength);
            if (rc != (int)AudienceReturnCode.AudienceSDKOk && rc != (int)AudienceReturnCode.AudienceBufferSizeNotEnough) {
                chatChannelList = null;
                return rc;
            } else if (channelListSize > channelListLength) {
                chatChannelList = JsonConvert.DeserializeObject<ChatChannelList>(channelList.ToString());
                return rc;
            }

            channelList = new StringBuilder(channelListLength);
            rc = NativeMethods.GetChatChannel(platform, channelListLength, channelList, ref channelListLength);
            if (rc == (int)AudienceReturnCode.AudienceSDKOk) {
                chatChannelList = JsonConvert.DeserializeObject<ChatChannelList>(channelList.ToString());
            }
            else {
                chatChannelList = null;
            }

            return rc;
        }

        public int Start() {
            return NativeMethods.Start();
        }

        public int Stop() {
            return NativeMethods.Stop();
        }

        public int _StartStream() {
            return NativeMethods._StartStream();
        }

        public int _StopStream() {
            return NativeMethods._StopStream();
        }

        public int _StartChat() {
            return NativeMethods._StartChat();
        }

        public int _StopChat() {
            return NativeMethods._StopChat();
        }

        public int ConnectChatroom(string platform, string channel) {
            return NativeMethods.ConnectChatroom(platform, channel);
        }

        public int DisconnectChatroom(string platform) {
            return NativeMethods.DisconnectChatroom(platform);
        }

        public int SendChatMessage(string chatMessage) {
            return NativeMethods.SendChatMessage(chatMessage);
        }

        public int MuteEmoji(bool enable) {
            return NativeMethods.MuteEmoji(enable);
        }

        public void StartSendCameraFrame() {
            this._sendCameraFrame = true;
        }

        public void StopSendCameraFrame() {
            this._sendCameraFrame = false;
        }

        public bool SendCameraFrameNow() {
            return this._sendCameraFrame;
        }

        public void SetCurrentLogonState(LogonState state) {
            this._logonState = state;
        }

        public LogonState GetCurrentLogonState() {
            return this._logonState;
        }

        public void SetCurrentStreamState(StreamState state) {
            this._streamState = state;
        }

        public StreamState GetCurrentStreamState() {
            return this._streamState;
        }

        public void SetCurrentSignalingConnectionState(SignalingConnectionState state) {
            Debug.Log("SetCurrentSignalingConnectionState : state = " + state);
            this._signalingConnectionState = state;
        }

        public SignalingConnectionState GetCurrentSignalingConnectionState() {
            return this._signalingConnectionState;
        }

        public void SetCurrentSceneState(SceneState state) {
            this._sceneState = state;
        }

        public void SetCurrentCultureName(string cultureName) {
#if DLL_BUILD
            this._currentCultureName = cultureName.ToLower();
            try {
                CultureInfo ci = new CultureInfo(cultureName);
            } catch (CultureNotFoundException) {
                this._currentCultureName = "en-us";
            }

            if (this._currentCultureResourceSet != null) {

                this._currentCultureResourceSet.Close();
            }

            string resourceName = "AudienceSDK.Resources.Messages." + this._currentCultureName + ".resources";
            Debug.Log("SetCurrentCultureName : resourceName = " + resourceName);

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream == null) {
                Debug.Log("stream == null, load default resources.");
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AudienceSDK.Resources.Messages.Messages.resources");
                if (stream == null) {
                    Debug.Log("default resources is also null");
                    this._currentCultureResourceSet = null;
                } else {
                    this._currentCultureResourceSet = new ResourceSet(stream);
                    stream.Close();
                }
            } else {
                this._currentCultureResourceSet = new ResourceSet(stream);
                stream.Close();
            }
#else
            this._currentCultureResourceSet = null;
#endif
        }

        public string GetCurrentCultureName() {
            return this._currentCultureName;
        }

        public string GetLanguageDisplayString(string name, params object[] p) {

            string returnValue = string.Empty;
#if DLL_BUILD
            if (this._currentCultureResourceSet != null) {
                returnValue = this._currentCultureResourceSet.GetString(name);
                if (returnValue == null) {
                    // if can't found name, just return key.
                    returnValue = name;
                }
            } else {
                Debug.Log("this._currentCultureResourceSet == null");
            }

            return string.Format(returnValue, p);
#else
            returnValue = name;
            return string.Format(returnValue, p);
#endif
        }

        public SceneState GetCurrentSceneState() {
            return this._sceneState;
        }

        public void SetCurrentSceneListState(SceneListState state) {
            this._sceneListState = state;
        }

        public SceneListState GetCurrentSceneListState() {
            return this._sceneListState;
        }

        public void SetCurrentChatConnectionState(ChatConnectionState state) {
            this._chatConnectionState = state;
        }

        public ChatConnectionState GetCurrentChatConnectionState() {
            return this._chatConnectionState;
        }

        public void SetCurrentPlatformConnectionState(string platform, PlatformConnectionState state) {
            this._platformConnectionState[platform] = state;
        }

        public PlatformConnectionState GetCurrentPlatformConnectionState(string platform) {
            PlatformConnectionState state = PlatformConnectionState.Closed;
            this._platformConnectionState.TryGetValue(platform, out state);
            return state;
        }

        public IntPtr GetScene() {
            return NativeMethods.GetProfile();
        }

        public int GetSceneList(out AudienceSDK.SceneList sceneList) {
            var sceneListSize = 40960;
            var sceneListVar = new StringBuilder(sceneListSize);
            int rc = NativeMethods.GetProfileList(sceneListVar, ref sceneListSize);
            Debug.Log(sceneListVar.ToString());
            if (rc == (int)AudienceReturnCode.AudienceSDKOk) {
                sceneList = JsonConvert.DeserializeObject<SceneList>(sceneListVar.ToString());
            } else {
                sceneList = null;
            }

            return rc;
        }

        public int SaveScene(bool forceUpdate) {
            return NativeMethods.SaveProfile(forceUpdate);
        }

        public int SetCameraParam(int cameraMappingId, CameraParamType paramType, string value) {
            return NativeMethods.SetCameraParam(cameraMappingId, paramType, value);
        }

        public int SetAudioParam(int mappingId, AudioParamType paramType, string paramValue) {
            return NativeMethods.SetAudioParam(mappingId, paramType, paramValue);
        }

        public int SetAudioDeviceParam(int mappingId, string deviceId, AudioParamType paramType, string paramValue) {
            return NativeMethods.SetAudioDeviceParam(mappingId, deviceId, paramType, paramValue);
        }

        public int RegisterSceneStateChanged(DelegateSceneStateChanged callback) {
            return NativeMethods.RegisterProfileStateChanged(callback);
        }

        public int RegisterSceneChanged(DelegateSceneChanged callback) {
            return NativeMethods.RegisterProfileChanged(callback);
        }

        public int RegisterLogonStateChanged(DelegateLogonStateChanged callback) {
            return NativeMethods.RegisterLogonStateChanged(callback);
        }

        public int RegisterLoginCompleted(DelegateLoginCompleted callback) {
            return NativeMethods.RegisterLoginCompleted(callback);
        }

        public int RegisterLogoutCompleted(DelegateLogoutCompleted callback) {
            return NativeMethods.RegisterLogoutCompleted(callback);
        }

        public int RegisterSaveSceneCompleted(DelegateSaveSceneCompleted callback) {
            return NativeMethods.RegisterSaveProfileCompleted(callback);
        }

        public int RegisterChatConnectionStateChanged(DelegateChatConnectionStateChanged callback) {
            return NativeMethods.RegisterChatConnectionStateChanged(callback);
        }

        public int RegisterChatMessageReceived(DelegateChatMessageReceived callback) {
            return NativeMethods.RegisterChatMessageReceived(callback);
        }

        public int RegisterPlatformConnectionStateChanged(DelegatePlatformConnectionStateChanged callback) {
            return NativeMethods.RegisterPlatformConnectionChanged(callback);
        }

        public int RegisterChatMuteEmojiChanged(DelegateChatEmojiMuteChanged callback) {
            return NativeMethods.RegisterChatMuteEmojiChanged(callback);
        }

        public int SendCameraTexture(int cameraMappingId, IntPtr texture) {
            return NativeMethods.SendCameraTexture(cameraMappingId, texture);
        }

        public int GetRegisterPageURL(StringBuilder registerPageUrl, ref int registerPageUrlSize) {
            return NativeMethods.GetRegisterPageURL(registerPageUrl, ref registerPageUrlSize);
        }

        public int GetPasswordResetURL(StringBuilder passwordResetUrl, ref int passwordResetUrlSize) {
            return NativeMethods.GetPasswordResetURL(passwordResetUrl, ref passwordResetUrlSize);
        }

        public int GetChatSettingURL(StringBuilder chatSettingUrl, ref int chatSettingUrlSize) {
            return NativeMethods.GetChatSettingURL(chatSettingUrl, ref chatSettingUrlSize);
        }

        public int GetUserChannelsPageURL(StringBuilder userChannelsPageUrl, ref int userChannelsPageUrlSize) {
            return NativeMethods.GetUserChannelsPageURL(userChannelsPageUrl, ref userChannelsPageUrlSize);
        }

        public int GetAudienceDiscordInvitationURL(StringBuilder audienceDiscordInvitationUrl, ref int audienceDiscordInvitationUrlSize) {
            return NativeMethods.GetAudienceDiscordInvitationURL(audienceDiscordInvitationUrl, ref audienceDiscordInvitationUrlSize);
        }

        public int GetAudienceTwitterInvitationURL(StringBuilder audienceTwitterInvitationUrl, ref int audienceTwitterInvitationUrlSize) {
            return NativeMethods.GetAudienceTwitterInvitationURL(audienceTwitterInvitationUrl, ref audienceTwitterInvitationUrlSize);
        }

        public int GetUserAppRank(string appName, ref int appRank) {
            return NativeMethods.GetUserAppRank(appName, ref appRank);
        }

        public int SetUserAppRank(string appName, int appRank) {
            return NativeMethods.SetUserAppRank(appName, appRank);
        }

        public int EnableUserBehavior(bool enable) {
            return NativeMethods.EnableUserBehavior(enable);
        }

        public int SetUserBehaviorInfo(UserBehaviorInfoType type, string infoValue) {
            return NativeMethods.SetUserBehaviorInfo(type, infoValue);
        }

        public void ShowDefaultEmoji(EmojiType emojiType) {
            this.BundleManager.ShowDefaultEmoji(emojiType);
            return;
        }

        public GameObject GetEmoji(EmojiType emojiType) {
            return this.BundleManager.GetEmoji(emojiType);
        }

        private Context() {
            this._sendCameraFrame = false;
            this._sceneListState = SceneListState.Uncached;
            this._streamState = StreamState.Unload;
            this._sceneState = SceneState.None;
            this._sceneListState = SceneListState.Uncached;
            this._chatConnectionState = ChatConnectionState.Closed;
            this._platformConnectionState = new Dictionary<string, PlatformConnectionState>();
            this.CurrentUserData = new UserData();
        }
    }
}
