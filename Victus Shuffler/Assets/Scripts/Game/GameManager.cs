using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDificulty { Begginer, Advanced, Pro }

public class GameManager : MonoBehaviour
{
    [Space]
    [SerializeField] private GameDificulty dificulty;

    public GameDificulty Dificulty { get => dificulty; set => dificulty = value; }
}
