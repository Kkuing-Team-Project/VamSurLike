using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    /// <summary>
    /// This class is created according to the message format of the chat gpt api as specified here :https://platform.openai.com/docs/guides/chat/introduction
    /// </summary>
    [Serializable]
    public class ChatApiMessage
    {
        public ChatApiMessage(string _role,string _content)
        {
            role = _role;
            content = _content;
        }
        public static ChatApiMessage System(string content)
        {
            return new ChatApiMessage(RoleSystem, content);
        }
        public static ChatApiMessage User(string content)
        {
            return new ChatApiMessage(RoleUser, content);
        }

        public const string RoleAssistant = "assistant";
        public const string RoleUser = "user";
        public const string RoleSystem = "system";
        /// <summary>
        /// the role of the message. This can be one of the strings RoleAssistant,RoleUser,RoleSystem
        /// </summary>
        public string role;
        /// <summary>
        /// the content of the message
        /// </summary>
        public string content;
    }
}