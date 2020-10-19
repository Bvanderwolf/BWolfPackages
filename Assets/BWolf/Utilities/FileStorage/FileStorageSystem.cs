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

        /// <summary>Saves value at given path from the root directory</summary>
        public static void SaveToFile<T>(string path, T value)
        {
            string filePath = $"{rootPath}/{path}";

            CreateFileDirectorPath(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            bf.Serialize(file, value);
            file.Close();
        }

        /// <summary>Tries loading and outputting value at given path from the root directory</summary>
        public static bool LoadFromFile<T>(string path, out T outValue)
        {
            string filePath = $"{rootPath}/{path}";
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

        /// <summary>Saves value as json string at given path from root directory. Can be used for saving monobehaviours and scriptableobjects</summary>
        public static void SaveAsJsonToFile<T>(string path, T value)
        {
            string filePath = $"{rootPath}/{path}";

            CreateFileDirectorPath(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            string json = JsonUtility.ToJson(value);
            bf.Serialize(file, json);
            file.Close();
        }

        /// <summary>Loads value as json string at given path from root directory. Can be used for loading monobehaviours and scriptableobjects</summary>
        public static bool LoadAsJsonFromFile<T>(string path, ref T outValue)
        {
            string filePath = $"{rootPath}/{path}";
            if (!File.Exists(filePath))
            {
                return false;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            string json = (string)bf.Deserialize(file);
            JsonUtility.FromJsonOverwrite(json, outValue);
            file.Close();

            return true;
        }

        /// <summary>Checks whether the file at given filePath its parent directory exists. Creates it if doesn't</summary>
        private static void CreateFileDirectorPath(string filePath)
        {
            string directoryPath = filePath.Substring(0, filePath.LastIndexOf('/'));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}