using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AudienceSDK {

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "this class is a base class that contains several protected variables to inherit")]
    public abstract class PanoramicCameraCapture : BaseCameraCapture {
        protected Vector2Int _targetEquirectSize;
        protected int _faceMask = 63;
        private const int _maximumCubemapSize = 16384;

        protected int GetCubemapSize(int equirectWidth, int equirectHeight) {
            if (UserConfig.CubemapSize != -1) {

                if (UserConfig.CubemapSize != 0) {

                    return UserConfig.CubemapSize;
                }
            }

            var result = -1;
            var longSide = Math.Max(equirectWidth, equirectHeight);
            if (longSide < 2) {

                Debug.LogError("input equirect size is illegal, width = " + equirectWidth + ", height = " + equirectHeight + ".");
                result = -1;
                Debug.Log("return cubemap size is " + result.ToString());
                return result;
            }

            result = UserConfig.CubemapSize;

            var bitMoveSteps = 0;
            var tempLongSide = longSide - 1;
            while (tempLongSide > 0) {

                tempLongSide = tempLongSide >> 1;
                if (tempLongSide > 0) {

                    bitMoveSteps++;
                }
            }

            var calCubemapSize = 1 << bitMoveSteps;
            if (calCubemapSize > _maximumCubemapSize) {

                Debug.LogWarning("input equirect size is bigger than maximum CubemapSize, width = " + equirectWidth + ", height = " + equirectHeight + ".");
                calCubemapSize = _maximumCubemapSize;
            }

            result = calCubemapSize;
            Debug.Log("return cubemap size is " + result.ToString());
            return result;
        }
    }
}
