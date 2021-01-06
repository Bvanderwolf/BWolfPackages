using BWolf.Utilities.FileStorage;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.FileStorage
{
    public class InputStorageUI : MonoBehaviour
    {
        [SerializeField]
        private InputField input = null;

        private const string FILE_PATH = "inputSave.txt";

        public void OnSaveButtonClick()
        {
            string text = input.text;
            if (!string.IsNullOrEmpty(text))
            {
                FileStorageSystem.SaveAsPlainText(FILE_PATH, text, SaveMode.Hashed);
            }
        }

        public void OnLoadButtonClick()
        {
            if (FileStorageSystem.LoadPlainText<string>(FILE_PATH, out LoadResult<string> loadResult))
            {
                input.text = loadResult.data;
            }
        }
    }
}