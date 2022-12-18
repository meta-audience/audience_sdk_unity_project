using System.Collections.Generic;
using AudienceSDK;
using NUnit.Framework;

namespace AudienceSDK.UnitTest {
    public class TestEmojiSentencesInAuthor {
        private EmojiSentencesInAuthor _emojiSentencesInAuthor;

        [SetUp]
        public void Setup() {
            this._emojiSentencesInAuthor = new EmojiSentencesInAuthor(new ChatAuthor());
        }

        [TearDown]
        public void TearDown() {
            this._emojiSentencesInAuthor = null;
        }

        [Test]
        public void AddEmojiSentenceButOutOfLimitReturnBufferSizeNotEnough() {
            UserConfig.EmojiMaxSentencesInOneAuthor = 1;

            var rc1 = this._emojiSentencesInAuthor.AddEmojiSentence(new Queue<EmojiMessage>());
            var rc2 = this._emojiSentencesInAuthor.AddEmojiSentence(new Queue<EmojiMessage>());

            Assert.AreEqual(rc1, AudienceReturnCode.AudienceSDKOk);
            Assert.AreEqual(rc2, AudienceReturnCode.AudienceBufferSizeNotEnough);
        }

        [Test]
        public void AddEmojiSentenceReturnOk() {
            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());
            var rc = this._emojiSentencesInAuthor.AddEmojiSentence(sentence);

            Assert.AreEqual(rc, AudienceReturnCode.AudienceSDKOk);
        }

        [Test]
        public void AcquireNextEmojiButAuthorIsCoolingDownReturnInvalidState() {

            // Arrange
            var realtimeSinceStartUp = 0.1f;
            var timeInterval = 10f;

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());
            var rc = this._emojiSentencesInAuthor.AddEmojiSentence(sentence);

            // realtimeSinceStartUp - lastAcquireTime(new created author is zero) is smaller than timeinterval,
            // author is cooling down.
            // Act
            EmojiMessage emojiMsg = null;
            var leftSize = 0;
            rc = this._emojiSentencesInAuthor.AcquireNextEmoji(realtimeSinceStartUp, timeInterval, out emojiMsg, out leftSize);

            // Assert
            Assert.IsNull(emojiMsg);
            Assert.AreEqual(1, leftSize);
            Assert.AreEqual(AudienceReturnCode.AudienceSDKInvalidState, rc);
        }

        [Test]
        public void AcquireNextEmojiReturnMessageAndDequeueEmptyQueue() {

            // Arrange
            var realtimeSinceStartUp = 0.6f;
            var timeInterval = 0.5f;

            var sentence1 = new Queue<EmojiMessage>();
            var sentence2 = new Queue<EmojiMessage>();
            sentence1.Enqueue(new Emoji2DMessage());
            sentence2.Enqueue(new Emoji2DMessage());

            var rc = this._emojiSentencesInAuthor.AddEmojiSentence(sentence1);
            rc = this._emojiSentencesInAuthor.AddEmojiSentence(sentence2);

            // 2 sentence, their count are 1. after acquire emoji, dequeue 1 sentence.
            // expect sentence count = 1
            // Act
            EmojiMessage emojiMsg = null;
            var leftSize = 0;
            rc = this._emojiSentencesInAuthor.AcquireNextEmoji(realtimeSinceStartUp, timeInterval, out emojiMsg, out leftSize);

            // Assert
            Assert.IsNotNull(emojiMsg);
            Assert.AreEqual(1, leftSize);
            Assert.AreNotEqual(0f, this._emojiSentencesInAuthor.GetLastAcquireTime());
            Assert.AreEqual(1, this._emojiSentencesInAuthor.GetSentenceCount());
        }

        [Test]
        public void AcquireNextEmojiReturnNullAfterAcquireAllEmojis() {

            // Arrange
            var realtimeSinceStartUp1 = 0.6f;
            var realtimeSinceStartUp2 = 1.2f;
            var timeInterval = 0.5f;

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());
            var rc = this._emojiSentencesInAuthor.AddEmojiSentence(sentence);

            // Act
            EmojiMessage emojiMsg1 = null, emojiMsg2 = null;
            int leftSize1 = 0, leftSize2 = 0;
            rc = this._emojiSentencesInAuthor.AcquireNextEmoji(realtimeSinceStartUp1, timeInterval, out emojiMsg1, out leftSize1);
            rc = this._emojiSentencesInAuthor.AcquireNextEmoji(realtimeSinceStartUp2, timeInterval, out emojiMsg2, out leftSize2);

            // Assert
            Assert.IsNotNull(emojiMsg1);
            Assert.AreEqual(0, leftSize1);
            Assert.IsNull(emojiMsg2);
            Assert.AreEqual(0, leftSize2);
            Assert.AreNotEqual(0f, this._emojiSentencesInAuthor.GetLastAcquireTime());
            Assert.AreEqual(0, this._emojiSentencesInAuthor.GetSentenceCount());
        }
    }
}
