
# How to start your first live stream
- [Sign in/Sign up on the audience website](#step1)
- [Authorize YouTube/Twitch channels](#step2)
- [Add a scene and source camera](#step3)
- [Add viewers](#step4)
- [Save your settings](#step5)
- [Choose a Sample scene in Unity Editor](#step6)
- [Load a scene ](#step7)
- [Start your streaming](#step8)
- [Verify your audience/Youtube/Twitch streaming chat and Emoji is working](#step9)
- [Share a URL](#step9)
****
#### <a name="step1"> Step 1:  Sign in/Sign up on the audience website
- [Official Website](https://www.meta-audience.com/en-us/)
****

#### <a name="step2"> Step 2: Authorize YouTube/Twitch channels
- Click [`here`](https://www.meta-audience.com/en-us/accounts/chatSetting/) to connect your YouTube/Twitch channel
- To connect YouTube:
    - **First make sure your YouTube account is capable of streaming**
    - Click `Connect YouTube Account`
    - Choose your YouTube account
    - Choose a channel (make sure your channel is **not** private or locked with YouTube Kids)
- To connect Twitch:
    - Click `Connect Twitch Account`
    - If you're already signed into Twitch, audience automatically authorizes the account
    - If not signed in, the Twitch sign in window appears
    - To connect a different Twitch account, first sign out of Twitch 

****
#### <a name="step3"> Step 3: Add a scene and source camera
- Click [`here`](https://www.meta-audience.com/en-us/accounts/scenes/) to edit your scene settings
- Click `+ Add` to add a new scene 
    - You can add up to **`20`** scenes
- Click `Add Source` to add a source camera
    - (Optional) Rename the source camera name
    - Select 2D or 3D capture mode
    - Set your resolution
    - Position and rotation can be modified in BISPA (used for Beat Saber audience mods)
    - You can add up to **`3`** cameras
> You must add at least **`1`** source camera
****
#### <a name="step4"> Step 4: Add viewers
- A `viewer` represents a `channel`, which can be shared as a URL to your viewers later
- Click `Add Viewer` to add a new viewer 
- (***Recommended***) Select **`SFU`** viewer type, which supports **multiple online viewers**
- Link the viewer with a **`source camera`**
- You can add up to **`3`** viewers
- (***Recommended***) Rename your `Viewer Name` to make it easier to distinguish different viewers joining a channel through a URL
****
#### <a name="step5"> Step 5: Save your settings
- Click `Save` to save your scene settings on `official website`
- Your scene settings will appear in the list after you `Log in` success in `Unity`

****
#### <a name="step6"> Step 6: Choose a Sample scene in Unity Editor
- Choose a scene in `Assets/Samples/Scenes`
- Enter your Username(Email) and password
- After login success, the scene setting UI panel should appear 
    - If scene setting UI panel doesnt appear, check your Email and password is correct
    - If your dropdown list is empty, please click the `Refresh` button
    - After click refresh button but still empty, please check your scene setting again  
****    
#### <a name="step7"> Step 7: Load a scene 
- Check your audio device and microphone is available
    - If you have already have VR device, the audio and microphone should avilable by default
    - It maybe cause some ***`error`*** if you alerady open an Unity scene, then plug the headphone or microphone, please restart the Unity
        - Please plug the headphone or microphone first and then start the Unity    
- After choose a scene setting in dropdown list, click `Load` button
    - If nothing happend check the `Console`, and check the error code and message
    - Error code detail : [url need update] 
****
#### <a name="step8"> Step 8: Start your streaming
- After `Console` show connect success message, click `Start` button
- Wait for about 3 seconds, you will see a `Green Ball` in the scene, the streaming is started
    - The color of ball represent the status of streaming
    - `Yellow` is waiting ????
    - `Gray` means no streaming
    - `Red` means some error occured, please check `Console` or `log`
- After streaming start, the URL is in our [official website](https://www.meta-audience.com/en-us/accounts/userChannels/), you can share the URL to other device(Mobile / Occulus / Other PC)
****
#### <a name="step9"> Step 9: Verify your audience/Youtube/Twitch streaming chat and Emoji is working
- Sign into your audience account and make sure you've completed `Step 2` to authorize your YouTube or Twitch account
- 
- You can send emoji from our [official website channel](https://www.meta-audience.com/en-us/accounts/userChannels/)/Youtube chat/Twitch chat
    - Our website channel view has chat panel, click it to send a 3D emoji
    - Youtube/Twitch chat will support 2D and 3D emoji
        - **Try 3D emoji** in Youtube/Twitch
            - We offer some [`3D emoji`](https://www.meta-audience.com/en-us/download/). To use 3D emoji, type the emoji command in the chat (Ex: #bigheart#)
- The emoji (with username with avatar) should show in your Unity scene
****

