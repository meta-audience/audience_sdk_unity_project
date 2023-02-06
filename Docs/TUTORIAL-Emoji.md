# How to send your first VR emoji
- [Check TUTORIAL step 1~4](#step1)
- [Authorize YouTube/Twitch channels](#step2)
- [Start the streaming on `YouTube/Twitch`](#step3)
- [Verify your streaming Emoji is working](#step4)



****
#### <a name="step1"> Step 1 (Web): Check [TUTORIAL](TOUTORIAL-LiveStream.md) step 1~4
****
#### <a name="step2"> Step 2 (Web): Authorize YouTube/Twitch channels
- Click [`here`](https://www.meta-audience.com/en-us/accounts/chatSetting/) to connect your YouTube/Twitch channel
- To connect YouTube:
    - **First make sure your YouTube account is capable of streaming**
    - Click `Connect YouTube Account`
    - Choose your YouTube account
    - Choose a channel (make sure your channel is **not** private or locked with YouTube Kids)
- To connect Twitch:
    - Click `Connect Twitch Account`
    - If you're already signed into Twitch, audience automatically authorizes the account
    - If your browser does not have a record of Twitch, the sign in window will pop out
    - To connect a different Twitch account, first sign out of Twitch 

****
<a name="step3"> Step 3 (Unity): Start the streaming on `YouTube/Twitch`
- Choose the scene in `Assets/Samples/Scenes/SampleScene.5-Connect YouTube and Twitch`
- Enter your username (Email) and password to login 
- `Load scene setting` this step is for audience (our website) streaming, in the case of streaming `only` on `YouTube/Twitch` , ***`skip this step`***
- YouTube
  - ***Default behavior is streaming on the first channel created in your YouTube account***
  - Click `Connect YouTube` checkbox
- Twitch
  - Click `Connect Twitch` checkbox
- You can go to the account that you binded in Step 2 to see the streaming

****
<a name="step4"> Step 4: Verify your streaming Emoji is working
- Sign in to your audience account and make sure you've completed Step 2 to authorize your YouTube or Twitch account
- We support live streaming on three platforms to streaming your games: `YouTube`, `Twitch` and our `official website channel`
- audience (official website)
  - The channel is in our  [official website](https://www.meta-audience.com/en-us/accounts/userChannels/), you can share the URL to other device(Mobile / Oculus / Other PC)
  - Emoji panel can send the 3D-emoji
  - As soon as viewers send emojis, the 3D emojis, along with avatar and username, will appear in your Unity scene
- YouTube/Twitch
  - Support 2D and 3D emoji
  - Emojis in chat messages are auto-detected
  - As soon as viewers send emojis, the 2D/3D emojis, along with avatar and username, will appear in your Unity scene
  - **Try 3D emoji** on YouTube/Twitch
    - We have designed some [`3D emoji`](https://www.meta-audience.com/en-us/download/). Try entering the emoji command (e.g. #bigheart# ) in the chat to show our 3D emoji.

