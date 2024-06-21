using UnityEngine;
using Zenject;

namespace DriftGame
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public void Run()
        {
            Debug.Log("Gameplay Scene Loaded");
        }
    }
}