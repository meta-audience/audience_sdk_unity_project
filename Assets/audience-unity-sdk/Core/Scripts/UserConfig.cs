namespace AudienceSDK {
    // UserConfig should be renamed!!, it's not for user, it's for SDK class config.
    public static class UserConfig {
        public static bool IsUnityEnable360StereoCapture { get; set; } = false;

        public static int CubemapSize { get; set; } = -1;

        public static string DefaultCamAvatarShader { get; set; } = string.Empty;

        public static string DefaultEmojiShader { get; set; } = string.Empty;

        public static string DefaultPreviewQuadShader { get; set; } = string.Empty;

        public static int EmojiLimitPerMessage { get; set; } = 20;

        public static int EmojiMaxAuthors { get; set; } = 500;

        public static int EmojiMaxSentencesInOneAuthor { get; set; } = 2;

        public static int EmojiMaxSceneEmojis { get; set; } = 1000;
    }
}
