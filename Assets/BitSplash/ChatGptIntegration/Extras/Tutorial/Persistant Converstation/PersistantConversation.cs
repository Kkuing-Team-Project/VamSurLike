using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersistantConversation : MonoBehaviour
{
    public string ConversationName = "MyConvo";
    public TMP_Text AnswerText;
    ChatGPTConversation Conversation;
    bool askedAgain = false;
    void Start()
    {
        // call ChatGPTConversation.Start to start a conversation at any time
        Conversation = ChatGPTConversation.Start(this)
            .System("Answer as a helpful friend") // sets the identity of the chat ai agent
            .MaximumLength(2048) // set the maximum length of tokens per request
            .SaveHistory(true); // this will keep track of older messages but also increases the number of tokens used
        Conversation.Load(ConversationName); // load the converstation from the file <ConversationName>
        //call Conversation.Say to say something. Make sure not to call it again until you get a response or error
        AnswerText.text += "Me: What is my name ?\r\n";
        Conversation.Say("What is my name ?");
    }

    /// <summary>
    /// this method is called when a response comes from the conversation
    /// </summary>
    /// <param name="text">the response text</param>
    void OnConversationResponse(string text)
    {
        Conversation.Save(ConversationName); // save the converstation to the file <ConversationName>
        AnswerText.text += "Chat GPT:" + text +"\r\n";
        if (askedAgain == false)
        {
            askedAgain = true;
            Conversation.Say("My name is John Smith");
            AnswerText.text += "Me: My name is John Smith\r\n";
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
        Conversation.Save(ConversationName);
    }
}
