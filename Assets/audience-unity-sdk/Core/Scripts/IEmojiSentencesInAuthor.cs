using System;
using System.Collections.Generic;

namespace AudienceSDK {
    public interface IEmojiSentencesInAuthor {
        AudienceReturnCode AddEmojiSentence(Queue<EmojiMessage> sentence);

        AudienceReturnCode AcquireNextEmoji(float realtimeSinceStartup, float timeInterval, out EmojiMessage emojiMsg, out int leftSize);

        float GetLastAcquireTime();

        int GetSentenceCount();

        ChatAuthor GetAuthor();
    }
}
