using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdvancedConversation : MonoBehaviour
{
    public TMP_Text AnswerText;
    ChatGPTConversation Conversation;
    bool askedAgain = false;
    void Start()
    {
        // call ChatGPTConversation.Start to start a conversation at any time
        Conversation = ChatGPTConversation.Start(this)
            .System("Answer as a helpful unity developer") // sets the identity of the chat ai agent
            .MaximumLength(2048) // set the maximum length of tokens per request
            .SaveHistory(true); // this will keep track of older messages but also increases the number of tokens used
        Conversation.Temperature = 0.7f; // control chat gpt inner parameters 
        Conversation.Top_P = 1f;
        Conversation.Presence_Penalty = 0f;
        Conversation.Frequency_Penalty = 0f;
        //call Conversation.Say to say something. Make sure not to call it again until you get a response or error
        Conversation.Say("My name is John Smith");
        AnswerText.text += "Me: My name is John Smith\r\n";
    }

    /// <summary>
    /// this method is called when a response comes from the conversation
    /// </summary>
    /// <param name="text">the response text</param>
    void OnConversationResponse(string text)
    {
        AnswerText.text += "Chat GPT:" + text +"\r\n";
        if (askedAgain == false)
        {
            askedAgain = true;
            AnswerText.text += "Me: What is my name ?\r\n";
            Conversation.Say("What is my name ?");
        }
    }
    /// <summary>
    /// this method is called if there was an error in the api
    /// </summary>
    /// <param name="text">the error message</param>
    void OnConversationError(string text)
    {
        Debug.Log("Error : " + text);
        Conversation.RestartConversation();
    }
}
