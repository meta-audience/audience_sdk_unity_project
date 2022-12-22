using System.Collections;
using System.Collections.Generic;
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

    }

    public void Logout()
    {
    }

    public void RefreshScene()
    {
    }

    public void LoadScene()
    {
    }

    public void UnloadScene()
    {
    }

    public void StartStream()
    {
    }

    public void StopStream()
    {
    }

    public void UpdateSceneList() {

    }

    // UI Control
    public void ShowLoginCompletedView() {
        this.sceneBlock.SetActive(true);
        this.logInButton.interactable = false;
        this.logOutButton.interactable = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
