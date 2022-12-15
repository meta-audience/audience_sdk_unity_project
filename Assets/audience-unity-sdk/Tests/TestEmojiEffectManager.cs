using AudienceSDK;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AudienceSDK.UnitTest {
    class TestEmojiEffectManager {

        private EmojiEffectManager _emojiEffectManager = null;

        [SetUp]
        public void Setup() {
            var obj = new GameObject();
            this._emojiEffectManager = obj.AddComponent<EmojiEffectManager>();
        }

        [Test]
        public void TestEmojiAssetListAndUrlNullWillReturnNetworkDBDataError() {
            ChatAuthor author = new ChatAuthor();
            EmojiMessage messsage = new Emoji2DMessage();
            messsage.asset_list = null;
            messsage.url = null;
            
            var rc = this._emojiEffectManager.CreateEmoji(author, messsage);

            LogAssert.Expect(LogType.Error, "[Error][Emoji]CreateEmoji Fail, emoji asset list and url are empty.");
            Assert.AreEqual(rc, AudienceReturnCode.AudienceSDKNetworkDBDataError);
        }

    }
}
