using System;
using System.Linq;
using UnityEditor;
namespace Unity_CICD{
    public class UnityCommonBuildScript
    {
        public static void BuildProject()
        {
            // Read environment variables
            string buildTargetEnv = Environment.GetEnvironmentVariable("BUILD_TARGET") ?? "StandaloneWindows64";
            string scenesEnv = Environment.GetEnvironmentVariable("SCENES") ?? "Assets/audience-unity-sdk/Samples/Scenes/SampleScene.1-Start streaming.unity";
            string buildPath = Environment.GetEnvironmentVariable("BUILD_PATH") ?? "Build/YourGame.exe";
            Console.WriteLine("ExportPackage method started.");
            // Parse environment variables
            BuildTarget buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTargetEnv);
            string[] scenes = scenesEnv.Split(';').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();

            // Build player
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath + "/audience_unity_sample_scene.exe",
                target = buildTarget,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            Console.WriteLine("buld_path:" + buildPlayerOptions.locationPathName);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                throw new Exception("Build failed");
            }
            EditorApplication.Exit(report.summary.totalErrors > 0 ? 1 : 0);
        }
    }
}
