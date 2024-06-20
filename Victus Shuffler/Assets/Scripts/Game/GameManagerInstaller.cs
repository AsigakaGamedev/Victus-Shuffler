using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManagerInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;   

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(gameManager);
    }
}
