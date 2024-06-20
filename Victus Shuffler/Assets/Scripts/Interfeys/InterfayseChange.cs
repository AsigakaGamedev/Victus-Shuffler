using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfayseChange : MonoBehaviour
{
    public Button knopka;
    public int nextIndex;

    private void Start()
    {
        knopka.onClick.AddListener(() =>
        {
            InterfeysManager interfeysManager = FindAnyObjectByType<InterfeysManager>();
            interfeysManager.Change(nextIndex);
        });
    }
}
