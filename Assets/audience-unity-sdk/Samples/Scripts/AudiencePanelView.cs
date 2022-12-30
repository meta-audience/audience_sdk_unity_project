using System.Collections;
using System.Collections.Generic;
using AudienceSDK;
using UnityEngine;
using UnityEngine.UI;

public class AudiencePanelView : MonoBehaviour
{
    [SerializeField]
    private Button minimizePanelButton = null;

    [SerializeField]
    private Button maxmizePanelButton = null;

    [SerializeField]
    private GameObject audienceBlocks = null;

    [SerializeField]
    private GameObject logOnBlock = null;

    [SerializeField]
    private InputField usernameInputField = null;

    [SerializeField]
    private InputField passwordInputField = null;

    [SerializeField]
    private Button logInButton = null;

    [SerializeField]
    private Button logOutButton = null;

    [SerializeField]
    private GameObject sceneBlock = null;

    [SerializeField]
    private Dropdown sceneListDropdown = null;

    [SerializeField]
    private Button refreshButton = null;

    [SerializeField]
    private Button loadButton = null;

    [SerializeField]
    private Button unloadButton = null;

    [SerializeField]
    private GameObject streamBlock = null;

    [SerializeField]
    private Button startStreamButton = null;

    [SerializeField]
    private Button stopStreamButton = null;

    private List<NativeSceneSummaryData> CachedSceneList { get; set; }

    // UI function
    public void MaxmizePanel() {
        this.audienceBlocks.SetActive(true);
        this.maxmizePanelButton.gameObject.SetActive(false);
        this.minimizePanelButton.gameObject.SetActive(true);
    }

    public void MinimizePanel() {
        this.audienceBlocks.SetActive(false);
        this.maxmizePanelButton.gameObject.SetActive(true);
        this.minimizePanelButton.gameObject.SetActive(false);
    }

    // API function
    public void Login()
    {
        Audience.Context.Login(this.usernameInputField.text, this.passwordInputField.text);
    }

    public void Logout()
    {
        Audience.Context.Logout();
    }

    public void RefreshScene()
    {
        Audience.Context.RefreshSceneList();
    }

    public void LoadScene()
    {
        if (this.CachedSceneList != null && this.sceneListDropdown.options.Count > 0) {
            if (this.sceneListDropdown.value < this.CachedSceneList.Count)
            {
                Audience.Context.LoadScene(this.CachedSceneList[this.sceneListDropdown.value].SceneId);
            }
            else 
            {
                Debug.LogError("Invalid Call:Dropdown index out of range.");
            }
        }
        else
        {
            Debug.LogError("Invalid Call:scene list is empty.");
        }
    }

    public void UnloadScene()
    {
        Audience.Context.UnloadScene();
    }

    public void StartStream()
    {
        AudienceSDK.Audience.Context.StartSendCameraFrame();
        Audience.Context.Start();
    }

    public void StopStream()
    {
        Audience.Context.Stop();
        AudienceSDK.Audience.Context.StopSendCameraFrame();
    }

    // UI Control
    public void InitPanelView() {
        this.logInButton.interactable = true;
        this.logOutButton.interactable = false;

        this.sceneBlock.SetActive(false);
        this.sceneListDropdown.ClearOptions();
        this.refreshButton.interactable = false;
        this.loadButton.interactable = false;
        this.unloadButton.interactable = false;

        this.streamBlock.SetActive(false);
        this.startStreamButton.interactable = false;
        this.stopStreamButton.interactable = false;
    }

    public void DisplayLoginCompletedView() {
        this.logInButton.interactable = false;
        this.logOutButton.interactable = true;

        this.sceneBlock.SetActive(true);
        this.sceneListDropdown.ClearOptions();
        this.refreshButton.interactable = true;
        this.loadButton.interactable = false;
        this.unloadButton.interactable = false;

        this.streamBlock.SetActive(false);
        this.startStreamButton.interactable = false;
        this.stopStreamButton.interactable = false;
    }

    public void DisplayLogoutCompletedView()
    {
        this.logInButton.interactable = true;
        this.logOutButton.interactable = false;

        this.sceneBlock.SetActive(false);
        this.sceneListDropdown.ClearOptions();
        this.refreshButton.interactable = false;
        this.loadButton.interactable = false;
        this.unloadButton.interactable = false;

        this.streamBlock.SetActive(false);
        this.startStreamButton.interactable = false;
        this.stopStreamButton.interactable = false;
    }

    public void UpdateSceneList(List<NativeSceneSummaryData> sceneList) {
        this.CachedSceneList = sceneList;
        this.sceneListDropdown.ClearOptions();

        var sceneNameList = new List<string>();
        foreach (var sceneData in sceneList) {
            sceneNameList.Add(sceneData.SceneName);
        }

        this.sceneListDropdown.AddOptions(sceneNameList);

        this.loadButton.interactable = (this.sceneListDropdown.options.Count > 0);
    }

    public void DisplayUnloadCompletedView()
    {
        this.logInButton.interactable = false;
        this.logOutButton.interactable = true;

        this.sceneBlock.SetActive(true);
        this.refreshButton.interactable = true;
        this.loadButton.interactable = (this.sceneListDropdown.options.Count > 0);
        this.unloadButton.interactable = false;

        this.streamBlock.SetActive(false);
        this.startStreamButton.interactable = false;
        this.stopStreamButton.interactable = false;
    }

    public void DisplayLoadCompletedView()
    {
        this.logInButton.interactable = false;
        this.logOutButton.interactable = false;

        this.sceneBlock.SetActive(true);
        this.refreshButton.interactable = false;
        this.loadButton.interactable = false;
        this.unloadButton.interactable = true;

        this.streamBlock.SetActive(true);
        this.startStreamButton.interactable = true;
        this.stopStreamButton.interactable = false;
    }

    public void DisplayStartStreamCompletedView()
    {
        this.logInButton.interactable = false;
        this.logOutButton.interactable = false;

        this.sceneBlock.SetActive(true);
        this.refreshButton.interactable = false;
        this.loadButton.interactable = false;
        this.unloadButton.interactable = false;

        this.streamBlock.SetActive(true);
        this.startStreamButton.interactable = false;
        this.stopStreamButton.interactable = true;
    }

    public void OnSceneListValueChanged() {
        Debug.Log("OnSceneListValueChanged:" + this.sceneListDropdown.value.ToString());
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

}
