using System;
using System.Collections.Generic;

namespace AudienceSDK {
    public interface IEmojiSentencesInAuthorFactory {
        IEmojiSentencesInAuthor CreateEmojiSentencesInAuthor(ChatAuthor author);
    }

    public class EmojiSentencesInAuthorFactory : IEmojiSentencesInAuthorFactory {
        public IEmojiSentencesInAuthor CreateEmojiSentencesInAuthor(ChatAuthor author) {
            return new EmojiSentencesInAuthor(author);
        }
    }
}
