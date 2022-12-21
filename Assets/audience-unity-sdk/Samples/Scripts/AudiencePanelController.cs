using System.Collections;
using System.Collections.Generic;
using AudienceSDK;
using UnityEngine;

namespace AudienceSDK.Sample
{
    public class AudiencePanelController : MonoBehaviour
    {
        private bool audienceControlPanelInited = false;

        private List<NativeSceneSummaryData> CachedProfiles { get; set; }

        private void Start()
        {
            if (Audience.Instance == null) {
                Debug.LogWarning("[AudienceControlPanel] Audience does not exist in this scene.");
                return;
            }

            if (Audience.Instance.AudienceInited) {
                this.InitPanelController();
            }

            Audience.Instance.onAudienceInitStateChanged += OnAudienceInitStateChanged;
        }

        private void OnDestroy()
        {
            Audience.Instance.onAudienceInitStateChanged -= OnAudienceInitStateChanged;
        }

        private void InitPanelController() {
            if (!this.audienceControlPanelInited) {
                AudienceSDK.Audience.Context.RefreshSceneListCompleted += this.OnRefreshSceneListCompleted;
                this.audienceControlPanelInited = true;
            }
        }

        private void DeInitPanelController() {
            if (this.audienceControlPanelInited) {
                AudienceSDK.Audience.Context.RefreshSceneListCompleted -= this.OnRefreshSceneListCompleted;
                this.audienceControlPanelInited = false;
            }
        }

        private void OnAudienceInitStateChanged(bool initState) {
            if (initState)
            {
                this.InitPanelController();
            }
            else 
            {
                this.DeInitPanelController();
            }
        }

        private void OnRefreshSceneListCompleted(List<NativeSceneSummaryData> sceneList)
        {
            Debug.LogFormat("[AudienceControlPanel] RefreshSceneListCompleted: size={0}", sceneList.Count);
            this.CachedProfiles = sceneList;
        }
    }
}