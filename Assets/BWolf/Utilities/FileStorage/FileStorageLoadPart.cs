// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BWolf.Utilities.FileStorage
{
    /// <summary>
    /// Provides Loading functionalities to retreived saved data
    /// </summary>
    public static partial class FileStorageSystem
    {
        /// <summary>Tries loading and outputting serialized value at given path from the root directory</summary>
        public static bool LoadBinary<T>(string path, out T outValue)
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

        /// <summary>Loads json string at given path from root directory and converts it into T. Can also be used for loading monobehaviours and scriptableobjects</summary>
        public static bool LoadFromJSON<T>(string path, out LoadResult<T> loadResult) where T : new()
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

        /// <summary>Loads data of <typeparamref name="T"/> as a string from given path</summary>
        /// <typeparam name="T">The type of object to load from text</typeparam>
        public static bool LoadPlainText<T>(string path, out LoadResult<string> loadResult)
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
}