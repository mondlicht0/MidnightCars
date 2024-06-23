using UnityEngine;

namespace DriftGame.Systems.SaveSystem
{
    [System.Serializable]

    public class GameData
    {
        public Color Color;
        public int EngineLevel;
        public bool HasSpoiler;
        public int Cash;
        public int Gold;
        public int BestScore;

        public GameData()
        {
            Cash = 0;
            Gold = 0;
            EngineLevel = 0;
            BestScore = 0;
            Color = Color.white;
            HasSpoiler = false;
        }
    }
}