using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BitSplash.AI.GPT
{
    class CodeExtractor
    {
        /// <summary>
        /// finds the starter string for source code. marked with ```csharp or ```c#
        /// </summary>
        /// <param name="response"></param>
        /// <param name="start"></param>
        /// <param name="starterLength"></param>
        /// <returns></returns>
        static int FindStarter(string response,int start,out string codeType,out int starterLength)
        {
            string starter = "```";
            int index = response.IndexOf(starter, start); // check for ```csharp
            if (index == -1)
            {
                starterLength = 0;
                codeType = "";
                return -1;
            }
            int end = index+3;
            codeType = response.Substring(index+3, end-(index+3)).Trim();
            starterLength = end - index; // return the length of the starter string (so we can skip it)
            return index; // return the index of the code string
        }
        public static string getClassName(string code)
        {
            Match m = Regex.Match(code, @"class\s+([A-Za-z_][A-Za-z0-9_]+)");
            if(m.Success)
            {
                if (m.Groups.Count <= 1)
                    return null;
                return m.Groups[1].Value;
            }
            return null;
        }
        /// <summary>
        /// splits the given chatGpt response to text blocks and code blocks.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static IEnumerable<AnswerString> ExtractCode(string response)
        {
            int index = 0;
            int starterLength;
            while (index < response.Length)
            {
                string codeType = "";
                int start = FindStarter(response, index,out codeType,out starterLength); // find code block
                if (start == -1) // if no code block exists
                    start = response.Length; // then we are at the end of the string
                string nonCode = response.Substring(index, start-index).Trim(); // create the non code block

                if(nonCode.Length >0) // dont return empty blocks
                    yield return new AnswerString(false, "", nonCode); //return the non code block
                index = start + starterLength; //skip to the code block
                if (index >= response.Length) // dont continue if we are past the end of the string
                    break;                  
                int end = response.IndexOf("```", index); // find the end of the code block
                if (end == -1)
                    end = response.Length; // if no ending found for the code block. then the end of the string it is
                string code = response.Substring(index, end - index).Trim();
                if (code.StartsWith("csharp"))
                    code = code.Substring("csharp".Length);
                if (code.StartsWith("c#"))
                    code = code.Substring("c#".Length);
                if (code.StartsWith("cs"))
                    code = code.Substring("cs".Length);
                codeType = "csharp";
                if (code.Length > 0)
                    yield return new AnswerString(true, codeType, code);
                index = end + "```".Length;
            }
        }
    }
}
