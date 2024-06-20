using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMusicChangeTetx : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private GameObject musicObj;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            int musicIsOn = PlayerPrefs.GetInt("musicIsOn", 1);

            if (musicIsOn == 0)
            {
                text.text = "Music: <color=green>ON</color>";
                PlayerPrefs.SetInt("musicIsOn", 1);
            }
            else
            {
                text.text = "Music: <color=red>OFF</color>";
                PlayerPrefs.SetInt("musicIsOn", 0);
            }
        });
    }
}
