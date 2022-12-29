using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK {
    public class EmojiAuthorManager : IEmojiAuthorManager {

        private float _intervalBetweenEmojis = 0.5f;
        private List<IEmojiSentencesInAuthor> _emojiAuthorList;
        private IEmojiSentencesInAuthorFactory _emojiSentencesInAuthorFactory;
        private bool _isMuteEmoji = false;

        public EmojiAuthorManager(IEmojiSentencesInAuthorFactory factory) {
            this._emojiAuthorList = new List<IEmojiSentencesInAuthor>();
            this._emojiSentencesInAuthorFactory = factory;
        }

        ~EmojiAuthorManager() {
            this._emojiAuthorList = null;
            this._emojiSentencesInAuthorFactory = null;
        }

        public AudienceReturnCode AddEmojiSentence(ChatAuthor author, Queue<EmojiMessage> sentence) {

            if (this._emojiAuthorList == null) {
                Debug.LogError("AddEmojiSentence failed, EmojiAuthorManager init incomplete.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            if (this._isMuteEmoji) {
                Debug.Log("AddEmojiSentence failed, emoji had been muted.");
                return AudienceReturnCode.AudienceSDKInvalidState;
            }

            if (author == null) {
                Debug.LogWarning("AddEmojiSentence failed, author is null.");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            if (author.platform == null || author.user_id == null) {
                Debug.LogWarning("AddEmojiSentence failed, author is not complete.");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            if (sentence == null || sentence.Count <= 0) {
                Debug.LogWarning("AddEmojiSentence failed, Emoji sentence is empty.");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            var authorIndex = this._emojiAuthorList.FindIndex(x =>
            (x.GetAuthor().platform == author.platform && x.GetAuthor().user_id == author.user_id));

            // Find in list, AddEmojiSentence directly; not in list, call factory create the add to list.
            if (authorIndex != -1) {
                return this._emojiAuthorList[authorIndex].AddEmojiSentence(sentence);
            } else {
                if (this._emojiAuthorList.Count >= UserConfig.EmojiMaxAuthors) {
                    Debug.LogWarning("AddEmojiSentence failed, Emoji author out of limit.");
                    return AudienceReturnCode.AudienceBufferSizeNotEnough;
                }

                var newAuthor = this._emojiSentencesInAuthorFactory.CreateEmojiSentencesInAuthor(author);
                if (newAuthor != null) {
                    var rc = newAuthor.AddEmojiSentence(sentence);
                    if (rc != AudienceReturnCode.AudienceSDKOk) {
                        return rc;
                    } else {
                        this._emojiAuthorList.Add(newAuthor);
                    }
                } else {
                    Debug.LogWarning("AddEmojiSentence failed, author factory create failed.");
                    return AudienceReturnCode.AudienceSDKNullPtr;
                }
            }

            return AudienceReturnCode.AudienceSDKOk;
        }

        public Dictionary<ChatAuthor, EmojiMessage> ExtractCandidateEmojis() {
            Dictionary<ChatAuthor, EmojiMessage> candidateEmojis = new Dictionary<ChatAuthor, EmojiMessage>();

            if (this._emojiAuthorList == null) {
                Debug.LogError("ExtractCandidateEmojis failed, EmojiAuthorManager init incomplete.");
                return candidateEmojis;
            }

            var realtimeSinceStartup = Time.realtimeSinceStartup;
            for (int i = this._emojiAuthorList.Count - 1; i >= 0; i--) {
                EmojiMessage emojiMsg = null;
                var leftSize = 0;
                var rc = this._emojiAuthorList[i].AcquireNextEmoji(realtimeSinceStartup, this._intervalBetweenEmojis, out emojiMsg, out leftSize);
                if (rc == AudienceReturnCode.AudienceSDKOk) {
                    candidateEmojis.Add(this._emojiAuthorList[i].GetAuthor(), emojiMsg);
                } else if (rc == AudienceReturnCode.AudienceSDKInvalidState) {
                    // emojiAuthor is cooling down, do nothing
                } else {
                    this._emojiAuthorList.RemoveAt(i);
                    continue;
                }

                if (leftSize <= 0) {
                    this._emojiAuthorList.RemoveAt(i);
                }
            }

            return candidateEmojis;
        }

        public void OnMuteEmojiChanged(bool enable) {
            this._isMuteEmoji = enable;
            if (this._isMuteEmoji) {
                this._emojiAuthorList.Clear();
            }
        }

        public int GetCurrentAuthorCount() {
            return this._emojiAuthorList == null ? 0 : this._emojiAuthorList.Count;
        }
    }
}
