using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BitSplash.AI.GPT
{ 
    [Serializable]
    class AnswerString
    {
        public AnswerString(bool isCode,string codeType,string text)
        {
            IsCode = isCode;
            Text = text;
            CodeType = codeType;
            if (IsCode && String.IsNullOrEmpty(codeType))
                CodeType = "csharp";
        }
        public Vector2 scroll;
        public bool IsCode;
        public string Text;
        public string CodeType;
        public string HighlightCode;
    }
}
