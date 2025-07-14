using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAdController : MonoBehaviour
{
/*#if UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private const string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private const string _adUnitId = "unused";
#endif*/
#if UNITY_EDITOR && UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3784229013534169/1525609706";
#elif UNITY_IOS
    private const string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#elif UNITY_EDITOR
    private const string _adUnitId = "unused";
#else
    private const string _adUnitId = "unused";
#endif
    public static RewardAdController instance;
    private RewardedAd _rewardedAd;
    private Action _onRewardedCallback;

    private void Awake()
    {
        instance = this;
    }


    public void LoadAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        var adRequest = new AdRequest();
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }

            if (ad == null)
            {
                Debug.LogError("Rewarded ad is null after loading.");
                return;
            }

            _rewardedAd = ad;
            RegisterEventHandlers(ad);

            Debug.Log("Rewarded ad loaded successfully.");
        });
    }

    public void ShowAd(Action onRewarded)
    {
        
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _onRewardedCallback = onRewarded;
            _rewardedAd.Show((Reward reward) => // ВИКОНУЄТЬСЯ тільки при повному перегляді
            {
                Debug.Log($"Rewarded ad completed. Reward: {reward.Amount} {reward.Type}");
                Debug.Log(_onRewardedCallback == null);
                _onRewardedCallback?.Invoke();
                _onRewardedCallback = null;
                LoadAd();
            });
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Rewarded ad paid {adValue.Value / 1_000_000.0} {adValue.CurrencyCode}");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Ad closed.");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Failed to open ad: " + error);
            _onRewardedCallback = null;
            LoadAd(); // все одно пробуємо підгрузити нову
        };
    }
}

