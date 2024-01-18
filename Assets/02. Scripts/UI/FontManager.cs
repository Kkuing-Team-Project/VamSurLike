using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FontManager : MonoBehaviour
{
    public TMP_FontAsset text_Font;
    void Start()
    {
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            text.font = text_Font;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
