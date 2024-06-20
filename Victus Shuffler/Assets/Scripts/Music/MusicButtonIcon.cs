using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonIcon : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite offIcon;
    [SerializeField] private Sprite onIcon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject musicObj;

    private void Start()
    {
        int musicIsOn = PlayerPrefs.GetInt("musicIsOn", 1);
        musicObj.SetActive(musicIsOn == 1);

        if (musicIsOn == 0)
        {
            img.sprite = offIcon;
        }
        else
        {
            img.sprite = onIcon;
        }

        button.onClick.AddListener(() =>
        {
            int musicIsOn = PlayerPrefs.GetInt("musicIsOn", 1);

            if (musicIsOn == 0)
            {
                img.sprite = onIcon;
                PlayerPrefs.SetInt("musicIsOn", 1);
            }
            else
            {
                img.sprite = offIcon;
                PlayerPrefs.SetInt("musicIsOn", 0);
            }

            musicObj.SetActive(PlayerPrefs.GetInt("musicIsOn", 1) == 1);
        });
    }
}
