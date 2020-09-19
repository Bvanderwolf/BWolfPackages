using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>static class used for saving and loading progress related files to and from local storage</summary>
    public static class ProgressFileSystem
    {
        private static readonly string rootPath;
        private static readonly string saveDirectoryPath;

        private const string SAVE_FOLDER_NAME = "ProgressSaves";

        static ProgressFileSystem()
        {
            rootPath = Application.persistentDataPath;
            saveDirectoryPath = $"{rootPath}/{SAVE_FOLDER_NAME}";

            if (!Directory.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }
        }

        /// <summary>Saves value at given path from the save directory</summary>
        public static void SaveProgress<T>(string path, T value)
        {
            string filePath = $"{saveDirectoryPath}/{path}";

            CheckFileDirectorPath(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            bf.Serialize(file, value);
            file.Close();
        }

        /// <summary>Tries loading and outputting value at given path from the save directory</summary>
        public static bool LoadProgress<T>(string path, out T outValue)
        {
            string filePath = $"{saveDirectoryPath}/{path}";
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

        /// <summary>Checks whether the file at given filePath its parent directory exists. Creates it if doesn't</summary>
        private static void CheckFileDirectorPath(string filePath)
        {
            string directoryPath = filePath.Substring(0, filePath.LastIndexOf('/'));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}