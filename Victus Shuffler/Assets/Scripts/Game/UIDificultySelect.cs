using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIDificultySelect : MonoBehaviour
{
    [SerializeField] private GameDificulty dificulty;
    [SerializeField] private Button button;

    private GameManager gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            gameManager.Dificulty = dificulty;
            SceneManager.LoadScene(1);
        });
    }
}
