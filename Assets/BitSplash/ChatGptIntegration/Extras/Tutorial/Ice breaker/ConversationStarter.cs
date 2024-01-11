using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace BitSplash.AI.GPT.Extras
{
    public class ConversationStarter : MonoBehaviour
    {
        public TMP_Text Answer;
        ChatGPTConversation Conversation;
        void Start()
        {
            // call ChatGPTConversation.Start to start a conversation at any time
            Conversation = ChatGPTConversation.Start(this)
                .System("Answer as a helpful unity developer") // sets the identity of the chat ai agent
                .MaximumLength(2048); // set the maximum length of tokens per request

            //call Conversation.Say to say something. Make sure not to call it again until you get a response or error
            Conversation.Say("who are you?");
            Answer.text += "Me: who are you?\r\n";
        }

        /// <summary>
        /// this method is called when a response comes from the conversation
        /// </summary>
        /// <param name="text">the response text</param>
        void OnConversationResponse(string text)
        {
            Answer.text += "ChatGPT: " + text;
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
}