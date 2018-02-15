using UnityEngine;
using UnityEditor;

public class ExportAssetBundles
{
    [MenuItem("Exporter/Build AssetBundle For Android with Dependencies")]
    static void ExportResourceAndroidwith()
    {
        string path = "Assets/AssetBundles";
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
    }

    [MenuItem("Exporter/Build AssetBundle For Android without Dependencies")]
    static void ExportResourceAndroidwithout()
    {
        string path = "Assets/AssetBundles";
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("Exporter/Build AssetBundle For IOS")]
    static void ExportResourceIOS()
    {
        string path = "Assets/AssetBundles";
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iOS);
    }

    /* [MenuItem("Assets/BuildSceneBundleforAndroid")]
     static void ExportSceneAndroid()
     {
         Object SelectedScene = Selection.activeObject;
         string[] levels = new string[1];
         levels[0] = AssetDatabase.GetAssetOrScenePath(SelectedScene);
         string path = "Assets/SceneBundles/";
         BuildPipeline.BuildStreamedSceneAssetBundle(levels, path + SelectedScene.name + ".unity3d", BuildTarget.Android);
     }*/
}