using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class AssetBundleLoader
{
    // purpose is to handle the logic of loading an asset bundle from web... using the stuff in ResourceDefinitions but abstract enough to be shared
    //  between Resource and Planet Definitions.

    private static List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();
    public static IList<AssetBundle> LoadedAssetBundles
    {
        get
        {
            return loadedAssetBundles;
        }
    }

    private static string bundlePath = "";
    public static string BundlePath
    {
        get
        {
            if (bundlePath == "")
            {

#if UNITY_EDITOR
                bundlePath = string.Concat("file://", Application.streamingAssetsPath, "/", AssetBundles.Utility.GetPlatformName(), "/");
#elif UNITY_ANDROID
                bundlePath = string.Concat("jar:file://", Application.dataPath, "!/assets/", AssetBundles.Utility.GetPlatformName(), "/");
#elif UNITY_IOS
                bundlePath = string.Concat("file://", Application.dataPath, "/Raw/", AssetBundles.Utility.GetPlatformName(), "/");
#endif
            }

            return bundlePath;
        }
    }

    public static AssetBundle SingleBundle { get; private set; }

    public static IEnumerator LoadSingleBundle(string name)
    {
        // if single bundle is already loaded, do nothing.
        //if (SingleBundle != null && SingleBundle.name == name) yield break;
        if(SingleBundle == null)
        {
            SingleBundle = null;

            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(BundlePath + name);

            yield return request.SendWebRequest();

            SingleBundle = DownloadHandlerAssetBundle.GetContent(request);
        }
    }

    public static IEnumerator LoadAssetBundles(List<string> bundleNames, string ActiveFaction)
    {
        ActiveFaction = ActiveFaction.ToLower();

        List<string> alreadyLoaded = loadedAssetBundles.ConvertAll(ab => ab.name);
        bundleNames = bundleNames.Except(alreadyLoaded).ToList();

        foreach (string bn in bundleNames)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(BundlePath + bn);

            yield return request.SendWebRequest();

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            if (bundle != null)
            {
                loadedAssetBundles.Add(bundle);
            }
        }
    }

}
