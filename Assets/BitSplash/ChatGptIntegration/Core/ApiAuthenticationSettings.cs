using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    [CreateAssetMenu(fileName = "ApiAuthenticationSettings", menuName = "ScriptableObjects/ApiAuthenticationSettings", order = 1)]
    public class ApiAuthenticationSettings : ScriptableObject
    {
        public const string defaultUrl = @"https://api.openai.com/v1/chat/completions";
        public ChatModels Model = ChatModels.GPT_3_5_TURBO; 
        public string CompletionUrl = defaultUrl;
        public string PrivateApiKey;
        public string Organization;
    }
}
