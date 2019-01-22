
using UnityEngine;

#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif

public class IDLEAds
{
    public delegate void AfterAdAction();
    public AfterAdAction DoAfterPassingAd;
    public AfterAdAction DoAfterFailingAd;

    public bool IsAdReady()
    {
#if UNITY_ANDROID || UNITY_IOS
        return Advertisement.IsReady(placementId);
#else
        return true;
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    private string placementId = "rewardedVideo";
#endif

    public void ShowAd(AfterAdAction onPass = null, AfterAdAction onFail = null)
    {
#if UNITY_ANDROID || UNITY_IOS
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
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    void HandleShowResult(ShowResult result)
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
#endif

}
