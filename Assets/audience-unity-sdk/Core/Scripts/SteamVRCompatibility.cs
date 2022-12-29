using System;

namespace AudienceSDK {
    public static class SteamVRCompatibility {
        public static Type SteamVRCamera { get; set; }

        public static Type SteamVRExternalCamera { get; set; }

        public static Type SteamVRFade { get; set; }

        public static bool IsAvailable => FindSteamVRAsset();

        private static bool FindSteamVRAsset() {
            SteamVRCamera = Type.GetType("SteamVR_Camera, Assembly-CSharp-firstpass", false);
            SteamVRExternalCamera = Type.GetType("SteamVR_ExternalCamera, Assembly-CSharp-firstpass", false);
            SteamVRFade = Type.GetType("SteamVR_Fade, Assembly-CSharp-firstpass", false);
            return SteamVRCamera != null && SteamVRExternalCamera != null && SteamVRFade != null;
        }
    }
}