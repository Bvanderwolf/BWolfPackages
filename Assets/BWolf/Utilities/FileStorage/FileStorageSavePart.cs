// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

namespace BWolf.Utilities.FileStorage
{
    /// <summary>Provides saving and loading of data including monobehaviours and scriptableobjects</summary>
    public static partial class FileStorageSystem
    {
        private static readonly string rootPath;
        private static readonly object saveLock;

        static FileStorageSystem()
        {
            rootPath = Application.persistentDataPath;
            saveLock = new object();
        }

        /// <summary>Saves serialized value at given path from the root directory</summary>
        public static void SaveAsBinary<T>(string path, T value)
        {
            string filePath = Path.Combine(rootPath, path);

            VerifyFileDirectoryPath(filePath);

            lock (saveLock)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(filePath);

                bf.Serialize(file, value);
                file.Close();
            }
        }

        /// <summary>ASynchronously Saves serialized value at given path from the root directory</summary>
        public static async void SaveAsBinaryASync<T>(string path, T value)
        {
            await Task.Run(() => SaveAsBinary(path, value));
        }

        /// <summary>Saves value as json string at given path from root directory. Can also be used for saving monobehaviours and scriptableobjects</summary>
        public static void SaveAsJSON<T>(string path, T value, SaveMode mode = SaveMode.UnSafe)
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

            lock (saveLock)
            {
                File.WriteAllText(filePath, json);
            }
        }

        /// <summary>ASynchronously Saves value as json string at given path from root directory. Can also be used for saving monobehaviours and scriptableobjects</summary>
        public static async void SaveAsJSONASync<T>(string path, T value, SaveMode mode = SaveMode.UnSafe)
        {
            await Task.Run(() => SaveAsJSON(path, value, mode));
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

            lock (saveLock)
            {
                File.WriteAllText(filePath, text);
            }
        }

        /// <summary>ASynchronously Saves the given data as plain text at given path</summary>
        public static async void SaveAsPlainTextASync<T>(string path, T data, SaveMode mode = SaveMode.UnSafe)
        {
            await Task.Run(() => SaveAsPlainText(path, data, mode));
        }

        /// <summary>Checks whether the file at given filePath its parent directory exists. Creates it if doesn't</summary>
        private static void VerifyFileDirectoryPath(string filePath)
        {
            string directoryPath = filePath.Substring(0, filePath.LastIndexOf(Path.DirectorySeparatorChar));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }

    public enum SaveMode
    {
        UnSafe = 0,
        Hashed
    }
}