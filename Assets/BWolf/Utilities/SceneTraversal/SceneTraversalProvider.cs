#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.SceneTraversal
{
    public class SceneTraversalProvider : MonoBehaviour
    {
        [SerializeField]
        private bool _loadASync;
        
        [SerializeField]
        private LoadSceneMode _loadMode = LoadSceneMode.Single;
        
        [SerializeField]
        private KeyCode _hotkey = KeyCode.LeftControl;

        private static SceneTraversalProvider _instance;
        
        private int _activeSceneIndex;

        private int _sceneCount;

        private AsyncOperation _loadOperation;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            _sceneCount = SceneManager.sceneCountInBuildSettings;
        }

        private void Update()
        {
            // If the hotkey is not pressed, do nothing.
            if (!Input.GetKey(_hotkey))
                return;

            // If an async load operation is in progress, do nothing.
            if (_loadOperation != null)
                return;

            int increment = GetSceneIndexIncrement();
            if (increment != 0)
            {
                increment = RepeatUsingSceneCount(increment);
                IncrementScene(increment);
            }
        }

        private void IncrementScene(int increment)
        {
            int newActiveSceneIndex = _activeSceneIndex + increment;
            
            if (_loadASync)
            {
                _loadOperation = SceneManager.LoadSceneAsync(_activeSceneIndex + increment, _loadMode);
                _loadOperation.completed += _ =>
                {
                    SetNewActiveSceneIndex();
                    _loadOperation = null;
                };
            }
            else
            {
                SceneManager.LoadScene(_activeSceneIndex + increment, _loadMode);
                SetNewActiveSceneIndex();
            }

            void SetNewActiveSceneIndex() => _activeSceneIndex = newActiveSceneIndex;
        }

        private int RepeatUsingSceneCount(int increment)
        {
            int newActiveSceneIndex = _activeSceneIndex + increment;
            
            if (newActiveSceneIndex < 0)
                return (_sceneCount - 1) - _activeSceneIndex;
            if (newActiveSceneIndex == _sceneCount)
                return -_activeSceneIndex;
            return increment;
        }

        private int GetSceneIndexIncrement()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                return -1;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                return 1;
            if (GetNumberKeyDown(out int keyNumber))
                return MapKeyNumberToIncrement(keyNumber);
            return 0;
        }

        private bool GetNumberKeyDown(out int keyNumber)
        {
            keyNumber = -1;
            
            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
                keyNumber = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                keyNumber = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                keyNumber = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                keyNumber = 3;
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                keyNumber = 4;
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                keyNumber = 5;
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                keyNumber = 6;
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                keyNumber = 7;
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
                keyNumber = 8;
            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
                keyNumber = 9;

            return keyNumber != -1;
        }

        private int MapKeyNumberToIncrement(int keyNumber)
        {
            // Return 0 if the key number is the current active scene index.
            if (_activeSceneIndex == keyNumber)
                return 0;
            if (_activeSceneIndex > keyNumber)
                return keyNumber - _activeSceneIndex;
            return _activeSceneIndex - keyNumber;
        }
    }
}

#endif
