using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AudienceSDK.mgGif {
    public class GifDataHolder {

        public bool Finished { get; private set; } = false;

        public int AvailableFrame { get; private set; } = 0;

        public List<Texture2D> TexList { get; private set; } = null;

        public List<float> DelayList { get; private set; } = null;

        private string keyword;
        private byte[] _data;
        private MonoBehaviour _behaviour;

        // From undecoded data
        public GifDataHolder(string keyword, byte[] data, MonoBehaviour behaviour) {
            this.keyword = keyword;
            this._data = data;
            this._behaviour = behaviour;
            this.Finished = false;
        }

        // From cache
        public GifDataHolder(List<Texture2D> texList, List<float> delayList) {
            this.TexList = texList;
            this.DelayList = delayList;
            this.AvailableFrame = texList.Count;
            this.Finished = true;
        }

        public void StartDecode() {
            if (!this.Finished) {
                this.TexList = new List<Texture2D>();
                this.DelayList = new List<float>();
                this.AvailableFrame = 0;

                this._behaviour.StartCoroutine(this.Decode());
            }
        }

        private IEnumerator Decode() {
            using (var decoder = new MG.GIF.Decoder(this._data)) {
                var img = decoder.NextImage();
                while (img != null) {
                    this.TexList.Add(img.CreateTexture());
                    this.DelayList.Add(img.Delay / 1000.0f);    // Convert ms to s
                    ++this.AvailableFrame;

                    yield return null;

                    img = decoder.NextImage();
                }

                AssetLoader.SetCachedGif(this.keyword, this.TexList, this.DelayList);
                this.Finished = true;
            }
        }
    }
}
