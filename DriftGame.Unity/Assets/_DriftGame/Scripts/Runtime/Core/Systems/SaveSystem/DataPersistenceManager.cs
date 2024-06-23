using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DriftGame.Systems.SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")] 
        [SerializeField] private string _fileName;

        [SerializeField] private bool _useFileEncryption;
        
        private GameData _gameData;
        private List<IDataPersistence> _dataPersistences = new();
        private FileDataHandler _fileDataHandler;

        private void Awake()
        {
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useFileEncryption);
            _dataPersistences = FindAllDataPersistences();
            LoadGameData();
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        private void NewGame()
        {
            _gameData = new GameData();
        }
        
        public void LoadGameData()
        {
            _gameData = _fileDataHandler.Load();
            
            if (_gameData == null)
            {
                Debug.Log("There is no game data");
                NewGame();
            }

            foreach (IDataPersistence dataPersistence in _dataPersistences)
            {
                dataPersistence.LoadData(_gameData);
            }
            
            Debug.Log("Loaded");
        }
        
        public void SaveGameData()
        {
            foreach (IDataPersistence dataPersistence in _dataPersistences)
            {
                dataPersistence.SaveData(ref _gameData);
            }
            
            _fileDataHandler.Save(_gameData);
        }

        private List<IDataPersistence> FindAllDataPersistences()
        {
            IEnumerable<IDataPersistence> dataPersistences =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            
            return new List<IDataPersistence>(dataPersistences);
        }
    }
}