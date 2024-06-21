using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private InputHandler _inputHandler;
    
    public override void InstallBindings()
    {
        GameObject inputHandler = Container.InstantiatePrefab(_inputHandler);
        Container.Bind<InputHandler>().FromInstance(inputHandler.GetComponent<InputHandler>()).AsSingle().NonLazy();
    }
}
