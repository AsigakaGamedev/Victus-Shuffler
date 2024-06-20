using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneIndex);
        });
    }
}
