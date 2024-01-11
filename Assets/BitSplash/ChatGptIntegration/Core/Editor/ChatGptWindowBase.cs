// Importing necessary namespaces
using BitSplash.AI.GPT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Defining a namespace to keep the class ChatGptWindowBase
namespace BitSplash.AI.GPT
{
    // Defining the class ChatGptWindowBase which is inherited from EditorWindow
    public class ChatGptWindowBase : EditorWindow
    {
        // Defining the properties of the class
        protected ChatConversationEditor Conversation { get; private set; }
        protected Highlight.Highlighter Highlight { get; private set; }

        // Constructor of the class
        public ChatGptWindowBase()
        {
            
        }

        // Method which is called when the window is enabled
        protected virtual void OnEnable()
        {
            try
            {
                // Creating a new instance of Highlighter and ChatConversationEditor
                Highlight = new Highlight.Highlighter(new BitSplashUnityGUIEngine());
                Conversation = new ChatConversationEditor(this);
                // Setting the maximum length for the chat conversation
                Conversation.MaximumLength(2048);
            }
            catch(Exception)
            {
                Conversation = null;
            }
        }

        // Method which is called when the window is disabled
        protected virtual void OnDisable()
        {

        }

        // Method which is called when a response is received in the conversation
        public virtual void OnConversationResponse(string text)
        {

        }

        // Method which is called when an error occurs in the conversation
        public virtual void OnConversationError(string text)
        {

        }
    }
}