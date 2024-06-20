using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RecordsManagerInstaller : MonoInstaller
{
    [SerializeField] private RecordsManager recordsManager;

    public override void InstallBindings()
    {
        Container.Bind<RecordsManager>().FromInstance(recordsManager);
    }
}
