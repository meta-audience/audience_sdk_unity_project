
# How to start your first live stream
- [Sign in/Sign up on the audience website](#step1)
- [Add a scene and source camera](#step2)
- [Add viewers](#step3)
- [Save your settings](#step4)
- [Sign into audience in Beat Saber](#step5)
- [Start streaming](#step6)
- [Share a URL](#step7)
****
#### <a name="step1"> Step 1:  Sign in/Sign up on the audience website
- [Official Website](https://www.meta-audience.com/en-us/)
****
#### <a name="step2"> Step 2: Add a scene and source camera
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
#### <a name="step3"> Step 3: Add viewers
- A `viewer` represents a `channel`, which can be shared as a URL to your viewers later
- Click `Add Viewer` to add a new viewer 
- (***Recommended***) Select **`SFU`** viewer type, which supports **multiple online viewers**
- Link the viewer with a **`source camera`**
- You can add up to **`3`** viewers
- (***Recommended***) Rename your `Viewer Name` to make it easier to distinguish different viewers joining a channel through a URL
****
#### <a name="step4"> Step 4: Save your settings
- Click `Save` to save your scene settings
- Your scene settings will appear in the Beast Saber audience mod after you save
****
#### <a name="step5"> Step 5: SDK development
1. call Initialize() to initialize audience SDK
2. call Login() to login audience service
3. call RefreshSceneList() to get the latest scene list from audience webserver
4. call LoadScene() when you've select one of the scene you want to load
5. call Start() to start your selected scene
6. call GetUserChannelsPageURL() to get your live stream share page
7. After streaming, call Stop() to stop your live steram
8. call Logout() to logout audience service
9. call Dispose() to uninitialize audience SDK
****



