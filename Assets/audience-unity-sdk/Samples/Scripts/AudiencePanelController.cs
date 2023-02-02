using System.Collections;
using System.Collections.Generic;
using AudienceSDK;
using UnityEngine;

namespace AudienceSDK.Sample
{
    public class AudiencePanelController : MonoBehaviour
    {
        private bool audienceControlPanelInited = false;

        [SerializeField]
        private AudiencePanelView audiencePanelView = null;

        private void Start()
        {
            if (AudienceSDK.Audience.AudienceInited) {
                this.InitPanelController();
            }

            AudienceSDK.Audience.AudienceInitStateChanged += this.OnAudienceInitStateChanged;
        }

        private void OnDestroy()
        {
            this.DeInitPanelController();
            AudienceSDK.Audience.AudienceInitStateChanged -= this.OnAudienceInitStateChanged;
        }

        private void InitPanelController() {
            if (!this.audienceControlPanelInited) {
                AudienceSDK.Audience.Context.LogonStateChanged += this.OnLogonStateChanged;
                AudienceSDK.Audience.Context.StreamStateChanged += this.OnStreamStateChanged;
                AudienceSDK.Audience.Context.RefreshSceneListCompleted += this.OnRefreshSceneListCompleted;
                this.audiencePanelView.InitPanelView();
                this.audienceControlPanelInited = true;
            }
        }

        private void DeInitPanelController() {
            if (this.audienceControlPanelInited) {
                AudienceSDK.Audience.Context.LogonStateChanged -= this.OnLogonStateChanged;
                AudienceSDK.Audience.Context.StreamStateChanged -= this.OnStreamStateChanged;
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

        private void OnLogonStateChanged(LogonState state) {
            switch (state) {
                case LogonState.LoggedIn:
                    this.audiencePanelView.DisplayLoginCompletedView();
                    break;
                case LogonState.LoggedOut:
                    this.audiencePanelView.DisplayLogoutCompletedView();
                    break;
                case LogonState.LoggingIn:
                case LogonState.LoggingOut:
                default:
                    break;
            }
        }

        private void OnStreamStateChanged(StreamState state) {
            switch (state)
            {
                case StreamState.Unload:
                    this.audiencePanelView.DisplayUnloadCompletedView();
                    break;
                case StreamState.Loaded:
                    this.audiencePanelView.DisplayLoadCompletedView();
                    break;
                case StreamState.Started:
                    this.audiencePanelView.DisplayStartStreamCompletedView();
                    break;
                case StreamState.Loading:
                case StreamState.Unloading:
                case StreamState.Starting:
                case StreamState.Stopping:
                default:
                    break;
            }
        }

        private void OnRefreshSceneListCompleted(List<NativeSceneSummaryData> sceneList)
        {
            Debug.LogFormat("[AudienceControlPanel] RefreshSceneListCompleted: size={0}", sceneList.Count);
            this.audiencePanelView.UpdateSceneList(sceneList);
        }
    }
}