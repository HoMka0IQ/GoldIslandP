using GoogleMobileAds.Api;
using UnityEngine;

public class AdsInitializer : MonoBehaviour
{
    private static bool _initialized = false;


    public BannerController bannerController;
    public RewardAdController rewardAdController;
    void Awake()
    {

        Debug.Log("Initializing AdMob SDK...");
        ConfigureRequestByAge();
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob SDK initialized.");
            bannerController.LoadAd();
            rewardAdController.LoadAd();
        });

    }
    public void ConfigureRequestByAge()
    {
        int playerAge = PlayerPrefs.GetInt("AgeForAds", -1);
        Debug.Log("Player age: " + playerAge);
        var requestConfig = new RequestConfiguration();


        if (playerAge >= 0 && playerAge < 13)
        {
            requestConfig.TagForChildDirectedTreatment = TagForChildDirectedTreatment.True;
            requestConfig.MaxAdContentRating = MaxAdContentRating.G;
        }
        else if (playerAge < 16)
        {
            requestConfig.TagForChildDirectedTreatment = TagForChildDirectedTreatment.False;
            requestConfig.TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.True;
            requestConfig.MaxAdContentRating = MaxAdContentRating.PG;
        }
        else if (playerAge < 18)
        {
            requestConfig.TagForChildDirectedTreatment = TagForChildDirectedTreatment.False;
            requestConfig.TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.False;
            requestConfig.MaxAdContentRating = MaxAdContentRating.T;
        }
        else if (playerAge >= 18)
        {
            requestConfig.TagForChildDirectedTreatment = TagForChildDirectedTreatment.False;
            requestConfig.TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.False;
            requestConfig.MaxAdContentRating = MaxAdContentRating.MA;
        }

        MobileAds.SetRequestConfiguration(requestConfig);
    }
}
