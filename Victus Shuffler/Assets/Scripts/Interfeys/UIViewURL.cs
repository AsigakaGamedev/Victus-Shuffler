using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViewURL : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button button;
    [SerializeField] private Button closeBtn;
    [SerializeField] private string url;

    private void Start()
    {
        canvas.gameObject.SetActive(false);

        button.onClick.AddListener(() =>
        {
            closeBtn.gameObject.SetActive(true);
            canvas.gameObject.SetActive(true);
            WebViewObject.Instance.SetVisibility(true);
            WebViewObject.Instance.LoadURL(url);
        });

        closeBtn.onClick.AddListener(() =>
        {
            canvas.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(false);
            WebViewObject.Instance.SetVisibility(false);
        });
    }
}
