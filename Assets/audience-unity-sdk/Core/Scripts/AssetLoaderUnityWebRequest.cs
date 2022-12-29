﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AudienceSDK {

    public class AssetLoaderUnityWebRequest : IInternalAssetLoader {
        public void LoadAssetBundle(string url, AssetLoader.LoadedAssetBundle data) {
            Audience.Context.ChatMessageManager.StartCoroutine(this.GetAssetBundleFromUrl(url, data));
        }

        public void LoadTexture2D(string url, AssetLoader.LoadedTexture2D data) {
            Audience.Context.ChatMessageManager.StartCoroutine(this.GetTexture2DFromUrl(url, data));
        }

        public void LoadGif(string url, AssetLoader.LoadedGif data) {
            Audience.Context.ChatMessageManager.StartCoroutine(this.GetGifFromUrl(url, data));
        }

        public void LoadImage(string url, AssetLoader.LoadedImage data) {
            Audience.Context.ChatMessageManager.StartCoroutine(this.GetImageFromUrl(url, data));
        }

        private IEnumerator GetAssetBundleFromUrl(string url, AssetLoader.LoadedAssetBundle data) {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
                data.Result = AudienceReturnCode.AudienceSDKNetworkError;
            } else {
                data.AssetBundle = DownloadHandlerAssetBundle.GetContent(request);
                data.Result = AudienceReturnCode.AudienceSDKOk;
            }

            data.Finished = true;
        }

        private IEnumerator GetTexture2DFromUrl(string url, AssetLoader.LoadedTexture2D data) {

            // It's workaround for DVAT-3939
            if (url.EndsWith(".svg")) {
                url = url.Remove(url.Length - 3) + "png";
                Debug.LogWarning("GetTexture2DFromUrl change svg url to png.");
            }

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
                data.Result = AudienceReturnCode.AudienceSDKNetworkError;
            } else {
                data.Tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                data.Result = AudienceReturnCode.AudienceSDKOk;
            }

            data.Finished = true;
        }

        private IEnumerator GetGifFromUrl(string url, AssetLoader.LoadedGif data) {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            byte[] rawData;

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
                data.Result = AudienceReturnCode.AudienceSDKNetworkError;
            } else {
                data.Finished = true;
                data.Result = AudienceReturnCode.AudienceSDKOk;
                data.TexList = new System.Collections.Generic.List<Texture2D>();
                data.DelayList = new System.Collections.Generic.List<float>();

                rawData = request.downloadHandler.data;
                using (var decoder = new MG.GIF.Decoder(rawData)) {
                    var img = decoder.NextImage();
                    while (img != null) {
                        data.TexList.Add(img.CreateTexture());
                        data.DelayList.Add(img.Delay / 1000.0f);    // Convert ms to s
                        yield return null;

                        img = decoder.NextImage();
                    }

                    data.Decoded = true;
                }
            }
        }

        private IEnumerator GetImageFromUrl(string url, AssetLoader.LoadedImage data) {

            bool svgWorkaround = false;

            // It's workaround for DVAT-3939
            if (url.EndsWith(".svg")) {
                url = url.Remove(url.Length - 3) + "png";
                Debug.LogWarning("GetTexture2DFromUrl change svg url to png.");
                svgWorkaround = true;
            }

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            byte[] rawData;

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
                data.Result = AudienceReturnCode.AudienceSDKNetworkError;
            } else {
                data.Tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                data.Result = AudienceReturnCode.AudienceSDKOk;

                rawData = request.downloadHandler.data;

                data.IsGif = !svgWorkaround && this.IsGif(rawData);
                data.RawData = rawData;
                data.Finished = true;
            }
        }

        private byte[] gifPattern = new byte[] { 0x4E, 0x45, 0x54, 0x53, 0x43, 0x41, 0x50, 0x45, 0x32, 0x2E, 0x30 };

        private bool IsGif(byte[] data) {

            byte[] pattern = this.gifPattern;

            Debug.Log("[AssetLoaderUnityWebRequest] - IsGif - data[0]:" + data[0]);

            if (data.Length < pattern.Length || data[0] != 0x47) {
                return false;
            }

            var p = 0;
            for (int i = 0; i < data.Length; ++i) {
                if (data[i] != pattern[p]) {
                    p = 0;
                    continue;
                }

                ++p;

                if (p == pattern.Length) {
                    Debug.Log("[AssetLoaderUnityWebRequest] - IsGif - Find Gif pattern at " + i);
                    return true;
                }
            }

            return p == pattern.Length;
        }
    }
}