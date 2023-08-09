using AudienceSDK;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AudienceSDK.UnitTest {
    public class TestChatMessage {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestChatSchemaFields1() {
            string testChatMessage = @"
{
   'chat_id':'8395035a-374b-4272-998e-602c29a8f2af',
    'author':{
        'platform':'youtube',
        'name':'Ian_Luo',
        'user_id':'120353655',
        'is_mod':false,
        'is_sub':false,
        'badges':null
    },
    'orginal_text':'LUL',
    'message':[
        {
        'type':1,
        'text':'LUL',
        'url':'https://static-cdn.jtvnw.net/emoticons/v2/425618/static/light/1.0'
        }
    ],
    'utc_timestamp':1656761183069
}
";
            // Check all fields
            var chatSchema = JsonConvert.DeserializeObject<ChatData>(testChatMessage);
            Assert.NotNull(chatSchema);
            Assert.AreEqual(chatSchema.chat_id, "8395035a-374b-4272-998e-602c29a8f2af");
            Assert.AreEqual(chatSchema.utc_timestamp, "1656761183069");

            var author = chatSchema.author;
            Assert.NotNull(author);
            Assert.AreEqual(author.platform, "youtube");
            Assert.AreEqual(author.name, "Ian_Luo");
            Assert.AreEqual(author.user_id, "120353655");
            Assert.AreEqual(author.is_mod, false);
            Assert.AreEqual(author.is_sub, false);
            Assert.IsNull(author.badges);

            var message = chatSchema.message;
            Assert.NotNull(message);
            Assert.AreEqual(message.Count, 1);
            Assert.AreEqual(message[0].type, (int)ChatMessageType.Emoji_2D);
            Assert.AreEqual(message[0].text, "LUL");
            Assert.AreEqual(message[0].url, "https://static-cdn.jtvnw.net/emoticons/v2/425618/static/light/1.0");
            Assert.IsNull(message[0].asset_list);
        }

        [Test]
        public void TestChatSchemaFields2() {
            string testChatMessage = @"
{
    'chat_id':'8395035a-374b-4272-998e-602c29a8f2af',
    'author':{
        'platform':'twitch',
        'name':'tl_tsai',
        'user_id':'120353655',
        'is_mod':false,
        'is_sub':false,
        'badges':null
    },
    'orginal_text':'#jonny#',
    'message':[
        {
        'type':2,
        'text':'#jonny#',
        'asset_list':[{'engine':'Unity','render_type':'SinglePass','asset':{'keyword':'#heart#','version':10,'url':'https://www.google.com/'}}],
        'animation':{
            'asset_list':[{'engine':'Unity','render_type':'SinglePass','asset':{'keyword':'wave','version':7,'url':'https://www.google.com/'}}]
        },
        'interaction':{}
        }
    ],
    'utc_timestamp':1656761187532
}
";
            // Check all fields
            var chatSchema = JsonConvert.DeserializeObject<ChatData>(testChatMessage);
            Assert.NotNull(chatSchema);
            Assert.AreEqual(chatSchema.chat_id, "8395035a-374b-4272-998e-602c29a8f2af");
            Assert.AreEqual(chatSchema.utc_timestamp, "1656761187532");

            var author = chatSchema.author;
            Assert.NotNull(author);
            Assert.AreEqual(author.platform, "twitch");
            Assert.AreEqual(author.name, "tl_tsai");
            Assert.AreEqual(author.user_id, "120353655");
            Assert.AreEqual(author.is_mod, false);
            Assert.AreEqual(author.is_sub, false);
            Assert.IsNull(author.badges);

            var message = chatSchema.message;
            Assert.NotNull(message);
            Assert.AreEqual(message.Count, 1);
            Assert.AreEqual(message[0].type, (int)ChatMessageType.Emoji_3D);
            Assert.AreEqual(message[0].text, "#jonny#");
            Assert.IsNull(message[0].url);
            Assert.AreEqual(message[0].asset_list[0].engine, "Unity");
            Assert.AreEqual(message[0].asset_list[0].render_type, "SinglePass");
            Assert.AreEqual(message[0].asset_list[0].asset.keyword, "#heart#");
            Assert.NotNull(message[0].animation.asset_list[0]);
            Assert.AreEqual(message[0].animation.asset_list[0].engine, "Unity");
            Assert.AreEqual(message[0].animation.asset_list[0].render_type, "SinglePass");
            Assert.AreEqual(message[0].animation.asset_list[0].asset.keyword, "wave");
            Assert.NotNull(message[0].interaction);
        }
    }
}