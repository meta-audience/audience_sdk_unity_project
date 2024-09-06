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

Our Unity project is a template designed to showcase our SDK's features. You can either clone the repository to explore the features or export the necessary components and import them into your own project. Follow these steps to export the SDK package:

1. Open the Unity project, `sdk-unity-project`.
2. In the Project window, select the `Assets\audience-unity-sdk` folder.
3. From the top menu, navigate to `Assets > Export Package...`.
4. In the Export Package dialog, uncheck `Include Dependencies` and ensure you exclude the `Assets\audience-unity-sdk\Tests` folder.
5. Click `Export...` and save the package.
6. Import the exported package into your target Unity project via `Assets > Import Package > Custom Package...`.

# Dependencies

## Unity Registry packages
To use the SDK, you need to install the necessary dependencies via the Unity Package Manager (UPM). Here's how to install the required `Newtonsoft.Json` package:

### Installing Newtonsoft.Json
1. In your Unity project, open the Package Manager by going to Window > Package Manager.
2. In the Package Manager window, click the "+" button in the top-left corner and select "Add package by name...".
3. In the dialog box, enter the following: `com.unity.nuget.newtonsoft-json`
4. Please select a version higher than `2.0.2` and click "Add" to install the package.

By following these steps, you ensure the SDK's dependencies are properly installed in your project without manual edits to the manifest.json file.

# Usage

Follow the document to setup your audience configuration
* [How to develop your first live stream project](Docs/TUTORIAL-LiveStream.md)
* [How to send your first VR emoji](Docs/TUTORIAL-Emoji.md)

# Limitations and known issues

Our unity project is not compatible with URP or HDRP project.
