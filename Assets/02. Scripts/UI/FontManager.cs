using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FontManager : MonoBehaviour
{
    public TMP_FontAsset text_Font;
    public TextMeshProUGUI animatedText;
    public float scaleSpeed = 1.0f;
    public float maxScale = 1.2f;
    public float minScale = 0.8f;

    private bool scalingUp = true;
    void Start()
    {
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            text.font = text_Font;
        }
    }

    void Update()
    {
        if (animatedText != null)
        {
            // Animate the scale of the text
            if (scalingUp)
            {
                // Scale up
                animatedText.transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
                if (animatedText.transform.localScale.x > maxScale)
                {
                    scalingUp = false;
                }
            }
            else
            {
                // Scale down
                animatedText.transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
                if (animatedText.transform.localScale.x < minScale)
                {
                    scalingUp = true;
                }
            }
        }
    }
}
