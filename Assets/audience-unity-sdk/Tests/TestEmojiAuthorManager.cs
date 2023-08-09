using System.Collections.Generic;
using System.Threading.Tasks;
using AudienceSDK;
using NUnit.Framework;
using Moq;

namespace AudienceSDK.UnitTest {
    public class TestEmojiAuthorManager {
        private EmojiAuthorManager _emojiAuthorManager;
        private Mock<IEmojiSentencesInAuthorFactory> _mockEmojiSentencesInAuthorFactory;

        [SetUp]
        public void Setup() {
            this._mockEmojiSentencesInAuthorFactory = new Mock<IEmojiSentencesInAuthorFactory>();
            this._emojiAuthorManager = new EmojiAuthorManager(this._mockEmojiSentencesInAuthorFactory.Object);
        }

        [TearDown]
        public void TearDown() {
            this._emojiAuthorManager = null;
            this._mockEmojiSentencesInAuthorFactory = null;
        }

        [Test]
        public void AddEmojiSentenceButNullAuthorReturnInvalidParams() {

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());

            var rc = this._emojiAuthorManager.AddEmojiSentence(null, sentence);

            Assert.AreEqual(rc, AudienceReturnCode.AudienceInvalidParams);
        }

        [Test]
        public void AddEmojiSentenceIncompleteAuthorReturnInvalidParams() {

            var author = new ChatAuthor();

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());

            var rc = this._emojiAuthorManager.AddEmojiSentence(author, sentence);

            Assert.AreEqual(rc, AudienceReturnCode.AudienceInvalidParams);
        }

        [Test]
        public void AddEmojiSentenceButEmojiQueueInSentenceIsEmptyReturnInvalidParams() {

            var author = new ChatAuthor();
            author.user_id = "-1";
            author.platform = "twitch";

            var sentence = new Queue<EmojiMessage>();

            var rc = this._emojiAuthorManager.AddEmojiSentence(author, sentence);

            Assert.AreEqual(rc, AudienceReturnCode.AudienceInvalidParams);
        }

        [Test]
        public void AddEmojiSentenceButStateIsMutedReturnInvalidState() {

            var author = new ChatAuthor();
            author.user_id = "-1";
            author.platform = "twitch";

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());

            this._emojiAuthorManager.OnMuteEmojiChanged(true);
            var rc = this._emojiAuthorManager.AddEmojiSentence(author, sentence);

            Assert.AreEqual(rc, AudienceReturnCode.AudienceSDKInvalidState);
        }

        [Test]
        public void AddEmojiSentenceAuthorOutOfLimitReturnBufferSizeNotEnough() {

            // Arrange
            var fakeReturnAuthor = new ChatAuthor();
            fakeReturnAuthor.user_id = "-1";
            fakeReturnAuthor.platform = "twitch";

            var mockEmojiSentencesInAuthor = new Mock<IEmojiSentencesInAuthor>();

            this._mockEmojiSentencesInAuthorFactory
                .Setup(x => x.CreateEmojiSentencesInAuthor(It.IsAny<ChatAuthor>()))
                .Returns(mockEmojiSentencesInAuthor.Object);
            mockEmojiSentencesInAuthor.Setup(x => x.GetAuthor())
                .Returns(fakeReturnAuthor);

            for (int i = 0; i < 500; i++) {
                var author = new ChatAuthor();
                author.user_id = i.ToString();
                author.platform = "twitch";

                var sentence = new Queue<EmojiMessage>();
                sentence.Enqueue(new Emoji2DMessage());
                this._emojiAuthorManager.AddEmojiSentence(author, sentence);
            }

            var author501 = new ChatAuthor();
            author501.user_id = "501";
            author501.platform = "twitch";

            var sentence501 = new Queue<EmojiMessage>();
            sentence501.Enqueue(new Emoji2DMessage());

            // Act
            var rc = this._emojiAuthorManager.AddEmojiSentence(author501, sentence501);

            // Assert
            Assert.AreEqual(rc, AudienceReturnCode.AudienceBufferSizeNotEnough);
        }

        [Test]
        public void AddEmojiSentenceReturnOK() {

            var author = new ChatAuthor();
            author.user_id = "5566";
            author.platform = "twitch";

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(new Emoji2DMessage());

            var mockEmojiSentencesInAuthor = new Mock<IEmojiSentencesInAuthor>();

            this._mockEmojiSentencesInAuthorFactory
                .Setup(x => x.CreateEmojiSentencesInAuthor(It.IsAny<ChatAuthor>()))
                .Returns(mockEmojiSentencesInAuthor.Object);

            var rc = this._emojiAuthorManager.AddEmojiSentence(author, sentence);

            Assert.AreEqual(rc, AudienceReturnCode.AudienceSDKOk);
        }

        [Test]
        public void ExtractCandidateEmojisWhenAuthorListIsEmptyReturnEmptyDictionary() {
            Dictionary<ChatAuthor, EmojiMessage> playList;

            playList = this._emojiAuthorManager.ExtractCandidateEmojis();

            Assert.AreEqual(0, playList.Count);
        }

        [Test]
        public void ExtractCandidateEmojisReturnOKAndRemoveAuthorWhenNoEmojiLeft() {

            // Arrange
            Dictionary<ChatAuthor, EmojiMessage> playList;
            var author = new ChatAuthor();
            author.user_id = "5566";
            author.platform = "twitch";
            var mockEmojiSentencesInAuthor = new Mock<IEmojiSentencesInAuthor>();

            var emojiMsg = (new Emoji2DMessage()) as EmojiMessage;
            var leftSize = 0;

            this._mockEmojiSentencesInAuthorFactory
                .Setup(x => x.CreateEmojiSentencesInAuthor(author))
                .Returns(mockEmojiSentencesInAuthor.Object);

            mockEmojiSentencesInAuthor.Setup(x => x.AddEmojiSentence(It.IsAny<Queue<EmojiMessage>>()))
                .Returns(AudienceReturnCode.AudienceSDKOk);

            mockEmojiSentencesInAuthor.Setup(x => x.AcquireNextEmoji(It.IsAny<float>(), It.IsAny<float>(), out emojiMsg, out leftSize))
                .Returns(AudienceReturnCode.AudienceSDKOk);

            mockEmojiSentencesInAuthor.Setup(x => x.GetAuthor())
                .Returns(author);

            var sentence = new Queue<EmojiMessage>();
            sentence.Enqueue(emojiMsg);
            var rc = this._emojiAuthorManager.AddEmojiSentence(author, sentence);

            // Act
            playList = this._emojiAuthorManager.ExtractCandidateEmojis();

            // Assert
            Assert.AreEqual(rc, AudienceReturnCode.AudienceSDKOk);

            // AcquireNextEmoji and leftSize is zero, authorManager will remove this author from list.
            Assert.AreEqual(0, this._emojiAuthorManager.GetCurrentAuthorCount());
            Assert.AreEqual(1, playList.Count);
        }
    }
}
