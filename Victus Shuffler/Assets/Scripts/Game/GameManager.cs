using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDificulty { Begginer, Advanced, Pro }

public class GameManager : MonoBehaviour
{
    [Space]
    [SerializeField] private GameDificulty dificulty;
    [SerializeField] private bool withShuffle;

    public GameDificulty Dificulty { get => dificulty; set => dificulty = value; }
    public bool WithShuffle { get => withShuffle; set => withShuffle = value; }
}
