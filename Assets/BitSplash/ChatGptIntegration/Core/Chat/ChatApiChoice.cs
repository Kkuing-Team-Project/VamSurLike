using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    [Serializable]
    public class ChatApiChoice
    {
        /// <summary>
        /// this index of the choice
        /// </summary>
        public int index;
        /// <summary>
        /// the message completion for the choice
        /// </summary>
        public ChatApiMessage message;
        /// <summary>
        /// the finish reason for choice
        /// </summary>
        public string finish_reason;
    }
}