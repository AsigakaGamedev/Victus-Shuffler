using Gpm.WebView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SitePokazat : MonoBehaviour
{
    public void ShowUrlPopupMargins(string url)
    {
        GpmWebView.ShowUrl(
            url,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                margins = new GpmWebViewRequest.Margins
                {
                    hasValue = true,
                    left = (int)(Screen.width * 0.1f),
                    top = (int)(Screen.height * 0.1f),
                    right = (int)(Screen.width * 0.1f),
                    bottom = (int)(Screen.height * 0.1f)
                },
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            }, null, null);
    }
}
