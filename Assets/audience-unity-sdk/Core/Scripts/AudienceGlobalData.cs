using UnityEngine;

namespace AudienceSDK {

    public static class AudienceGlobalData {

        public static double[] VolumeValueArray => _volumeValueArray;

        private static double[] _volumeValueArray = new double[6] {
            0f,
            0.25f,
            0.5f,
            1f,
            2f,
            4f,
        };
    }
}
