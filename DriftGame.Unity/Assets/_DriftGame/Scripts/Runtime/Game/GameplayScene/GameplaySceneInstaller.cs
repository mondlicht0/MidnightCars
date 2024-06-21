using UnityEngine;
using Zenject;

namespace DriftGame
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public void Run(UIRoot uiRoot)
        {
            Debug.Log("Gameplay Scene Loaded");
        }
    }
}