// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BWolf.Utilities.FileStorage
{
    /// <summary>Basic file storage system that can handle saving and loading data including monobehaviours and scriptableobjects</summary>
    public static class FileStorageSystem
    {
        private static readonly string rootPath;

        static FileStorageSystem()
        {
            rootPath = Application.persistentDataPath;
        }

        /// <summary>Saves serialized value at given path from the root directory</summary>
        public static void SaveToFile<T>(string path, T value)
        {
            string filePath = Path.Combine(rootPath, path);

            VerifyFileDirectoryPath(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            bf.Serialize(file, value);
            file.Close();
        }

        /// <summary>Tries loading and outputting serialized value at given path from the root directory</summary>
        public static bool LoadFromFile<T>(string path, out T outValue)
        {
            string filePath = Path.Combine(rootPath, path);
            if (!File.Exists(filePath))
            {
                outValue = default;
                return false;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            outValue = (T)bf.Deserialize(file);
            file.Close();

            return true;
        }

        /// <summary>Saves value as json string at given path from root directory. Can also be used for saving monobehaviours and scriptableobjects</summary>
        public static void SaveAsJsonToFile<T>(string path, T value, SaveMode mode = SaveMode.UnSafe)
        {
            string filePath = Path.Combine(rootPath, path);

            VerifyFileDirectoryPath(filePath);

            string json = JsonUtility.ToJson(value);
            if (mode == SaveMode.Hashed)
            {
                //store the hashed json in player prefs to use for verification on load
                string hash = EncryptionSystem.Hash(json);
                string key = $"{typeof(T).Name}@{path}";
                PlayerPrefs.SetString(key, hash);
            }

            File.WriteAllText(filePath, json);
        }

        /// <summary>Loads json string at given path from root directory and converts it into T. Can also be used for loading monobehaviours and scriptableobjects</summary>
        public static bool LoadJsonFromFile<T>(string path, out LoadResult<T> loadResult) where T : new()
        {
            string filePath = Path.Combine(rootPath, path);
            if (!File.Exists(filePath))
            {
                loadResult.data = default;
                loadResult.result = LoadResult.FileNotFound;
                return false;
            }
            else
            {
                T data = new T();
                string json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, data);

                loadResult.data = data;

                string key = $"{typeof(T).Name}@{path}";
                if (PlayerPrefs.HasKey(key))
                {
                    string storedHash = PlayerPrefs.GetString(key);
                    string loadedHash = EncryptionSystem.Hash(json);
                    if (storedHash != loadedHash)
                    {
                        Debug.LogWarning($"Loaded data {loadResult.data} @ {filePath} path isn't the same as the saved data :: this is not intended");
                        loadResult.result = LoadResult.FileChanged;
                        return true;
                    }
                }

                loadResult.result = LoadResult.Succesfull;
                return true;
            }
        }

        /// <summary>Saves the given data as plain text at given path</summary>
        public static void SaveAsPlainText<T>(string path, T data, SaveMode mode = SaveMode.UnSafe)
        {
            string filePath = Path.Combine(rootPath, path);

            VerifyFileDirectoryPath(filePath);

            string text = data.ToString();
            if (mode == SaveMode.Hashed)
            {
                //store the hashed text in player prefs to use for verification on load
                string hash = EncryptionSystem.Hash(text);
                string key = $"{typeof(T).Name}@{path}";
                PlayerPrefs.SetString(key, hash);
            }

            File.WriteAllText(filePath, text);
        }

        /// <summary>Loads data of given type as a string from given path</summary>
        public static bool LoadFromPlainText<T>(string path, out LoadResult<string> loadResult)
        {
            string filePath = Path.Combine(rootPath, path);

            if (!File.Exists(filePath))
            {
                loadResult.data = string.Empty;
                loadResult.result = LoadResult.FileNotFound;
                return false;
            }
            else
            {
                loadResult.data = File.ReadAllText(filePath);

                string key = $"{typeof(T).Name}@{path}";
                if (PlayerPrefs.HasKey(key))
                {
                    //if a hash was stored for this loadable object in player prefs, check if it matches the loaded one
                    string storedHash = PlayerPrefs.GetString(key);
                    string loadedHash = EncryptionSystem.Hash(loadResult.data);
                    if (storedHash != loadedHash)
                    {
                        Debug.LogWarning($"Loaded data {loadResult.data} @ {filePath} path isn't the same as the saved data :: this is not intended");
                        loadResult.result = LoadResult.FileChanged;
                        return true;
                    }
                }

                loadResult.result = LoadResult.Succesfull;
                return true;
            }
        }

        /// <summary>Checks whether the file at given filePath its parent directory exists. Creates it if doesn't</summary>
        private static void VerifyFileDirectoryPath(string filePath)
        {
            string directoryPath = filePath.Substring(0, filePath.LastIndexOf('/'));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }

    public struct LoadResult<T>
    {
        public LoadResult result;
        public T data;
    }

    public enum LoadResult
    {
        FileNotFound = 0,
        FileChanged,
        Succesfull
    }

    public enum SaveMode
    {
        UnSafe = 0,
        Hashed
    }
}