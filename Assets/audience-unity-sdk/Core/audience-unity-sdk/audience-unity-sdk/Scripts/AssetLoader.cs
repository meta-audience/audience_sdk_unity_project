using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK {

    public class AssetLoader {

        public class LoadedAssetBundle {
            public bool Finished { get; set; }

            public AudienceReturnCode Result { get; set; }

            public AssetBundle AssetBundle { get; set; }
        }

        public class LoadedTexture2D {
            public bool Finished { get; set; }

            public AudienceReturnCode Result { get; set; }

            public Texture2D Tex { get; set; }
        }

        public class LoadedGif {
            public bool Finished { get; set; }

            public bool Decoded { get; set; }

            public AudienceReturnCode Result { get; set; }

            public List<Texture2D> TexList { get; set; }

            public List<float> DelayList { get; set; }
        }

        public class LoadedImage {
            public bool Finished { get; set; }

            public AudienceReturnCode Result { get; set; }

            public bool IsGif { get; set; }

            public Texture2D Tex { get; set; } = null; // Only available under non-gif

            public byte[] RawData { get; set; } = null; // Only available under gif image
        }

        private class CachedPrefab {
            public string Keyword { get; set; }

            public int Version { get; set; }

            public GameObject Prefab { get; set; }
        }

        private class CachedAnimationClip {
            public string Keyword { get; set; }

            public int Version { get; set; }

            public AnimationClip Clip { get; set; }
        }

        // 2D texture just cache, needn't care version.
        private class CachedTexture2D {
            public string Keyword { get; set; }

            public Texture2D Tex { get; set; }
        }

        private class CachedGif {
            public string Keyword { get; set; }

            public List<Texture2D> TexList { get; set; }

            public List<float> DelayList { get; set; }
        }

        private static List<CachedPrefab> cachedPrefabList = new List<CachedPrefab>();
        private static List<CachedAnimationClip> cachedAnimationClipList = new List<CachedAnimationClip>();
        private static List<CachedTexture2D> cachedTexture2DList = new List<CachedTexture2D>();
        private static List<CachedGif> cachedGifList = new List<CachedGif>();

        /// <summary>
        /// Asynchronously load raw data from url.
        /// </summary>
        /// <param name="url"> url or file_path (start with file://). </param>
        /// <returns> Loading status and returned data. </returns>
        public static LoadedAssetBundle LoadAssetBundle(string url) {
            IInternalAssetLoader loader = GetLoader();
            LoadedAssetBundle returnData = new LoadedAssetBundle() {
                Finished = false,
            };

            loader.LoadAssetBundle(url, returnData);

            return returnData;
        }

        /// <summary>
        /// Asynchronously load image file as a texture from url.
        /// </summary>
        /// <param name="url"> url or file_path (start with file://). </param>
        /// <returns> Loading status and returned texture. </returns>
        public static LoadedTexture2D LoadTexture2D(string url) {
            IInternalAssetLoader loader = GetLoader();
            LoadedTexture2D returnData = new LoadedTexture2D() {
                Finished = false,
            };

            loader.LoadTexture2D(url, returnData);

            return returnData;
        }

        /// <summary>
        /// Asynchronously load GIF from url.
        /// </summary>
        /// <param name="url"> url or file_path (start with file://). </param>
        /// <returns> Loading status and returned data. </returns>
        public static LoadedGif LoadGif(string url) {
            IInternalAssetLoader loader = GetLoader();
            LoadedGif returnData = new LoadedGif() {
                Decoded = false,
                Finished = false,
            };

            loader.LoadGif(url, returnData);

            return returnData;
        }

        /// <summary>
        /// Asynchronously load GIF from url.
        /// </summary>
        /// <param name="url"> url or file_path (start with file://). </param>
        /// <returns> Loading status and returned data. </returns>
        public static LoadedImage LoadImage(string url) {
            IInternalAssetLoader loader = GetLoader();
            LoadedImage returnData = new LoadedImage() {
                Finished = false,
            };

            loader.LoadImage(url, returnData);

            return returnData;
        }

        public static GameObject GetCachedPrefab(string keyword, int version) {
            var cachedPrefab = cachedPrefabList.Find(x => x.Keyword == keyword && x.Version >= version);
            if (cachedPrefab != null) {
                return cachedPrefab.Prefab;
            }

            return null;
        }

        public static void SetCachedPrefab(string keyword, int version, GameObject prefab) {
            var cachedPrefab = new CachedPrefab() {
                Keyword = keyword,
                Version = version,
                Prefab = prefab,
            };

            cachedPrefabList.Add(cachedPrefab);
        }

        public static AnimationClip GetCachedAnimationClip(string keyword, int version) {
            var cachedAnimationClip = cachedAnimationClipList.Find(x => x.Keyword == keyword && x.Version >= version);
            if (cachedAnimationClip != null) {
                return cachedAnimationClip.Clip;
            }

            return null;
        }

        public static void SetCachedAnimationClip(string keyword, int version, AnimationClip clip) {
            var cachedAnimationClip = new CachedAnimationClip() {
                Keyword = keyword,
                Version = version,
                Clip = clip,
            };

            cachedAnimationClipList.Add(cachedAnimationClip);
        }

        public static Texture2D GetCachedTexture2D(string keyword) {
            var cachedTexture2D = cachedTexture2DList.Find(x => x.Keyword == keyword);
            if (cachedTexture2D != null) {
                return cachedTexture2D.Tex;
            }

            return null;
        }

        public static void SetCachedTexture2D(string keyword, Texture2D tex) {
            var cachedTexture2D = new CachedTexture2D() {
                Keyword = keyword,
                Tex = tex,
            };

            cachedTexture2DList.Add(cachedTexture2D);
        }

        public static bool GetCachedGif(string keyword, out List<Texture2D> texList, out List<float> delayList) {
            var cachedData = cachedGifList.Find(x => x.Keyword == keyword);
            if (cachedData != null) {
                texList = cachedData.TexList;
                delayList = cachedData.DelayList;
                return true;
            }

            texList = null;
            delayList = null;

            return false;
        }

        public static void SetCachedGif(string keyword, List<Texture2D> texList, List<float> delayList) {
            var cachedGif = new CachedGif() {
                Keyword = keyword,
                TexList = texList,
                DelayList = delayList,
            };

            cachedGifList.Add(cachedGif);
        }

        private static IInternalAssetLoader GetLoader() {
            return new AssetLoaderUnityWebRequest();
        }
    }

    public interface IInternalAssetLoader {

        void LoadAssetBundle(string url, AssetLoader.LoadedAssetBundle data);

        void LoadTexture2D(string url, AssetLoader.LoadedTexture2D data);

        void LoadGif(string url, AssetLoader.LoadedGif data);

        void LoadImage(string url, AssetLoader.LoadedImage data);
    }
}