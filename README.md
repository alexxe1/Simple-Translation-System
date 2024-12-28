# Simple Translation System
A simple asset for implementing translation, based on .csv files, for your [Unity](https://unity.com/) game.

## How does it work?
- **Simple Translation System** works using `.csv (Comma Separated Values)` files. `.csv` is a pretty convenient file type that separates spreadsheet values using `;`. The idea of this asset is pretty straightforward, you first get the raw data, then convert it to a string array and, finally, retrieve it using a unique ID.

## Download
https://github.com/alexxe1/Simple-Translation-System/releases/latest/download/TranslationSystem.unitypackage


## How to use
0. Create your `.csv` file using [Google Sheets](https://docs.google.com/spreadsheets/u/0/), [Microsoft Excel](https://www.microsoft.com/es-ar/microsoft-365/excel) or any other spreadsheet software. 
Make sure to format it using this convention:

```
id;lang1;lang2;lang3...
```

For example:

```
welcome;Hola;Hello
quit;¡Adios!;Goodbye!
...
```

> A .csv example can be found in the repository.

1. Drag the `TranslationSystem` script to a GameObject.
2. Drag your `.csv` to the `Raw Data` slot.
3. Configure the script (hover your mouse over each setting to see more information).
4. Add your languages (must be the same names and order as your .csv file).
5. Create a script that retrieves the translations, like this:

```
public class ExampleItem : MonoBehaviour
{
    [Header("References")] 
    private TMP_Text textToApply;
    
    [Header("Settings")] 
    [SerializeField] private string itemId;

    private void Awake()
    {
        textToApply = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        UpdateText();
    }
        
    private void UpdateText()
    {
        textToApply.text = TranslationSystem.Instance.GetTranslation(itemId);
    }
}
```

> **NOTE:** This code assumes you've checked the `singleton` box.

## Important
- After setting a new language, prompt the user to restart your game so changes take effect.

- When creating your `.csv` file, make sure not to include `new lines` or `semicolons` directly. 
Instead, use `<newline>` and `<semicolon>`.

For example:<br/>
```The pancakes were delicious<semicolon> they were fluffy and sweet.```
<br/>or<br/>
```This is the first line.<newline>This is a new line.```

## Inspector Settings
| Setting name | Description                    |
| ------------ | ------------------------------ |
| `Raw data` | The .csv file to extract the data from. |
| `Debug mode` | Enables the debug mode. |
| `Singleton` | If enabled, TranslationSystem will behave as a Singleton. |
| `AutoLoadPreferredLanguage` | Automatically loads the preferred language from the PlayerPrefs. Disabling this requires calling 'LoadPreferredLanguage' method manually. |
| `PreferredLanguagePath` | The path where the preferred translation will be stored in PlayerPrefs. Leave blank for default ('preferred_language'). | 
| `AvailableLanguages` | The list with all available languages. |

## Methods
| Function name | Parameters| Description                    |
| ------------- | --------- | ------------------------------ |
| `SetLanguage`  | `string newLang` | Sets the current language to a new one. If two languages have the same ID, the function will assign the first one to appear on the list. TranslationSystem will automatically save the preferred language in PlayerPrefs.     |
| `GetTranslation` | `string itemId` | Returns the corresponding translation depending on the selected language.|
| `GetCurrentLanguage` | | Returns the current language as a `Language` class. |
| `GetAvailableLanguages` | | Returns all available languages as an array of 'Language' class. |
| `GetCurrentSavePath` | | Returns the current save path used in PlayerPrefs. |
| `LoadPreferredLanguage` | | Loads the preferred language stored in the save path (can be modified in settings). If `autoLoadPreferredLanguage` option is enabled, this will be called automatically when starting. |

## License

MIT License

Copyright (c) 2024 alexe1

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
