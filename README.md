# audience SDK Unity Project

Bringing interactive 360 live streams to more viewers than ever before.
Allows VR content creators to stream high-quality, immersive video experiences to local and global viewers..

# website
 
 https://www.meta-audience.com
 
# Support Forum

[Visit our discord group](https://discord.gg/T2aKHMGbU2)

# Compatibility

Our unity project is tested with :  
* Unity 2019.4.28f1 (LTS)
* Unity 2021.3.15f1 (LTS)
* Unity 2021.3.18f1 (LTS)  
  
The minimum supported version is set to 2019.4.28   

# Installation

Our unity project is a template for you to try our features, so just clone it into your local and have fun!
Or you can **Export** package, then import in your own project. The steps are as follows:
1. Open your unity project.
2. Select `Asset\audience-unity-sdk` folder.
3. Open `Assets > Export Packages...`.
4. Uncheck `include dependencies` and exclude `Asset\audience-unity-sdk\Tests` folder.
5. Click `Export...`.

# Dependencies

## Unity Registry packages
There are 2 ways to install necessary packages
1. Via Unity Package Manager (UPM) window
2. Open `<project>/Packages/manifest.json`, then add package name and version in the list of dependencies.

### Newtonsoft.Json
* Name: com.unity.nuget.newtonsoft-json
* Version: 2.0.2 or higher 


# Usage

Follow the document to setup your audience configuration
* [How to develop your first live stream project](Docs/TUTORIAL-LiveStream.md)
* [How to send your first VR emoji](Docs/TUTORIAL-Emoji.md)

# Limitations and known issues

Our unity project is not compatible with URP or HDRP project.
