using System;
using System.IO;
using UnityEngine;

namespace DriftGame.Systems.SaveSystem
{
    public class FileDataHandler
    {
        private string _dataDirectionPath;
        private string _dataFileName;
        private bool _useXOREncryption;
        private readonly string _encryptionCodeWord = "midnight";

        public FileDataHandler(string dataDirectionPath, string dataFileName, bool useEncryption)
        {
            _dataDirectionPath = dataDirectionPath;
            _dataFileName = dataFileName;
            _useXOREncryption = useEncryption;
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(_dataDirectionPath, _dataFileName);
            GameData loadedData = null;
            
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    
                    if (_useXOREncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }

                catch (Exception e)
                {
                    Debug.Log("Error occured when trying to LOAD data " + fullPath);
                }
            }

            return loadedData;
        }

        public void Save(GameData data)
        {
            string fullPath = Path.Combine(_dataDirectionPath, _dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useXOREncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error occured when trying to SAVE data " + fullPath);
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }

            return modifiedData;
        }
    }
}