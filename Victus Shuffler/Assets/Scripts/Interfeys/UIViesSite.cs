using Gpm.WebView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViesSite : MonoBehaviour
{
    [SerializeField] private SitePokazat pokazat;
    [SerializeField] private Button button;
    [SerializeField] private string url;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            pokazat.ShowUrlPopupMargins(url);
        });
    }
}
