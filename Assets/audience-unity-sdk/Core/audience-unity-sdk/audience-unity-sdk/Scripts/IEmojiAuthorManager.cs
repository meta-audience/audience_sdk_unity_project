using System;
using System.Collections.Generic;

namespace AudienceSDK {
    using EmojiSentence = System.Collections.Generic.Queue<AudienceSDK.EmojiMessage>;

    public interface IEmojiAuthorManager {
        AudienceReturnCode AddEmojiSentence(ChatAuthor author, EmojiSentence sentence);

        Dictionary<ChatAuthor, EmojiMessage> ExtractCandidateEmojis();

        int GetCurrentAuthorCount();
    }
}
