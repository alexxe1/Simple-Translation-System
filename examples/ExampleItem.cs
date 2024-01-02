using TMPro;
using UnityEngine;

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
        // NOTE: This code assumes you've checked the singleton box.
        textToApply.text = TranslationSystem.Instance.GetTranslation(itemId);
    }
}