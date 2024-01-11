using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BitSplash.AI.GPT
{
    /// <summary>
    /// the api response for chat requests
    /// </summary>
    [Serializable]
    public class ChatApiResponse
    {
        /// <summary>
        /// the id of the response
        /// </summary>
        public string id;
        //public string object;
        public long created;
        /// <summary>
        /// an array of choices for the response
        /// </summary>
        public ChatApiChoice[] choices;
        /// <summary>
        /// api usage for the request
        /// </summary>
        public ApiUsage usage;
    }
}
