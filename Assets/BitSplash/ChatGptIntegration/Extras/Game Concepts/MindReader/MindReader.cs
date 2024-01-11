using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MindReader : MonoBehaviour
{
    /// <summary>
    /// it seems more then 10 questions is to heavy for Chat GPT.
    /// </summary>
    [Range(5,10)]
    public int NumberOfQuestions = 10;
    public Button ButtonStart;
    public Button ButtonYes;
    public Button ButtonNo;
    public TMP_Text Answer;

    /// <summary>
    /// hold the conversation
    /// </summary>
    ChatGPTConversation mConvo; 
    void Start()
    {
        //start a chat gpt conversation
        mConvo = ChatGPTConversation.Start(this)
                                    .SaveHistory(true) // keep track of the conversation
                                    .MaximumLength(600);
       // mConvo.Temperature = 0.3f; // be as predicteable as possible
    }

    public void StartGame()
    {
        ButtonStart.gameObject.SetActive(false);
        ButtonYes.gameObject.SetActive(true);
        ButtonNo.gameObject.SetActive(true);
        //start by saying to the ai we are thinking of a celebrity and we want them to guess it.
        //this is done under the hood so the user doesnt know
        mConvo.Say($"I am thinking of a well known celebrity, try guessing who by asking only {NumberOfQuestions} yes or no questions. start by asking the first one");
    }
    IEnumerator WaitForRateLimit()
    {
        yield return new WaitForSeconds(5f); // wait for 5 seconds between questions. So we don't exust the rate limit
        SetInteractionEnabled(true);
    }

    /// <summary>
    /// called by the yes button
    /// </summary>
    public void Yes()
    {
        SetInteractionEnabled(false);
        mConvo.Say("Yes"); // if the user clicked yes, then say yes
    }
    /// <summary>
    /// called by the no button
    /// </summary>
    public void No()
    {
        SetInteractionEnabled(false);
        mConvo.Say("No"); // if the user clicked no , then say no
    }
    /// <summary>
    /// make the yes and no buttons interactable or no interactable
    /// </summary>
    /// <param name="isEnabled"></param>
    void SetInteractionEnabled(bool isEnabled)
    {
        ButtonYes.interactable = isEnabled;
        ButtonNo.interactable = isEnabled;
    }
    void OnConversationResponse(string text)
    {
        StartCoroutine(WaitForRateLimit()); // renable the buttons after 5 seconds
        Answer.text = text; // set the answer text
    }
    void OnConversationError(string text)
    {
        StartCoroutine(WaitForRateLimit()); // renable the buttons after 5 seconds
        //make a nice displayable error message and ask the user to repeat their answer
        string errorText = "Sorry , I'm overflown, Please answer again.\r\n";
        if(Answer.text.StartsWith(errorText) == false)
            Answer.text = errorText + Answer.text;
        Debug.Log(text); // log the error
    }
}
