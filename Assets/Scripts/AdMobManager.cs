using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{

    public bool isTestMode;
    private bool isShow = true;
    public Text LogText;

    const string BANNER_TEST_ID = "ca-app-pub-3940256099942544/6300978111";
    const string BANNER_ID = "ca-app-pub-4666485356261186/3680118964";
    BannerView bannerAd;

    public void Start()
    {
        var requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(new List<string>() { "701C6A44E143E895" }) 
            .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);


        LoadBannerAd();
    }

    void LoadBannerAd()
    {
        bannerAd = new BannerView(isTestMode ? BANNER_TEST_ID : BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(GetAdRequest());
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
}
