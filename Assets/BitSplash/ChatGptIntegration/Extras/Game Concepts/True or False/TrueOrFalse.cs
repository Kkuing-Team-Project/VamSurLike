using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrueOrFalse : MonoBehaviour
{
    public Button ButtonStart;
    public Button ButtonYes;
    public Button ButtonNo;
    public TMP_Text Question;

    ChatGPTConversation mConvo;
    bool mIsQuestion = false;

    void Start()
    {
        mConvo = ChatGPTConversation.Start(this)
            .SaveHistory(true).MaximumLength(150);
        mConvo.Temperature = 0.7f;
    }

    IEnumerator EndGame()
    {
        ButtonYes.gameObject.SetActive(false);
        ButtonNo.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f); // wait so we don't exhaust the rate limit
        ButtonStart.gameObject.SetActive(true);
    }
    public void StartGame()
    {
        ButtonStart.gameObject.SetActive(false);
        ButtonYes.gameObject.SetActive(true);
        ButtonNo.gameObject.SetActive(true);
        SetInteractionEnabled(false);
        mIsQuestion = true;
        mConvo.Say($"Ask me a random true or false trivia question");
    }

    public void Yes()
    {
        SetInteractionEnabled(false);
        mIsQuestion = false;
        mConvo.Say("True");
    }
    public void No()
    {
        SetInteractionEnabled(false);
        mIsQuestion = false;
        mConvo.Say("False");
    }
    void SetInteractionEnabled(bool isEnabled)
    {
        ButtonYes.interactable = isEnabled;
        ButtonNo.interactable = isEnabled;
    }
    void OnConversationResponse(string text)
    {
        SetInteractionEnabled(true);
        Question.text = text;
        if(mIsQuestion == false)
            StartCoroutine(EndGame());
    }
    void OnConversationError(string text)
    {
        mConvo.RestartConversation();
        Question.text = "Sorry , I'm overflown, Let's try again";
        StartCoroutine(EndGame());
        Debug.Log(text);
    }
}
