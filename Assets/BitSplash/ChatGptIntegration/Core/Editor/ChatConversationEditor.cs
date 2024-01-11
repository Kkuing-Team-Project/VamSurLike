using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace BitSplash.AI.GPT
{
    public class ChatConversationEditor : ChatGPTConversation
    {
        UnityWebRequest mRequest;
        ChatGptWindowBase mWindow;
        public ChatConversationEditor(ChatGptWindowBase window)
        {
            mWindow = window;
        }
        protected override void StartWebRequest()
        {
            mRequest = CreateChatApiWebRequest();
            var handler = mRequest.SendWebRequest();
            handler.completed += Handler_completed;
        }
        protected override void OnError(long code, string message)
        {
            base.OnError(code, message);
            if (mWindow != null)
                mWindow.OnConversationError(message);
        }
        protected override void OnResponse(ChatApiResponse response)
        {
            base.OnResponse(response);
            if (mWindow == null)
                return;
            if (response.choices != null && response.choices.Length > 0)
            {
                if (response.choices[0].finish_reason == "length")
                {
                    OnError(0, "length");
                    return;
                }
                if (response.choices[0].message == null || string.IsNullOrEmpty(response.choices[0].message.content))
                {
                    OnError(0, "empty");
                    return;
                }
                if (mSaveHistory)
                    mHistory.Add(response.choices[0].message);
               mWindow.OnConversationResponse(response.choices[0].message.content);
            }
        }
        private void Handler_completed(UnityEngine.AsyncOperation obj)
        {
            if (mRequest != null)
            {
                HandleWebRequestResult(mRequest);
                mRequest.Dispose();
                mRequest = null;
            }   
        }
        public string CurrentText()
        {
            if (mRequest == null)
                return "";
            return mRequest.downloadHandler.text;
        }
        protected override bool StopWebRequest()
        {
            if(mRequest != null)
            {
                mRequest.Abort();
                mRequest = null;
                return true;
            }
            return false;
        }
        
    }
}
