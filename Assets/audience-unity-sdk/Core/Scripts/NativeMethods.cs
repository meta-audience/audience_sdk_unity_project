using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace AudienceSDK {

    /// <summary>
    /// Type of Audience plugin return code.
    /// </summary>
    public enum AudienceReturnCode {

        /// <summary>
        /// Code Ok
        /// </summary>
        AudienceSDKOk = 0,

        /// <summary>
        /// Code Not Initialize
        /// </summary>
        AudienceSDKNotInited,

        /// <summary>
        /// Code Null Pointer
        /// </summary>
        AudienceSDKNullPtr,

        /// <summary>
        /// Code Invalid Parameters
        /// </summary>
        AudienceInvalidParams,

        /// <summary>
        /// Code LoadConfigFailure
        /// </summary>
        AudienceSDKLoadConfigFailure,

        /// <summary>
        /// Code StringConvertError
        /// </summary>
        AudienceStringConvertError,

        /// <summary>
        /// Code Ok
        /// </summary>
        AudienceBufferSizeNotEnough,

        /// <summary>
        /// Code InvalidState
        /// </summary>
        AudienceSDKInvalidState,

        /// <summary>
        /// Code NetworkError
        /// </summary>
        AudienceSDKNetworkError,

        /// <summary>
        /// Code NetworkDBDataError
        /// </summary>
        AudienceSDKNetworkDBDataError,

        /// <summary>
        /// Code AccountDataError
        /// </summary>
        AudienceSDKAccountDataError,

        /// <summary>
        /// Code InternalError
        /// </summary>
        AudienceSDKInternalError,

        /// <summary>
        /// Code MapKeyNotFound
        /// </summary>
        AudienceSDKMapKeyNotFound,

        /// <summary>
        /// Code DataNotFound
        /// </summary>
        AudienceSDKDataNotFound,

        /// <summary>
        /// Code Logger Initialize Failure
        /// </summary>
        AudienceSDKLoggerInitFailure,

        /// <summary>
        /// Code ParseFailure
        /// </summary>
        AudienceSDKParseFailure,

        /// <summary>
        /// Code AudioStreamError
        /// </summary>
        AudienceSDKAudioStreamError,

        /// <summary>
        /// Code VideoStreamError
        /// </summary>
        AudienceSDKVideoStreamError,

        /// <summary>
        /// Code StreamSessionError
        /// </summary>
        AudienceSDKStreamSessionError,

        /// <summary>
        /// Code PeerError
        /// </summary>
        AudienceSDKPeerError,

        /// <summary>
        /// Code DeviceError
        /// </summary>
        AudienceSDKDeviceError,

        /// <summary>
        /// Code DuplicatedRegisterError
        /// </summary>
        AudienceSDKDuplicatedRegisterError,

        /// <summary>
        /// Code NetworkTimeOut
        /// </summary>
        AudienceNetworkTimeOut,

        /// <summary>
        /// Code SceneHadBeenModified
        /// </summary>
        AudienceSDKSceneHadBeenModified = 300,

        /// <summary>
        /// Code Unauthorized
        /// </summary>
        AudienceSDKUnauthorized,

        /// <summary>
        /// Code Forbidden
        /// </summary>
        AudienceSDKForbidden,

        /// <summary>
        /// Code Forbidden
        /// </summary>
        AudienceSDKBadRequest,

        /// <summary>
        /// Code VideoSessionError
        /// </summary>
        AudienceSDKVideoSessionError = 400,

        /// <summary>
        /// Code VideoEncoderCreateError
        /// </summary>
        AudienceSDKVideoEncoderCreateError,

        /// <summary>
        /// Code VideoEncoderError
        /// </summary>
        AudienceSDKVideoEncoderError,

        /// <summary>
        /// Code VideoFrameError
        /// </summary>
        AudienceSDKVideoFrameError,

        /// <summary>
        /// Code VideoScheduleError
        /// </summary>
        AudienceSDKVideoScheduleError,
    }

    /// <summary>
    /// Type of Log on audience webserver state.
    /// </summary>
    public enum LogonState {

        /// <summary>
        /// state LoggedOut
        /// </summary>
        LoggedOut = 0,

        /// <summary>
        /// state LoggedIn
        /// </summary>
        LoggedIn = 1,

        /// <summary>
        /// state LoggingOut
        /// </summary>
        LoggingOut = 2,

        /// <summary>
        /// state LoggingIn
        /// </summary>
        LoggingIn = 3,
    }

    /// <summary>
    /// Type of channel state.
    /// </summary>
    public enum ChannelState {

        /// <summary>
        /// state Upcoming
        /// </summary>
        Upcoming = 0,

        /// <summary>
        /// state Active
        /// </summary>
        Active = 1,

        /// <summary>
        /// state Completed
        /// </summary>
        Completed = 2,
    }

    /// <summary>
    /// Type of cached scene list state.
    /// </summary>
    public enum SceneListState {

        /// <summary>
        /// state without cache any scene list
        /// </summary>
        Uncached = 0,

        /// <summary>
        /// state Refreshing
        /// </summary>
        Refreshing = 1,

        /// <summary>
        /// state Cached
        /// </summary>
        Cached = 2,
    }

    /// <summary>
    /// Type of using Video Encoder.
    /// </summary>
    public enum VideoEncoderType {

        /// <summary>
        /// type Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// type NVENC
        /// </summary>
        NVENC = 1,

        /// <summary>
        /// type QuickSync
        /// </summary>
        QuickSync = 2,

        /// <summary>
        /// type Software
        /// </summary>
        Software = 3,
    }

    /// <summary>
    /// Type of using Video Codec.
    /// </summary>
    public enum VideoCodec {

        /// <summary>
        /// type Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// type H264
        /// </summary>
        H264 = 1,

        /// <summary>
        /// type H265
        /// </summary>
        H265 = 2,
    }

    /// <summary>
    /// State of connection with audio device.
    /// </summary>
    public enum AudioState {

        /// <summary>
        /// state Inactive
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// state Connected
        /// </summary>
        Connected = 1,

        /// <summary>
        /// state Connecting
        /// </summary>
        Connecting = 2,

        /// <summary>
        /// state Disconnected
        /// </summary>
        Disconnected = 3,
    }

    /// <summary>
    /// State of audience streaming.
    /// </summary>
    public enum StreamState {

        /// <summary>
        /// state Unload
        /// </summary>
        Unload = 0,

        /// <summary>
        /// state Unloading
        /// </summary>
        Unloading = 1,

        /// <summary>
        /// state Loading
        /// </summary>
        Loading = 2,

        /// <summary>
        /// state Loaded
        /// </summary>
        Loaded = 3,

        /// <summary>
        /// state Starting
        /// </summary>
        Starting = 4,

        /// <summary>
        /// state Started
        /// </summary>
        Started = 5,

        /// <summary>
        /// state Stopping
        /// </summary>
        Stopping = 6,
    }

    /// <summary>
    /// State of connection with chat service.
    /// </summary>
    public enum ChatConnectionState {

        /// <summary>
        /// state Closed
        /// </summary>
        Closed = 0,

        /// <summary>
        /// state Opening
        /// </summary>
        Opening = 1,

        /// <summary>
        /// state Opened
        /// </summary>
        Opened = 2,

        /// <summary>
        /// state Closing
        /// </summary>
        Closing = 3,
    }

    public enum PlatformConnectionState {

        /// <summary>
        /// state Closed
        /// </summary>
        Closed = 0,

        /// <summary>
        /// state Opened
        /// </summary>
        Opened = 1,

        /// <summary>
        /// state Connecting
        /// </summary>
        Connecting = 2,

        /// <summary>
        /// state Closing
        /// </summary>
        Closing = 3,
    }

    /// <summary>
    /// State of connection with signaling service.
    /// </summary>
    public enum SignalingConnectionState {

        /// <summary>
        /// state Closed
        /// </summary>
        [Description("CONNECTION_STATE_CLOSED")]
        Closed = 0,

        /// <summary>
        /// state Opened
        /// </summary>
        [Description("CONNECTION_STATE_OPENED")]
        Opened = 1,

        /// <summary>
        /// state Failed
        /// </summary>
        [Description("CONNECTION_STATE_FAILED")]
        Failed = 2,
    }

    /// <summary>
    /// State of connection with stream session (one camera per session).
    /// </summary>
    public enum SessionState {

        /// <summary>
        /// State Inactive
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// State initialize
        /// </summary>
        Inited = 1,

        /// <summary>
        /// State Starting
        /// </summary>
        Starting = 2,

        /// <summary>
        /// State Ready
        /// </summary>
        Ready = 3,

        /// <summary>
        /// State UpdateNeeded
        /// </summary>
        UpdateNeeded = 4,

        /// <summary>
        /// State Updating
        /// </summary>
        Updating = 5,

        /// <summary>
        /// State Unstable
        /// </summary>
        Unstable = 6,

        /// <summary>
        /// State Stopping
        /// </summary>
        Stopping = 7,
    }

    /// <summary>
    /// State of current scene.
    /// </summary>
    public enum SceneState {

        /// <summary>
        /// State None
        /// </summary>
        None = 0,

        /// <summary>
        /// State Saved
        /// </summary>
        Saved = 1,

        /// <summary>
        /// State Saving
        /// </summary>
        Saving = 2,

        /// <summary>
        /// State Modified
        /// </summary>
        Modified = 3,
    }

    /// <summary>
    /// Type of camera properties.
    /// </summary>
    public enum CameraParamType {

        /// <summary>
        /// Property Position
        /// </summary>
        Position = 0,

        /// <summary>
        /// Property Rotation
        /// </summary>
        Rotation = 1,

        /// <summary>
        /// Property Resolution
        /// </summary>
        Resolution = 2,

        /// <summary>
        /// Property CaptureType
        /// </summary>
        CaptureType = 3,
    }

    /// <summary>
    /// Type of audio properties.
    /// </summary>
    public enum AudioParamType {

        /// <summary>
        /// Property ChannelType
        /// </summary>
        ChannelType = 0,

        /// <summary>
        /// Property SampleRate
        /// </summary>
        SampleRate = 1,

        /// <summary>
        /// Property IsMuted
        /// </summary>
        IsMuted = 2,

        /// <summary>
        /// Property Volume
        /// </summary>
        Volume = 3,
    }

    /// <summary>
    /// Type of capture streaming frames.
    /// </summary>
    public enum CaptureType {

        /// <summary>
        /// Unknown type
        /// </summary>
        [Description("Unknown")]
        _Unknown = 0,

        /// <summary>
        /// Stereo Flat
        /// </summary>
        [Description("3D (Flat)")]
        _3D_Flat = 1,

        /// <summary>
        /// Stereo 360
        /// </summary>
        [Description("3D (360°)")]
        _3D_360 = 2,

        /// <summary>
        /// Stereo 180
        /// </summary>
        [Description("3D (180°)")]
        _3D_180 = 3,

        /// <summary>
        /// Mono Flat
        /// </summary>
        [Description("2D (Flat)")]
        _2D_Flat = 4,

        /// <summary>
        /// Mono 360
        /// </summary>
        [Description("2D (360°)")]
        _2D_360 = 5,

        /// <summary>
        /// Mono 180
        /// </summary>
        [Description("2D (180°)")]
        _2D_180 = 6,

        /// <summary>
        /// Stereo w/o back face while rendering cubemap
        /// </summary>
        [Description("3D (Cutback)")]
        _3D_CULLBACK = 7,

        /// <summary>
        /// Stereo with only 3 faces of cubemap
        /// </summary>
        [Description("3D (Half)")]
        _3D_HALF = 8,

        /// <summary>
        /// Mono w/o back face while rendering cubemap
        /// </summary>
        [Description("2D (Cutback)")]
        _2D_CULLBACK = 9,

        /// <summary>
        /// Mono with only 3 faces of cubemap
        /// </summary>
        [Description("2D (Half)")]
        _2D_HALF = 10,
    }

    /// <summary>
    /// Type of audio channel.
    /// </summary>
    public enum ChannelType {

        /// <summary>
        /// Type Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Type Mono
        /// </summary>
        Mono = 1,

        /// <summary>
        /// Type Stereo
        /// </summary>
        Stereo = 2,
    }

    /// <summary>
    /// Type of peer list information changed.
    /// </summary>
    public enum PeerInfoListChangedType {

        /// <summary>
        /// Join streaming
        /// </summary>
        Joined = 0,

        /// <summary>
        /// Leave streaming
        /// </summary>
        Left = 1,
    }

    /// <summary>
    /// Type of received peer message.
    /// </summary>
    public enum PeerMessageType {

        /// <summary>
        /// Type Message
        /// </summary>
        Message = 0,

        /// <summary>
        /// Type Emoticon
        /// </summary>
        Emoji = 1,

        /// <summary>
        /// Type Camera
        /// </summary>
        Camera = 2,
    }

    /// <summary>
    /// Type of received emoticon message.
    /// </summary>
    public enum EmojiType {

        /// <summary>
        /// Type Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Type Heart
        /// </summary>
        Heart = 1,

        /// <summary>
        /// Type Balloon
        /// </summary>
        Balloon = 2,

        /// <summary>
        /// Type Star
        /// </summary>
        Star = 3,

        /// <summary>
        /// Type Candy
        /// </summary>
        Candy = 4,

        /// <summary>
        /// Type Smile
        /// </summary>
        Smile = 5,
    }

    /// <summary>
    /// Type of user behavior info type.
    /// </summary>
    public enum UserBehaviorInfoType {

        /// <summary>
        /// User's headset
        /// </summary>
        HeadsetDevice = 0,

        /// <summary>
        /// User's playing game.
        /// </summary>
        Source = 1,

        /// <summary>
        /// User login unique id.
        /// </summary>
        LoginUId = 2,
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "ChatPlatform should be defined in unity-sdk, center them to same class.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1649:FileNameMustMatchTypeName", Justification = "Reviewed.")]
    public static class ChatPlatform {

        public const string Youtube = "youtube";
        public const string Twitch = "twitch";

        private const string _unsupportPlatform = "Unsupport platform";

        private static Dictionary<string, string> _platformDescriptionTable = new Dictionary<string, string> {
            { Youtube, "YouTube" },
            { Twitch, "Twitch" },
        };

        public static string GetDescription(string platform) {
            var description = _unsupportPlatform;
            _platformDescriptionTable.TryGetValue(platform, out description);
            return description;
        }
    }

    /// <summary NativeUserData from plugin./>
    // UserData comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1649:FileNameMustMatchTypeName", Justification = "Reviewed.")]
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]

    public struct NativeUserData {

        /// <summary nick name./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Nickname;

        /// <summary login uid/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string LoginUId;
    }

    /// <summary NativeSceneSummaryData from plugin./>
    // SceneObject comes from Database, center them to same class
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1649:FileNameMustMatchTypeName", Justification = "Reviewed.")]
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]

    public struct NativeSceneSummaryData {

        /// <summary SceneId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string SceneId;

        /// <summary SceneName/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string SceneName;

        /// <summary RoomTitle/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string RoomTitle;

        /// <summary CameraCount/>
        public int CameraCount;

        /// <summary AudioCount/>
        public int AudioCount;

        /// <summary VideoEncoderType/>
        public VideoEncoderType VideoEncoderType;

        /// <summary VideoCodec/>
        public VideoCodec VideoCodec;
    }

    /// <summary NativeCatpureType from plugin./>
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativeCatpureType {

        /// <summary CaptureType./>
        public CaptureType CaptureType;
    }

    /// <summary NativeResolution from plugin./>
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativeResolution {

        /// <summary TextureWidth./>
        public int TextureWidth;

        /// <summary TextureHeight./>
        public int TextureHeight;
    }

    /// <summary NativePosition from plugin./>
    // Content will use it to set to json format, won't fix naimng.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Follow source json data naming")]
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativePosition {

        /// <summary position_x./>
        public float position_x;

        /// <summary position_y./>
        public float position_y;

        /// <summary position_z./>
        public float position_z;
    }

    /// <summary NativeRotation from plugin./>
    // Content will use it to set to json format, won't fix naimng.
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Follow source json data naming")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Follow source json data naming")]
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativeRotation {

        /// <summary rotation_x./>
        public float rotation_x;

        /// <summary rotation_y./>
        public float rotation_y;

        /// <summary rotation_z./>
        public float rotation_z;
    }

    /// <summary NativePeerInfo from plugin./>
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativePeerInfo {

        /// <summary PeerId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PeerId;

        /// <summary PeerName./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PeerName;

        /// <summary VideoId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string VideoId;
    }

    /// <summary NativePeerEmojiData from plugin./>
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativePeerEmojiData {

        /// <summary PeerId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PeerId;

        /// <summary VideoId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string VideoId;

        /// <summary EmojiType./>
        public EmojiType EmojiType;
    }

    /// <summary NativePeerMessageData from plugin./>
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativePeerMessageData {

        /// <summary PeerId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PeerId;

        /// <summary Msg./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string Msg;
    }

    /// <summary NativePeerCameraData from plugin./>
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
    public struct NativePeerCameraData {

        /// <summary PeerId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PeerId;

        /// <summary VideoId./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string VideoId;

        /// <summary Position./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Position;

        /// <summary Rotation./>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Rotation;
    }

    /// <summary>
    /// Delegate callback when error occurss.
    /// </summary>
    /// <param name="errorCode">error code.</param>
    /// <param name="errorMessage">error message.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateErrorOccured(int errorCode, [MarshalAs(UnmanagedType.LPStr)] string errorMessage);

    /// <summary>
    /// Delegate callback when logon state changes.
    /// </summary>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateLogonStateChanged(LogonState state);

    /// <summary>
    /// Delegate callback when login complete.
    /// </summary>
    /// <param name="userData">login info.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateLoginCompleted(NativeUserData userData);

    /// <summary>
    /// Delegate callback when logout complete.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateLogoutCompleted();

    /// <summary>
    /// Delegate callback when scene list state changes.
    /// </summary>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateSceneListStateChanged(SceneListState state);

    /// <summary>
    /// Delegate callback when Refresh Scene List Completed.
    /// </summary>
    /// <param name="sceneSummaryDataList">raw data of scene.</param>
    /// <param name="size">how many scenes.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateRefreshSceneListCompleted(IntPtr sceneSummaryDataList, int size);

    /// <summary>
    /// Delegate callback when stream state changes.
    /// </summary>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateStreamStateChanged(StreamState state);

    /// <summary>
    /// Delegate callback when signaling connection state changes.
    /// </summary>
    /// <param name="streamSessionId">which stream session.</param>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateSignalingConnectionStateChanged([MarshalAs(UnmanagedType.LPStr)] string streamSessionId, SignalingConnectionState state);

    /// <summary>
    /// Delegate callback when Audio state changes.
    /// </summary>
    /// <param name="device_id">which audio device.</param>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateAudioStateChanged([MarshalAs(UnmanagedType.LPStr)] string device_id, AudioState state);

    /// <summary>
    /// Delegate callback when Audio Device Changed.
    /// </summary>
    /// <param name="old_device_id">old device id..</param>
    /// <param name="new_device_id">new device id.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateDefaultAudioDeviceChanged([MarshalAs(UnmanagedType.LPStr)] string old_device_id, [MarshalAs(UnmanagedType.LPStr)] string new_device_id);

    /// <summary>
    /// Delegate callback when Load Scene Completed.
    /// </summary>
    /// <param name="output">loaded scene content.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateLoadSceneCompleted(string output);

    /// <summary>
    /// Delegate callback when Unload Scene Completed.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateUnloadSceneCompleted();

    /// <summary>
    /// Delegate callback when Save Scene Completed.
    /// </summary>
    /// <param name="output">saved scene content.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateSaveSceneCompleted(string output);

    /// <summary>
    /// Delegate callback when Session state changes.
    /// </summary>
    /// <param name="streamSessionId">which stream session.</param>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateSessionStateChanged([MarshalAs(UnmanagedType.LPStr)] string streamSessionId, SessionState state);

    /// <summary>
    /// Delegate callback when peer info list changes.
    /// </summary>
    /// <param name="streamSessionId">which stream session.</param>
    /// <param name="peerInfoListChangedType">change type.</param>
    /// <param name="peerInfo">peer info.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegatePeerInfoListChanged([MarshalAs(UnmanagedType.LPStr)] string streamSessionId, PeerInfoListChangedType peerInfoListChangedType, IntPtr peerInfo);

    /// <summary>
    /// Delegate callback when peer message received.
    /// </summary>
    /// <param name="streamSessionId">which stream session.</param>
    /// <param name="peerMessageType">received message type.</param>
    /// <param name="peerMessage">peer message.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegatePeerMessageReceived([MarshalAs(UnmanagedType.LPStr)] string streamSessionId, PeerMessageType peerMessageType, IntPtr peerMessage);

    /// <summary>
    /// Delegate callback when Scene State Changes.
    /// </summary>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateSceneStateChanged(SceneState state);

    /// <summary>
    /// Delegate callback when Scene Changes.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateSceneChanged();

    /// <summary>
    /// Delegate callback when Chat Connection State Changes.
    /// </summary>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateChatConnectionStateChanged(ChatConnectionState state);

    /// <summary>
    /// Delegate callback when Chat Message Received.
    /// </summary>
    /// <param name="chatMessage">received message.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateChatMessageReceived([MarshalAs(UnmanagedType.LPStr)] string chatMessage);

    /// <summary>
    /// Delegate callback when Platform Connection State Changes.
    /// </summary>
    /// <param name="platform">the platform whose state is changed.</param>
    /// <param name="state">change to which state.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegatePlatformConnectionStateChanged([MarshalAs(UnmanagedType.LPStr)] string platform, PlatformConnectionState state);

    /// <summary>
    /// Delegate callback when Chat emoji mute Changes.
    /// </summary>
    /// <param name="enable">mute emoji is changed.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateChatEmojiMuteChanged(bool enable);

    /// <summary>
    /// Native apis of plugin.
    /// </summary>
    /// <param name="chatMessage">received message.</param>
    public class NativeMethods {
        private const string dllName = "audience-plugin";

        /// <summary>
        /// Init plugin.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Init();

        /// <summary>
        /// Init plugin.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "DeInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DeInit();

        /// <summary>
        /// Register ErrorOccured callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterErrorOccured", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterErrorOccured(DelegateErrorOccured callback);

        /// <summary>
        /// Register LogonStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterLogonStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterLogonStateChanged(DelegateLogonStateChanged callback);

        /// <summary>
        /// Register LoginCompleted callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterLoginCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterLoginCompleted(DelegateLoginCompleted callback);

        /// <summary>
        /// Register LogoutCompleted callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterLogoutCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterLogoutCompleted(DelegateLogoutCompleted callback);

        /// <summary>
        /// Register ProfileListStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterProfileListStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterProfileListStateChanged(DelegateSceneListStateChanged callback);

        /// <summary>
        /// Register RefreshProfileListCompleted callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterRefreshProfileListCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterRefreshProfileListCompleted(DelegateRefreshSceneListCompleted callback);

        /// <summary>
        /// API refresh Profile List.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RefreshProfileList", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RefreshProfileList();

        /// <summary>
        /// Register StreamStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterStreamStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterStreamStateChanged(DelegateStreamStateChanged callback);

        /// <summary>
        /// Register SignalingConnectionStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterSignalingConnectionStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterSignalingConnectionStateChanged(DelegateSignalingConnectionStateChanged callback);

        /// <summary>
        /// Register AudioStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        // register is ok but event trigger need to wait for modules implement
        [DllImport(dllName, EntryPoint = "RegisterAudioStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterAudioStateChanged(DelegateAudioStateChanged callback);

        /// <summary>
        /// Register DefaultAudioDeviceChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterDefaultAudioDeviceChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterDefaultAudioDeviceChanged(DelegateDefaultAudioDeviceChanged callback);

        /// <summary>
        /// Register LoadProfileCompleted callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterLoadProfileCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterLoadProfileCompleted(DelegateLoadSceneCompleted callback);

        /// <summary>
        /// Register UnloadProfileCompleted callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterUnloadProfileCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterUnloadProfileCompleted(DelegateUnloadSceneCompleted callback);

        /// <summary>
        /// Register SessionStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterSessionStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterSessionStateChanged(DelegateSessionStateChanged callback);

        /// <summary>
        /// Register PeerInfoListChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterPeerInfoListChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterPeerInfoListChanged(DelegatePeerInfoListChanged callback);

        /// <summary>
        /// Register PeerMessageReceived callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterPeerMessageReceived", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterPeerMessageReceived(DelegatePeerMessageReceived callback);

        /// <summary>
        /// Register SaveProfileCompleted callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterSaveProfileCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterSaveProfileCompleted(DelegateSaveSceneCompleted callback);

        /// <summary>
        /// Register ChatConnectionStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterChatConnectionStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterChatConnectionStateChanged(DelegateChatConnectionStateChanged callback);

        /// <summary>
        /// Register ChatMessageReceived callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterChatMessageReceived", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterChatMessageReceived(DelegateChatMessageReceived callback);

        /// <summary>
        /// Register PlatformConnectionStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterPlatformConnectionChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterPlatformConnectionChanged(DelegatePlatformConnectionStateChanged callback);

        /// <summary>
        /// Register ChatMuteEmojiChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterChatMuteEmojiChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterChatMuteEmojiChanged(DelegateChatEmojiMuteChanged callback);

        /// <summary>
        /// Get Plugin Version from plugin.
        /// </summary>
        /// <param name="audience_plugin_version_number">returned version number.</param>
        /// <param name="buffer_size">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetPluginVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPluginVersion(StringBuilder audience_plugin_version_number, ref int buffer_size);

        /// <summary>
        /// Get Module Version from plugin.
        /// </summary>
        /// <param name="audience_module_version_number">returned version number.</param>
        /// <param name="buffer_size">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetModuleVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetModuleVersion(StringBuilder audience_module_version_number, ref int buffer_size);

        /// <summary>
        /// API Login.
        /// </summary>
        /// <param name="username">login username.</param>
        /// <param name="password">login password.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "Login", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Login(string username, string password);

        /// <summary>
        /// API Logout.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "Logout", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Logout();

        /// <summary>
        /// API LoadProfile.
        /// </summary>
        /// <param name="scene_id">scene id.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "LoadProfile", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LoadProfile(string scene_id);

        /// <summary>
        /// API UnloadProfile.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "UnloadProfile", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UnloadProfile();

        /// <summary>
        /// API GetChatChannel.
        /// </summary>
        /// <param name="platform">The platform name, e.g. "youtube".</param>
        /// <param name="chat_channel_json_array_size">what size create for chat_channel_json_array.</param>
        /// <param name="chat_channel_json_array">The channel list in json format.</param>
        /// <param name="chat_channel_data_size">return size from plugin.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetChatChannel", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetChatChannel(string platform, int chat_channel_json_array_size, StringBuilder chat_channel_json_array, ref int chat_channel_data_size);

        /// <summary>
        /// API Start the main audience system.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "Start", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Start();

        /// <summary>
        /// API Stop the main audience system.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "Stop", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Stop();

        /// <summary>
        /// [Internal usage] API Start Streaming.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "_StartStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _StartStream();

        /// <summary>
        /// [Internal usage] API Stop Streaming.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "_StopStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _StopStream();

        /// <summary>
        /// [Internal usage] API Start Connect to Chat.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "_StartChat", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _StartChat();

        /// <summary>
        /// [Internal usage] API Stop Chat connection.
        /// </summary>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "_StopChat", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _StopChat();

        /// <summary>
        /// API connect to specified chat room (e.g. youtube, twitch).
        /// </summary>
        /// <param name="platform">The platform name, e.g. "youtube".</param>
        /// <param name="channel">The channel name.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "ConnectChatroom", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ConnectChatroom(string platform, string channel);

        /// <summary>
        /// API disconnect to specified chat room (e.g. youtube, twitch).
        /// </summary>
        /// <param name="platform">The platform name, e.g. "youtube".</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "DisconnectChatroom", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DisconnectChatroom(string platform);

        /// <summary>
        /// API Send Message to chat room.
        /// </summary>
        /// <param name="input_chat_message">sent message.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SendChatMessage", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendChatMessage(string input_chat_message);

        /// <summary>
        /// API MuteEmoji.
        /// </summary>
        /// <param name="enable">mute or unmute emoji.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "MuteEmoji", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MuteEmoji(bool enable);

        /// <summary>
        /// API Get Profile.
        /// </summary>
        /// <returns>returned output profile.</returns>
        [DllImport(dllName, EntryPoint = "GetProfile", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetProfile();

        /// <summary>
        /// API Get Profile Data.
        /// </summary>
        /// <param name="sceneList">raw scene data.</param>
        /// <param name="sceneListSize">raw scene data size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetProfileList", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetProfileList(StringBuilder sceneList, ref int sceneListSize);

        /// <summary>
        /// API Save Profile.
        /// </summary>
        /// <param name="forceUpdate">override anyway.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SaveProfile", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SaveProfile(bool forceUpdate);

        /// <summary>
        /// API Set Camera Parameters.
        /// </summary>
        /// <param name="cameraMappingId">which camera.</param>
        /// <param name="paramType">parameter type.</param>
        /// <param name="value">raw json value of parameter.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SetCameraParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetCameraParam(int cameraMappingId, CameraParamType paramType, string value);

        /// <summary>
        /// API Set Audio Parameters.
        /// </summary>
        /// <param name="mappingId">which audio.</param>
        /// <param name="paramType">parameter type.</param>
        /// <param name="paramValue">raw json value of parameter.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SetAudioParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAudioParam(int mappingId, AudioParamType paramType, string paramValue);

        /// <summary>
        /// API Set Audio Device Parameters.
        /// </summary>
        /// <param name="mappingId">which audio.</param>
        /// <param name="deviceId">which device.</param>
        /// <param name="paramType">parameter type.</param>
        /// <param name="paramValue">raw json value of parameter.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SetAudioDeviceParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAudioDeviceParam(int mappingId, string deviceId, AudioParamType paramType, string paramValue);

        /// <summary>
        /// Register ProfileStateChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterProfileStateChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterProfileStateChanged(DelegateSceneStateChanged callback);

        /// <summary>
        /// Register ProfileChanged callback to plugin.
        /// </summary>
        /// <param name="callback">intput callback.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "RegisterProfileChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterProfileChanged(DelegateSceneChanged callback);

        /// <summary>
        /// API Send Camera Texture to streaming.
        /// </summary>
        /// <param name="cameraMappingId">which camera.</param>
        /// <param name="texture">texture content.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SendCameraTexture", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendCameraTexture(int cameraMappingId, IntPtr texture);

        /// <summary>
        /// API Get register audience account page url.
        /// </summary>
        /// <param name="registerPageUrl">url.</param>
        /// <param name="registerPageUrlSize">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetRegisterPageURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRegisterPageURL(StringBuilder registerPageUrl, ref int registerPageUrlSize);

        /// <summary>
        /// API Get Password Reset page url.
        /// </summary>
        /// <param name="passwordResetUrl">url.</param>
        /// <param name="passwordResetUrlSize">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetPasswordResetURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPasswordResetURL(StringBuilder passwordResetUrl, ref int passwordResetUrlSize);

        /// <summary>
        /// API Get Chat Setting page url.
        /// </summary>
        /// <param name="chatSettingUrl">url.</param>
        /// <param name="chatSettingUrlSize">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetChatSettingURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetChatSettingURL(StringBuilder chatSettingUrl, ref int chatSettingUrlSize);

        /// <summary>
        /// API Get User Channels Page url.
        /// </summary>
        /// <param name="userChannelsPageUrl">url.</param>
        /// <param name="userChannelsPageUrlSize">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetUserChannelsPageURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetUserChannelsPageURL(StringBuilder userChannelsPageUrl, ref int userChannelsPageUrlSize);

        /// <summary>
        /// API Get Audience Discord invitation url.
        /// </summary>
        /// <param name="audienceDiscordInvitationUrl">url.</param>
        /// <param name="audienceDiscordInvitationUrlSize">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetAudienceDiscordInvitationURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAudienceDiscordInvitationURL(StringBuilder audienceDiscordInvitationUrl, ref int audienceDiscordInvitationUrlSize);

        /// <summary>
        /// API Get Audience Twitter invitation url.
        /// </summary>
        /// <param name="audienceTwitterInvitationUrl">url.</param>
        /// <param name="audienceTwitterInvitationUrlSize">total string size.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetAudienceTwitterInvitationURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAudienceTwitterInvitationURL(StringBuilder audienceTwitterInvitationUrl, ref int audienceTwitterInvitationUrlSize);

        /// <summary>
        /// API Get user app rank.
        /// </summary>
        /// <param name="appName">target app.</param>
        /// <param name="appRank">user app rank.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "GetUserAppRank", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetUserAppRank(string appName, ref int appRank);

        /// <summary>
        /// API Set user app rank.
        /// </summary>
        /// <param name="appName">target app.</param>
        /// <param name="appRank">user rate point.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SetUserAppRank", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetUserAppRank(string appName, int appRank);

        /// <summary>
        /// API enable user behavior, default is false.
        /// </summary>
        /// <param name="enable">enable / disable user behavior.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "EnableUserBehavior", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableUserBehavior(bool enable);

        /// <summary>
        /// API Set user behavior info.
        /// </summary>
        /// <param name="type">user behavior info type.</param>
        /// <param name="infoValue">user behavior info value.</param>
        /// <returns>error code.</returns>
        [DllImport(dllName, EntryPoint = "SetUserBehaviorInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetUserBehaviorInfo(UserBehaviorInfoType type, string infoValue);
    }
}