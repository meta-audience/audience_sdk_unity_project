# How to start your first live stream
- [Sign in/Sign up on the audience website](#step1)
- [Add a scene and source camera](#step2)
- [Add viewers](#step3)
- [Save your settings](#step4)
- [Choose a Sample scene in Unity Editor](#step5)
- [Load a scene](#step6)
- [Start your streaming](#step7)
- [Share your URL](#step8)

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
- (***Recommended***) Rename your `Viewer Name` making it easier to distinguish different viewers joining a channel through a URL
****  
 #### <a name="step4"> Step 4 (Web): Save your settings
- Click `Save` to save your scene settings on `official website`
- Your scene settings will appear in the list after you `Log in` successfully in `Unity`
**** 
#### <a name="step5"> Step 5 (Unity): Choose a Sample scene in Unity Editor
- Choose the scene in `Assets/Samples/Scenes/SampleScene.1-Start`
- Enter your username (Email) and password
- Once logged in, the scene setting panel will appear
    - If scene setting UI panel doesn't appear, check your Email and password are correct
    - If your dropdown menu is empty, please click the `Refresh` button
    - If it is still empty, please check your scene setting again 
****
#### <a name="step6"> Step 6 (Unity): Load a scene 
- Make sure your audio device and microphone are available
    - If you have a VR device, audio and microphone are available by default
    -Error may occur if plugging your audio device after starting Unity. In such cases, do the following:
      - Close Unity
      - Plug your audio device and microphone
      - Start Unity  
- After choose a scene setting in dropdown menu, click `Load` button
    - If nothing happened, check the `Console`, and check the error code and message
**** 
#### <a name="step7"> Step 7 (Unity): Start your streaming
- After `Console` show `connect success` message, click `Start` button
- Approximately 3 seconds later, a `green light` will appears, indicating that the streaming has started
    - The color of sphere represents the status of streaming
    - `Yellow` means waiting for streaming
    - `Gray` means no streaming
    - `Red` means some error occured, please check `Console` or `log`
****
#### <a name="step8"> Step 8 (Web): Share your streaming
- After streaming start,you can find a URL on our [official website channel list](https://www.meta-audience.com/en-us/accounts/userChannels/), you can share the URL to other device (Mobile / Oculus / Other PC)
> If any error occurs, please check [Error code detail](../../wiki/Initialization-&-Deinitialization#error-code--message-3)
