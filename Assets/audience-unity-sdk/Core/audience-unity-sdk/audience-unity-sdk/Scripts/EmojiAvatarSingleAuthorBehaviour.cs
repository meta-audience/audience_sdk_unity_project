using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AudienceSDK {
    public class EmojiAvatarSingleAuthorBehaviour : EmojiAvatarBehaviourBase {

        public override AudienceReturnCode SetAuthors(List<ChatAuthor> authors) {

            this._authors.Clear();
            this._authors = authors.ToList();

            if (this._authors.Count != 1) {
                Debug.LogError("SingleAuthor Avatar not support multiple authors");
                return AudienceReturnCode.AudienceInvalidParams;
            }

            if (this.transform.Find("Platform") == null) {
                Debug.LogError("SingleAuthor Avatar prefab missing component Platform.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            if (this.transform.Find("Nickname") == null || this.transform.Find("Nickname").GetComponent<TextMesh>() == null) {
                Debug.LogError("SingleAuthor Avatar prefab missing component Nickname.");
                return AudienceReturnCode.AudienceSDKInternalError;
            }

            var platform = this.transform.Find("Platform");
            var nicknameText = this.transform.Find("Nickname").GetComponent<TextMesh>();

            if (this._authors[0].platform == "Youtube") {
                platform.Find("youtube_logo").gameObject.SetActive(true);
            } else if (this._authors[0].platform == "Twitch") {
                platform.Find("twitch_logo").gameObject.SetActive(true);
            }

            nicknameText.text = this._authors[0].name;
            return AudienceReturnCode.AudienceSDKOk;
        }
    }
}
