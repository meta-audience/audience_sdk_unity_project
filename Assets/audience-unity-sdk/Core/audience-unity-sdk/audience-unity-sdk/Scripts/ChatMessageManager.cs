using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace AudienceSDK {
    public class ChatMessageManager : MonoBehaviour {
        private int _maxChatSchemaQueueCount = 1000;
        private Queue<ChatSchema> _chatSchemaQueue;
        private bool _isMessageProcessing = false;

        internal void HandleChatMessageReceived(string obj) {
            if (this._chatSchemaQueue.Count > this._maxChatSchemaQueueCount) {
                Debug.LogWarning("HandleChatMessageReceived - chat Schema Queue out of limit.");
                return;
            }

            ChatSchema chatSchema = null;
            try {
                chatSchema = JsonConvert.DeserializeObject<ChatSchema>(obj);
            } catch {
                Debug.Log(obj);
                Debug.LogWarning("HandleChatMessageReceived - Failed parsing chat message.");
                return;
            }

            this._chatSchemaQueue.Enqueue(chatSchema);
        }

        private void Awake() {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this._chatSchemaQueue = new Queue<ChatSchema>();
        }

        private void Start() {
            this._isMessageProcessing = true;
            this.StartCoroutine(this.ProcessReceivedMessageQueue());
        }

        private void OnDestroy() {
            this._isMessageProcessing = false;
        }

        private IEnumerator ProcessReceivedMessageQueue() {
            while (this._isMessageProcessing) {
                if (this._chatSchemaQueue.Count > 0) {
                    var schema = this._chatSchemaQueue.Dequeue();
                    var emojiMessageQueue = new Queue<EmojiMessage>();

                    foreach (ChatMessage chatMessage in schema.data.message) {
                        MessageBase convertedMessage = MessageDispatcher.ConvertMessage(chatMessage);
                        if (convertedMessage == null || !(convertedMessage is EmojiMessage)) {
                            continue;
                        }

                        var emojiMessage = convertedMessage as EmojiMessage;
                        emojiMessageQueue.Enqueue(emojiMessage);
                        if (emojiMessageQueue.Count >= UserConfig.EmojiLimitPerMessage) {
                            break;
                        }
                    }

                    if (emojiMessageQueue.Count > 0) {
                        Audience.Context.EmojiAuthorManager.AddEmojiSentence(schema.data.author, emojiMessageQueue);
                    }
                }

                yield return null;
            }
        }
    }
}
