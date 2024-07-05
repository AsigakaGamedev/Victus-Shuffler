using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinManeger : MonoBehaviour
{
    public Action onMagazinUpd;

    public static MagazinManeger Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    public void UpdateMagazin()
    {
        onMagazinUpd?.Invoke();
    }
}
