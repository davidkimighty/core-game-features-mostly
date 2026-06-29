using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace GameReady.SaveLoad
{
    public class SaveLoader<T> where T : class
    {
        public event Action OnSave;
        public event Action OnLoad;

        private T _saveData;
        private ISaveLoadSerializer _serializer;
        private SaveLoadSetting _setting;
        private string _saveDirectoryPath;
        private string _saveFilePath;

        public string SaveDirectoryPath => _saveDirectoryPath;
        public string SaveFilePath => _saveFilePath;

        public SaveLoader(T saveData, ISaveLoadSerializer saveSerializer, SaveLoadSetting setting)
        {
            _saveData = saveData;
            _serializer = saveSerializer;
            _setting = setting;
            _saveDirectoryPath = Path.Combine(Application.persistentDataPath, setting.SaveDirectory);
            _saveFilePath = Path.Combine(_saveDirectoryPath, setting.SaveFileName);
        }

        public async Task LoadDataAsync()
        {
            CreateSaveDirectoryIfNeeded(_saveDirectoryPath);
            if (!File.Exists(_saveFilePath))
            {
                Debug.Log("[SaveLoader] Saved data does not exist.");
                return;
            }

            try
            {
                using FileStream fileStream = new(_saveFilePath, FileMode.Open);
                if (_setting.UseCryptoStream)
                {
                    using Aes aes = Aes.Create();
                    byte[] iv = new byte[aes.IV.Length];
                    fileStream.Read(iv, 0, iv.Length);

                    byte[] key = await LoadKeyAsync(_setting.AesKeyName);
                    if (key == null)
                    {
                        Debug.Log("[SaveLoader] Key does not exist.");
                        return;
                    }

                    using CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                    await StreamReadAsync(cryptoStream, _saveData);
                }
                else
                {
                    await StreamReadAsync(fileStream, _saveData);
                }
                OnLoad?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log($"[SaveLoader] {e.Message}");
            }
        }

        public async Task SaveDataAsync()
        {
            CreateSaveDirectoryIfNeeded(_saveDirectoryPath);
            try
            {
                using Aes aes = Aes.Create();
                byte[] key = aes.Key;
                byte[] iv = aes.IV;
                await SaveKeyAsync(_setting.AesKeyName, key);

                using FileStream fileStream = new(_saveFilePath, FileMode.Create);
                if (_setting.UseCryptoStream)
                {
                    await fileStream.WriteAsync(iv, 0, iv.Length);
                    using CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                    await StreamWriterAsync(cryptoStream, _saveData);
                }
                else
                {
                    await StreamWriterAsync(fileStream, _saveData);
                }
                OnSave?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log($"[SaveLoader] {e.Message}");
            }
        }

        private void CreateSaveDirectoryIfNeeded(string path)
        {
            if (Directory.Exists(path)) return;

            Directory.CreateDirectory(path);
            Debug.Log("[SaveLoader] Save directory created.");
        }

        private async Task StreamReadAsync(Stream stream, T data)
        {
            using StreamReader streamReader = new(stream);
            string serialized = await streamReader.ReadToEndAsync();
            _serializer.Deserialize(serialized, data);
            Debug.Log("[SaveLoader] Data loaded.");
        }

        private async Task StreamWriterAsync(Stream stream, T data)
        {
            using StreamWriter streamWriter = new(stream);
            string serialized = _serializer.Serialize(data);
            await streamWriter.WriteAsync(serialized);
            Debug.Log("[SaveLoader] Data saved.");
        }

        private async Task SaveKeyAsync(string keyName, byte[] key)
        {
#if UNITY_STANDALONE_WIN
            byte[] encrypted = ProtectedData.Protect(key, null, DataProtectionScope.CurrentUser);
            await File.WriteAllBytesAsync(GetKeyPath(keyName), encrypted);
#elif UNITY_STANDALONE_OSX

#else
            Debug.Log("[SaveLoader] Platform not supported. Unable to save key.");
            return;
#endif
        }

        private async Task<byte[]> LoadKeyAsync(string keyName)
        {
#if UNITY_STANDALONE_WIN
            string keyPath = GetKeyPath(keyName);
            if (!File.Exists(keyPath)) return null;

            byte[] encrypted = await File.ReadAllBytesAsync(keyPath);
            return ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
#elif UNITY_STANDALONE_OSX
            return null;
#else
            Debug.Log("[SaveLoader] Platform not supported. Unable to load key.");
            return null;
#endif
        }

        private string GetKeyPath(string keyName)
        {
            return Path.Combine(Application.persistentDataPath, keyName + ".key");
        }
    }
}
