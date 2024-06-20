using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfeysManager : MonoBehaviour
{
    public int StartScreen = 1;

    public InterfaysScreen[] allScreens;
    private InterfaysScreen curScreen;

    private void Start()
    {
        foreach (var screen in allScreens)
        {
            screen.gameObject.SetActive(false);
        }

        Change(StartScreen);
    }

    public void Change(int index)
    {
        if (curScreen)
        {
            curScreen.gameObject.SetActive(false);
        }

        curScreen = allScreens[index];
        curScreen.gameObject.SetActive(true);
    }
}
