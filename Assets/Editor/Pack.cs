using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Pack : MonoBehaviour
{
    [MenuItem("ABTool/Build")]
    public static void BuildAllAB() {
        string ABOutPathDir = string.Empty;
        ABOutPathDir = Application.streamingAssetsPath;

        if (!Directory.Exists(ABOutPathDir)) {
            Directory.CreateDirectory(ABOutPathDir);
        }
        BuildPipeline.BuildAssetBundles(ABOutPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
