using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AudienceSDK {
    public static class MessageDispatcher {
        public static MessageBase ConvertMessage(ChatMessage inputMessage) {
            switch (inputMessage.type) {
                case (int)ChatMessageType.Emoji_2D:
                    {
                        var outputMsg = new Emoji2DMessage();
                        outputMsg.type = inputMessage.type;
                        outputMsg.text = inputMessage.text;
                        outputMsg.url = inputMessage.url;
                        outputMsg.asset_list = inputMessage.asset_list;
                        outputMsg.animation = inputMessage.animation;
                        return outputMsg;
                    }

                case (int)ChatMessageType.Emoji_3D:
                    {
                        var outputMsg = new Emoji3DMessage();
                        outputMsg.type = inputMessage.type;
                        outputMsg.text = inputMessage.text;
                        outputMsg.url = inputMessage.url;
                        outputMsg.asset_list = inputMessage.asset_list;
                        outputMsg.animation = inputMessage.animation;
                        return outputMsg;
                    }

                case (int)ChatMessageType.Text:
                default:
                    {
                        var outputMsg = new TextMessage();
                        outputMsg.type = inputMessage.type;
                        outputMsg.text = inputMessage.text;
                        return outputMsg;
                    }
            }
        }
    }

    // MessageBase is for MessageBaseDispatcher, center them to same .cs file
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public abstract class MessageBase {
        public int type { get; set; }

        public string text { get; set; }
    }

    // EmojiMessage from MessageBase, center them to same .cs file
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public abstract class EmojiMessage : MessageBase {
        public string url { get; set; }

        public List<EmojiRawObject> asset_list { get; set; }

        public EmojiAnimation animation { get; set; }
    }

    // EmojiMessage from MessageBase, center them to same .cs file
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class TextMessage : MessageBase {
    }

    // Emoji2DMessage from EmojiMessage, center them to same .cs file
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Emoji2DMessage : EmojiMessage {
    }

    // Emoji3DMessage from EmojiMessage, center them to same .cs file
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class Emoji3DMessage : EmojiMessage {
    }
}
