#define IGNORE_ADS

#if !IGNORE_ADS && (UNITY_ANDROID || UNITY_IOS) 
using UnityEngine;
using UnityEngine.Advertisements;
#endif

using UnityEngine.Events;

public class IDLEAds
{
    private static IDLEAds instance = null;

    public static string GameId = "";
    public static string placementId = "rewardedVideo";

    public static UnityAction DoAfterPassingAd;
    public static UnityAction DoAfterFailingAd;

    public static void InitializeAds(string gameId)
    {
        if(instance == null)
        {
            GameId = gameId;
            instance = new IDLEAds(GameId);
        }
    }

#if !IGNORE_ADS && (UNITY_ANDROID || UNITY_IOS)
    public IDLEAds(string GameId = "")
    {
        if (string.IsNullOrEmpty(GameId))
        {
            Debug.LogError("Advertisement Failed to load due to empty GameId");
            return;
        }

        Advertisement.Initialize(GameId, false);
        Debug.Log($"Initializing Unity Advertisements with GameId: {GameId}");
    }

    public static bool IsAdReady() { return Advertisement.IsReady(placementId); }

    public static void ShowAds(UnityAction onPass = null, UnityAction onFail = null)
    {
        instance.ShowAd(onPass, onFail);
    }

    private void ShowAd(UnityAction onPass = null, UnityAction onFail = null)
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (onPass != null)
        {
            DoAfterPassingAd = onPass;
        }

        if (onFail != null)
        {
            DoAfterFailingAd = onFail;
        }

        Advertisement.Show(placementId, options);
    }

    private void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            // Give player reward for playing ads.
            DoAfterPassingAd?.Invoke();
            DoAfterPassingAd = null;
        }
        else if (result == ShowResult.Skipped || result == ShowResult.Failed)
        {
            // Give player NO reward for playing ads.
            DoAfterFailingAd?.Invoke();
            DoAfterFailingAd = null;
        }
    }
#else
    public IDLEAds(string GameId = "") { }
    public static bool IsAdReady() { return false; }
    public static void ShowAds(UnityAction onPass = null, UnityAction onFail = null) { }
#endif

}
