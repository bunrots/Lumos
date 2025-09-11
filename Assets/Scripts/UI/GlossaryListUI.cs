using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class GlossaryListUI : MonoBehaviour
{
    public GameObject buttonPrefab;    // simple button with TMP_Text
    public Transform contentParent;    // parent for list items (scroll view content)
    public GlossaryInfoUI infoUI;

    private void Start()
    {
        PopulateList();
    }

    public void PopulateList()
    {
        // Load JSON from Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("PlanetData/glossary");
        if (jsonFile == null)
        {
            Debug.LogError("Glossary JSON not found in Resources/PlanetData/");
            return;
        }

        string wrappedJson = "{\"entries\":" + jsonFile.text + "}";
        GlossaryList glossaryList = JsonUtility.FromJson<GlossaryList>(wrappedJson);

        foreach (GlossaryEntry entry in glossaryList.entries)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, contentParent);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null) buttonText.text = entry.Term;

            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                GlossaryEntry captured = entry;
                button.onClick.AddListener(() => infoUI.DisplayEntry(captured));
            }
        }
    }
}