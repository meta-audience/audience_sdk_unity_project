using System;
using System.Collections;
using System.Collections.Generic;
using AudienceSDK.mgGif;
using UnityEngine;
using UnityEngine.Networking;

namespace AudienceSDK {
    public class Emoji2DBehaviour : EmojiBehaviourBase {

        private const float _spriteSize = 0.24f;
        private static Material _material = null;
        private SpriteRenderer _renderer = null;
        private bool _isGif = false;

        // For gif texture. brad: This will be further improved in next card.(DVAT-3949)
        private int _index = 0;
        private float _timer = 0.0f;
        private GifDataHolder _gifData;

        protected override IEnumerator LoadEmojiModel(string keyword, object asset) {

            // Type check
            if (!(asset is string)) {
                Debug.LogWarning("ShowImageFromUrl Failed to cast asset to string");
                UnityEngine.Object.DestroyImmediate(this.gameObject);
                yield break;
            }

            var emojiTexture = AssetLoader.GetCachedTexture2D(keyword);
            if (emojiTexture != null) {
                this._isGif = false;
                this.LoadEmojiModelTexture(emojiTexture);
            } else {
                List<Texture2D> texList;
                List<float> delayList;
                var cached = AssetLoader.GetCachedGif(keyword, out texList, out delayList);
                if (cached) {
                    this._isGif = true;
                    this._gifData = new GifDataHolder(texList, delayList);
                    this.LoadEmojiModelGif();
                } else {
                    // Use load image to load either static or gif image.
                    AssetLoader.LoadedImage data = AssetLoader.LoadImage(asset.ToString());
                    while (!data.Finished) {
                        yield return null;
                    }

                    if (data.Result != AudienceReturnCode.AudienceSDKOk) {
                        Debug.LogWarning("ShowImageFromUrl Failed to load texture");
                        UnityEngine.Object.DestroyImmediate(this.gameObject);
                        yield break;
                    }

                    if (data.IsGif) {
                        this._isGif = true;
                        var gifData = new GifDataHolder(keyword, data.RawData, this);
                        gifData.StartDecode();
                        while (gifData.AvailableFrame < 1 && !gifData.Finished) {
                            // Wait until the first frame is decoded
                            yield return null;
                        }

                        this._gifData = gifData;
                        this.LoadEmojiModelGif();
                    } else {
                        emojiTexture = data.Tex;
                        if (emojiTexture == null) {
                            Debug.LogWarning(" ShowImageFromUrl missing texture2D.");
                            UnityEngine.Object.DestroyImmediate(this.gameObject);
                            yield break;
                        }

                        if (emojiTexture == null || emojiTexture.width <= 0 || emojiTexture.height <= 0) {
                            Debug.LogWarning("ShowImageFromUrl invalid texture.");
                            UnityEngine.Object.DestroyImmediate(this.gameObject);
                            yield break;
                        }

                        this._isGif = false;
                        AssetLoader.SetCachedTexture2D(keyword, emojiTexture);

                        this.LoadEmojiModelTexture(emojiTexture);
                    }
                }
            }
        }

        private void LoadEmojiModelTexture(Texture2D emojiTexture) {
            this._emojiModel = new GameObject("Emoji Model");
            this._emojiModel.transform.SetParent(this.transform);
            this._emojiModel.transform.localEulerAngles = Vector3.zero;
            this._emojiModel.transform.localPosition = Vector3.zero;
            this._emojiModel.AddComponent<Animation>();

            var emojiObj = new GameObject("Emoji Obj");
            emojiObj.transform.SetParent(this._emojiModel.transform);
            emojiObj.transform.localEulerAngles = new Vector3(0, 180, 0);
            emojiObj.transform.localPosition = Vector3.zero;
            SpriteRenderer sr = emojiObj.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.drawMode = SpriteDrawMode.Sliced;

            // Assign the sprite to sprite renderer
            sr.sprite = Sprite.Create(emojiTexture, new Rect(0.0f, 0.0f, emojiTexture.width, emojiTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            // make all 2D Emoji has same size.
            if (emojiTexture.width >= emojiTexture.height) {
                sr.size = new Vector2(_spriteSize, ((float)emojiTexture.height / (float)emojiTexture.width) * _spriteSize);
            } else {
                sr.size = new Vector2(((float)emojiTexture.width / (float)emojiTexture.height) * _spriteSize, _spriteSize);
            }

            // Assign the material to sprite renderer
            Material mat = this.GetSharedMaterial();
            sr.material = mat;
            sr.sharedMaterial = mat;
        }

        private void LoadEmojiModelGif() {

            if (this._gifData.AvailableFrame < 1) {
                Debug.LogError("LoadEmojiModelGif - require at least one gif frame for sprite initialization.");
                return;
            }

            var tex = this._gifData.TexList[0];
            this._index = 0;

            this._emojiModel = Instantiate(new GameObject(), this.transform);
            this._emojiModel.transform.localEulerAngles = Vector3.zero;
            this._emojiModel.transform.localPosition = Vector3.zero;
            this._emojiModel.AddComponent<Animation>();

            var emojiObj = Instantiate(new GameObject(), this._emojiModel.transform);
            emojiObj.transform.localEulerAngles = new Vector3(0, 180, 0);
            emojiObj.transform.localPosition = Vector3.zero;
            SpriteRenderer sr = emojiObj.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.drawMode = SpriteDrawMode.Sliced;

            // Assign the sprite to sprite renderer
            sr.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            // make all 2D Emoji has same size.
            if (tex.width >= tex.height) {
                sr.size = new Vector2(_spriteSize, ((float)tex.height / (float)tex.width) * _spriteSize);
            } else {
                sr.size = new Vector2(((float)tex.width / (float)tex.height) * _spriteSize, _spriteSize);
            }

            this._renderer = sr;

            // Assign the material to sprite renderer
            Material mat = this.GetSharedMaterial();
            sr.material = mat;
            sr.sharedMaterial = mat;
        }

        private Material GetSharedMaterial() {
            if (_material == null) {
                var shader = Shader.Find("Particles/Standard Unlit");
                Material mat = null;
                if (shader != null) {
                    mat = new Material(shader);
                    mat.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
                    mat.SetFloat("_Mode", 3);
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }

                _material = mat;
            }

            return _material;
        }

        private void Update() {
            if (this._isGif && this._gifData != null && this._gifData.AvailableFrame > 0) {
                this._timer += Time.deltaTime;

                int i = this._index;
                float delay = this._gifData.DelayList[i];
                if (this._timer > delay) {
                    if (this._gifData.Finished || this._index < this._gifData.AvailableFrame - 1) {
                        // Next frame is available now
                        i = (i + 1) % this._gifData.AvailableFrame;
                        MaterialPropertyBlock block = new MaterialPropertyBlock();
                        block.SetTexture("_MainTex", this._gifData.TexList[i]);
                        this._renderer.SetPropertyBlock(block);
                        this._timer -= delay;
                        this._index = i;
                    }
                }
            }
        }
    }
}
