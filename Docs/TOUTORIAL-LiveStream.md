# How to start your first live stream
- [Sign in/Sign up on the audience website](#step1)
- [Add a scene and source camera](#step2)
- [Add viewers](#step3)
- [Save your settings](#step4)
- [Choose a Sample scene in Unity Editor](#step5)
- [Load a scene ](#step6)
- [Start your streaming](#step7)
- [Share a URL](#step8)

****
#### <a name="step1"> Step 1 (Web): Sign in/Sign up on the audience website
- [Official Website](https://www.meta-audience.com/en-us/)
****
  
#### <a name="step2"> Step 2 (Web): Add a scene and source camera
- Click [`here`](https://www.meta-audience.com/en-us/accounts/scenes/) to edit your scene settings
- Click `+ Add` to add a new scene 
    - You can add up to **`20`** scenes
- Click `Add Source` to add a source camera
    - (Optional) Rename the source camera name
    - Select 2D or 3D capture mode
    - Set your resolution
    - Position and rotation can be modified in Unity
    - You can add up to **`3`** cameras
> You must add at least **`1`** source camera

****
#### <a name="step3"> Step 3 (Web): Add viewers
- A `viewer` represents a `channel`, which can be shared as a URL to your viewers later
- Click `Add Viewer` to add a new viewer 
- (***Recommended***) Select **`SFU`** viewer type, which supports **multiple online viewers**
- Link the viewer with a **`source camera`**
- You can add up to **`3`** viewers
- (***Recommended***) Rename your `Viewer Name` to make it easier to distinguish different viewers joining a channel through a URL
****  
 #### <a name="step4"> Step 4 (Web): Save your settings
- Click `Save` to save your scene settings on `official website`
- Your scene settings will appear in the list after you `Log in` success in `Unity`
**** 
#### <a name="step5"> Step 5 (Unity): Choose a Sample scene in Unity Editor
- Choose the scene in `Assets/Samples/Scenes/SampleScene.1-Start a stream`
- Enter your Username(Email) and password
- After login success, the scene setting UI panel should appear 
    - If scene setting UI panel doesnt appear, check your Email and password is correct
    - If your dropdown list is empty, please click the `Refresh` button
    - After click refresh button but still empty, please check your scene setting again 
****
#### <a name="step6"> Step 6 (Unity): Load a scene 
- Check your audio device and microphone is available
    - If you have already have VR device, the audio and microphone should avilable by default
    - It maybe cause some ***`error`*** if you alerady open an Unity scene, then plug the headphone or microphone, please restart the Unity
        - Please plug the headphone or microphone first and then start the Unity    
- After choose a scene setting in dropdown list, click `Load` button
    - If nothing happend check the `Console`, and check the error code and message
**** 
#### <a name="step7"> Step 7 (Unity): Start your streaming
- After `Console` show connect success message, click `Start` button
- Wait for about 3 seconds, you will see a `Green Ball` in the scene, the streaming is started
    - The color of ball represent the status of streaming
    - `Yellow` is waiting for streaming
    - `Gray` means no streaming
    - `Red` means some error occured, please check `Console` or `log`
****
#### <a name="step8"> Step 8 (Web): Share your streaming
- After streaming start, the URL will create in our [official website](https://www.meta-audience.com/en-us/accounts/userChannels/), you can share the URL to other device (Mobile / Occulus / Other PC)
> If any error occur, please check the [Error code detail](https://adc.github.trendmicro.com/Consumer-TMXRLAB/audience_sdk_unity_project/wiki/Initialization-&-Deinitialization#error-code--message-3)
