using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DriftGame.Systems.SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }
        
        [Header("File Storage Config")] 
        [SerializeField] private string _fileName;

        [SerializeField] private bool _useFileEncryption;
        
        private GameData _gameData;
        private List<IDataPersistence> _dataPersistences = new();
        private FileDataHandler _fileDataHandler;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            //DontDestroyOnLoad(gameObject);
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useFileEncryption);
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        private void NewGame()
        {
            _gameData = new GameData();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadGameData();
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            SaveGameData();
        }
        
        public void LoadGameData()
        {
            _dataPersistences = FindAllDataPersistences();
            _gameData = _fileDataHandler.Load();
            
            if (_gameData == null)
            {
                Debug.Log("There is no game data");
                return;
            }

            foreach (IDataPersistence dataPersistence in _dataPersistences)
            {
                dataPersistence.LoadData(_gameData);
            }
            
            Debug.Log("Loaded");
        }

        public bool HasGameData()
        {
            return _gameData != null;
        }
        
        public void SaveGameData()
        {
            if (_gameData == null)
            {
                return;
            }
            
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