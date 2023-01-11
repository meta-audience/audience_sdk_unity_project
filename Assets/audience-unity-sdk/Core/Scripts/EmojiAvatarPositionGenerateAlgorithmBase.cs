using UnityEngine;

namespace AudienceSDK
{
    public interface EmojiAvatarPositionGenerateAlgorithmBase
    {
        AudienceReturnCode GenerateAvatarPosition(ref Vector3 relativeAvatarPositon);
    }
}
