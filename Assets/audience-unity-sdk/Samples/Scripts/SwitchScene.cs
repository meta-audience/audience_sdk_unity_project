using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene("SampleScene.1-Start streaming");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("SampleScene.2-Camera Follow");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("SampleScene.3-Switch Camera Mode");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("SampleScene.4-Emoji Follow");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene.5-Connect YouTube and Twitch");
        }
    }
}
