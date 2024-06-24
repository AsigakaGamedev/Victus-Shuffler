using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinLoader : MonoBehaviour
{
    [SerializeField] private string equipID = "equip_location";
    [SerializeField] private Sprite[] skins;
    [SerializeField] private Image image;

    private void Start()
    {
        int equipIndex = PlayerPrefs.GetInt(equipID, 0);

        image.sprite = skins[equipIndex];   
    }
}
