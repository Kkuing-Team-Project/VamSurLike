using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    [Serializable]
    public class ApiUsage
    {
        /// <summary>
        /// the amount of tokens in the request
        /// </summary>
        public int prompt_tokens;
        /// <summary>
        /// the amount of tokens int the response
        /// </summary>
        public int completion_tokens;
        /// <summary>
        /// the total amount of tokens in both the request and response
        /// </summary>
        public int total_tokens;
    }
}
