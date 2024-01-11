using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace BitSplash.AI.GPT
{
    class ChatConversationRuntime : ChatGPTConversation
    {
        /// <summary>
        /// runtime requests use a monobehviour
        /// </summary>
        MonoBehaviour mBehviour;
        /// <summary>
        /// keep track of the coroutine so it can be canceled
        /// </summary>
        Coroutine mCoroutine;

        public ChatConversationRuntime(MonoBehaviour b)
            :base()
        {
            mBehviour = b;
        }
        protected override void StartWebRequest()
        {
            mCoroutine = mBehviour.StartCoroutine(RequestCompletion());
        }
        protected override bool StopWebRequest()
        {
            if (mCoroutine != null)
            {
                mBehviour.StopCoroutine(mCoroutine);
                mCoroutine = null;
                return true;
            }
            return false;
        }
        protected override void OnError(long code,string message)
        {
            base.OnError(code, message);
            mBehviour.SendMessage("OnConversationError",message);
        }

        protected override void OnResponse(ChatApiResponse response)
        {
            base.OnResponse(response);
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
                mBehviour.SendMessage("OnConversationResponse", response.choices[0].message.content);
            }
        }

        IEnumerator RequestCompletion()
        {
            using (UnityWebRequest webRequest = CreateChatApiWebRequest())
            {

                yield return webRequest.SendWebRequest();
                HandleWebRequestResult(webRequest);
            }
            mCoroutine = null;
        }
    }
}
