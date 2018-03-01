using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class MobileAdManager : MonoBehaviour {

	private BannerView bannerView;

	public void Start()
	{
 //        #if UNITY_ANDROID
	// 	string appId = "ca-app-pub-5378309952892753~9394229167";
 //        #elif UNITY_IPHONE
	// 	string appId = "ca-app-pub-3940256099942544~1458002511";
 //        #else
	// 	string appId = "unexpected_platform";
 //        #endif

 //        // Initialize the Google Mobile Ads SDK.

	//	showBannerAd(); 

	}

	private void showBannerAd()
	{
		string adID = "ca-app-pub-5378309952892753~9394229167";

		MobileAds.Initialize(adID);
        //***For Testing in the Device***
		AdRequest request = new AdRequest.Builder()
       .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
       .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")  // My test device.
       .Build();

        //***For Production When Submit App***
        //AdRequest request = new AdRequest.Builder().Build();

       BannerView bannerAd = new BannerView(adID, AdSize.SmartBanner, AdPosition.Top);
       bannerAd.LoadAd(request);
   }

}
