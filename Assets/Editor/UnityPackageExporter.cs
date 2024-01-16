using System;
using System.Linq;
using UnityEditor;
namespace Unity_CICD{
    public class UnityPackageExporter
    {
        public static void ExportPackage()
        {
            Console.WriteLine("ExportPackage method started.");
            string relativePackageSourcePath = "Assets/audience-unity-sdk"; // Relative path
            string packageExportPath = Environment.GetEnvironmentVariable("BUILD_PATH") ?? "D:/Build/";
            string ciProjectName = Environment.GetEnvironmentVariable("CI_PROJECT_NAME") ?? "AudienceUnitySDK";
            string packageName = ciProjectName + ".unitypackage";
            string packageOption = Environment.GetEnvironmentVariable("PACKAGE_OPTIOIN") ?? "Recurse";
            // Get all asset paths from the source folder
            string[] assetPaths = AssetDatabase.GetAllAssetPaths()
                .Where(path => path.StartsWith(relativePackageSourcePath))
                .ToArray();

            if (assetPaths.Length == 0)
            {
                Console.WriteLine($"Error: No assets found in path {relativePackageSourcePath}");
                EditorApplication.Exit(1);
            }

            try
            {
                // Export the package without dependencies
                AssetDatabase.ExportPackage(assetPaths, $"{packageExportPath}/{packageName}", ParseExportOption(packageOption));
                Console.WriteLine($"Package exported to {packageExportPath}/{packageName} without dependencies.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during package export: {ex.Message}");
                EditorApplication.Exit(1);
            }

            EditorApplication.Exit(0);
        }
        private static ExportPackageOptions ParseExportOption(string exportOption)
        {
            switch (exportOption)
            {
                case "Default":
                    return ExportPackageOptions.Default;
                case "Interactive":
                    return ExportPackageOptions.Interactive;
                case "Recurse":
                    return ExportPackageOptions.Recurse;
                case "IncludeDependencies":
                    return ExportPackageOptions.IncludeDependencies;
                case "IncludeLibraryAssets":
                    return ExportPackageOptions.IncludeLibraryAssets;
                default:
                    Console.WriteLine($"Warning: Unknown export option '{exportOption}'. Using Default.");
                    return ExportPackageOptions.Default;
            }
        }
    }
}
