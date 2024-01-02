using System;
using UnityEngine;

namespace alexe1.Translation
{
    [Serializable]
    public class Language
    {
        public string languageName;
    }

    public class TranslationSystem : MonoBehaviour
    {
        #region VARIABLES

        [Header("References")] 
        [Tooltip("The .csv file from where the data will be extracted. Must follow a convention.")]
        [SerializeField] private TextAsset rawData;

        [Header("Settings")] 
        [Tooltip("Enables the debug mode.")]
        [SerializeField] private bool debugMode;

        [Tooltip("If enabled, TranslationSystem will behave as a Singleton.")]
        [SerializeField] private bool singleton = true;
    
        [Tooltip("Automatically loads the preferred language from the PlayerPrefs. Disabling this requires calling 'LoadPreferredLanguage' method manually.")]
        [SerializeField] private bool autoLoadPreferredLanguage = true;
    
        [Tooltip("The path where the preferred translation will be stored in PlayerPrefs. Leave blank for default ('preferred_language')")]
        [SerializeField] private string preferredLanguagePath = "preferred_language";
    
        [Header("Languages")]
        [Tooltip("The list with all available languages.")]
        [SerializeField] private Language[] availableLanguages;

        [Header("Values")] 
        private string[,] _processedData;
        private Language _currentLanguage;
    
        // Singleton
        public static TranslationSystem Instance;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            SetSingleton();
            ProcessData();
        }

        #endregion

        #region SETUP

        private void SetSingleton()
        {
            if (!singleton) return;
        
            if (Instance == null)
            {
                Instance = this;
            
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void ProcessData()
        {
            var lines = rawData.text.Split('\n');

            // Initialize the _processedData array with the correct dimensions
            _processedData = new string[lines.Length, lines[0].Split(';').Length];

            // Process each line
            for (var i = 0; i < lines.Length; i++)
            {
                // Split the line into words
                var words = lines[i].Trim().Split(';');

                // Assign each word to the _processedData array
                for (var j = 0; j < words.Length; j++)
                {
                    _processedData[i, j] = words[j];
                }
            }

            if (!autoLoadPreferredLanguage) return;
        
            LoadPreferredLanguage();
        }

        #endregion

        #region SET & GET

        /// <summary>
        /// Sets the current language to a new one.
        /// If two languages have the same ID, the function will
        /// assign the first one to appear on the list.
        /// TranslationSystem will automatically save the preferred
        /// language in PlayerPrefs.
        /// </summary>
        /// <param name="newLanguageName">The new language name</param>
        public void SetLanguage(string newLanguageName)
        {
            foreach (var language in availableLanguages)
            {
                if (language.languageName != newLanguageName) continue;
            
                _currentLanguage = language;
                SavePreferredLanguage();
            
                DebugMessage($"Language successfully changed to: {_currentLanguage.languageName}!", DebugType.Default);
                
                return;
            }

            DebugMessage($"No valid language was found with the name: {newLanguageName}!. Make sure you added the language in the settings.", DebugType.Error);
        }

        /// <summary>
        /// Returns the corresponding translation depending on the selected language.
        /// </summary>
        /// <example>
        /// <para>Having a .csv file like:</para> <para>welcome;Hola;Hello</para>
        ///  <para>and the current language as "Spanish"</para>
        /// <code>
        /// GetTranslation("welcome");
        /// </code>
        /// <para>would return "Hola"</para>
        /// </example>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public string GetTranslation(string itemId)
        {
            // Find the index of the id in the first column of _processedData
            for (var i = 0; i < _processedData.GetLength(0); i++)
            {
                if (_processedData[i, 0] == itemId)
                {
                    // Return the translation at the specified language index
                    return _processedData[i, GetLanguageID(_currentLanguage.languageName) + 1].Replace("<semicolon>", ";").Replace("<newline>", "\n");
                }
            }

            DebugMessage($"Could not find any translation for the item ID: {itemId} in the language: {_currentLanguage.languageName}", DebugType.Error);
        
            return null;
        }

        /// <summary>
        /// Returns the current language as a 'Language' class.
        /// </summary>
        /// <example>
        /// <code>
        /// Language currentLanguage = GetCurrentLanguage();
        /// Debug.Log(currentLanguage.languageName);
        /// </code>
        /// </example>
        /// <returns></returns>
        public Language GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        /// <summary>
        /// Returns all available languages as an array of 'Language' class.
        /// </summary>
        /// <example>
        /// <code>
        /// Language[] availableLanguages = GetAvailableLanguages();
        /// Debug.Log(availableLanguages[0].languageName);
        /// </code>
        /// </example>
        /// <returns></returns>
        public Language[] GetAvailableLanguages()
        {
            return availableLanguages;
        }

        /// <summary>
        /// Returns the current save path used in PlayerPrefs.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSavePath()
        {
            return preferredLanguagePath;
        }

        #endregion

        #region HELPERS

        private int GetLanguageID(string languageName)
        {
            for (var i = 0; i < availableLanguages.Length; i++)
            {
                if (availableLanguages[i].languageName == languageName)
                {
                    return i;
                }
            }

            DebugMessage($"Could not determine the ID of the language: {languageName}... Returning the first one. Make sure you added the language to the list and that no grammar mistakes were made.", DebugType.Error);
            return 0;
        }

        #endregion

        #region LOAD & SAVE

        private void SavePreferredLanguage()
        {
            var loadPath = preferredLanguagePath == string.Empty ? "preferred_language" : preferredLanguagePath;
        
            PlayerPrefs.SetString(loadPath, _currentLanguage.languageName);
        
            DebugMessage($"Successfully saved: {_currentLanguage.languageName} to path: {loadPath}!", DebugType.Default);
        }

        /// <summary>
        /// Loads the preferred language stored in the save path (can be modified in settings).
        /// If 'autoLoadPreferredLanguage' option is enabled, this will be called automatically when starting.
        /// </summary>
        public void LoadPreferredLanguage()
        {
            var loadPath = preferredLanguagePath == string.Empty ? "preferred_language" : preferredLanguagePath;
            var loadedLanguage = PlayerPrefs.GetString(loadPath, availableLanguages[0].languageName);
            
            SetLanguage(loadedLanguage);
            
            DebugMessage($"Successfully loaded: {loadedLanguage} from path: {loadPath}!", DebugType.Default);
        }

        #endregion

        #region DEBUG

        private enum DebugType
        {
            Default,
            Error,
            Warning
        }
    
        /// <summary>
        /// Internal method to debug custom messages.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="debugType">The debug type</param>
        private void DebugMessage(string message, DebugType debugType)
        {
            if (!debugMode) return;

            switch (debugType)
            {
                case DebugType.Default:
                    Debug.Log(message);
                    break;
                case DebugType.Error:
                    Debug.LogError(message);
                    break;
                case DebugType.Warning:
                    Debug.LogWarning(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(debugType), debugType, null);
            }
        }
        
        #endregion
    }
}