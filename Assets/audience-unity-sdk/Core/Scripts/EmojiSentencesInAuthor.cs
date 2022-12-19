using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK {
    public class EmojiSentencesInAuthor : IEmojiSentencesInAuthor {

        private Queue<Queue<EmojiMessage>> _emojiSentenceQueue;
        private ChatAuthor _sentenceAuthor = new ChatAuthor();

        private float _lastAcquireTime = 0f;

        public EmojiSentencesInAuthor(ChatAuthor author) {
            this._emojiSentenceQueue = new Queue<Queue<EmojiMessage>>();
            this._sentenceAuthor = author;
        }

        ~EmojiSentencesInAuthor() {
            this._emojiSentenceQueue = null;
        }

        public AudienceReturnCode AddEmojiSentence(Queue<EmojiMessage> sentence) {
            if (this._emojiSentenceQueue == null) {
                Debug.LogError("AddSentence failed, EmojiSentencesInAuthor init incomplete.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            if (this._emojiSentenceQueue.Count >= UserConfig.EmojiMaxSentencesInOneAuthor) {
                Debug.LogWarning("AddSentence failed, queue out of limit.");
                return AudienceReturnCode.AudienceBufferSizeNotEnough;
            }

            this._emojiSentenceQueue.Enqueue(sentence);
            return AudienceReturnCode.AudienceSDKOk;
        }

        public AudienceReturnCode AcquireNextEmoji(float realtimeSinceStartup, float timeInterval, out EmojiMessage emojiMsg, out int leftSize) {
            leftSize = 0;
            emojiMsg = null;
            if (this._emojiSentenceQueue == null) {
                Debug.LogError("AcquireNextEmoji failed, EmojiSentencesInAuthor init incomplete.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            if (realtimeSinceStartup - this._lastAcquireTime < timeInterval) {
                // comment this log, this log will flooded the entire log file.
                // Debug.Log("AcquireNextEmoji failed, EmojiSentencesInAuthor is cooling down.");
                leftSize = this.CalculateLeftSize();
                return AudienceReturnCode.AudienceSDKInvalidState;
            }

            // try to get next emoji message from 2-dimension queue.
            while (this._emojiSentenceQueue.Count > 0) {
                var peekSentence = this._emojiSentenceQueue.Peek();
                if (peekSentence.Count > 0) {
                    emojiMsg = peekSentence.Dequeue();

                    this._lastAcquireTime = realtimeSinceStartup;
                    if (peekSentence.Count <= 0) {
                        this._emojiSentenceQueue.Dequeue();
                    }

                    leftSize = this.CalculateLeftSize();
                    return AudienceReturnCode.AudienceSDKOk;
                } else {
                    this._emojiSentenceQueue.Dequeue();
                    continue;
                }
            }

            Debug.Log("AcquireNextEmoji failed, EmojiSentencesInAuthor queue is empty.");
            return AudienceReturnCode.AudienceSDKDataNotFound;
        }

        public float GetLastAcquireTime() {
            return this._lastAcquireTime;
        }

        public int GetSentenceCount() {
            return this._emojiSentenceQueue.Count;
        }

        public ChatAuthor GetAuthor() {
            return this._sentenceAuthor;
        }

        private int CalculateLeftSize() {
            var leftSize = 0;
            if (this._emojiSentenceQueue == null) {
                return leftSize;
            }

            // while loop traverse SentenceQueue to calcuate emoji total count.
            IEnumerator<Queue<EmojiMessage>> enumerator = this._emojiSentenceQueue.GetEnumerator();
            while (enumerator.MoveNext()) {
                leftSize += enumerator.Current.Count;
            }

            return leftSize;
        }
    }
}
