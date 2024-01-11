using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace BitSplash.AI.GPT
{
    public abstract class ChatGPTConversation
    {
        /// <summary>
        /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic
        /// We generally recommend altering this or top_p but not both
        /// </summary>
        public float Temperature = 0.7f;
        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered
        /// We generally recommend altering this or temperature but not both.
        /// </summary>
        public float Top_P = 1f;
        /// <summary>
        /// The maximum number of tokens allowed for the generated answer. By default, the number of tokens the model can return will be (4096 - prompt tokens).
        /// </summary>
        public int Maximum_Length = 600;
        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
        /// </summary>
        public float Presence_Penalty = 0f;
        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        public float Frequency_Penalty = 0f;

        protected List<ChatApiMessage> mHistory = new List<ChatApiMessage>();
        ChatApiMessage mSystemMessage = ChatApiMessage.System("You are a helpful assistant");
        public bool mSaveHistory = false;

        ApiAuthenticationSettings Authentication;
        public ChatGPTConversation()
        {
            Authentication =  Resources.Load<ApiAuthenticationSettings>("DefaultChatGptApiAuthExt");
            if (Authentication == null)
                Authentication = Resources.Load<ApiAuthenticationSettings>("DefaultChatGptApiAuth");
            if(Authentication == null)
                Authentication = Resources.Load<ApiAuthenticationSettings>("GPTAuth");
            if (Authentication == null)
                Authentication = Resources.Load<ApiAuthenticationSettings>("ChatGPTForGames/GPTAuth");
            if (Authentication == null)
                throw new Exception("Please set up authentication by editing the object GPTAuth. see the docs for more info");
        }
        /// <summary>
        /// starts a new converstation for this monobehaviour
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ChatGPTConversation Start(MonoBehaviour b)
        {
            return new ChatConversationRuntime(b);
        }
        /// <summary>
        /// sets the maximum token length per request
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public ChatGPTConversation MaximumLength(int length)
        {
            Maximum_Length = length;
            return this;
        }
        /// <summary>
        /// set to true to save the history of the conversation between requests. This will make chat gpt , remember your previous messages
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        public ChatGPTConversation SaveHistory(bool save)
        {
            mSaveHistory = save;
            return this;
        }
        /// <summary>
        /// cancel any pending request
        /// </summary>
        public void Cancel()
        {
            if(StopWebRequest())
            {
                RemoveLastIfUser();
            }
        }
        /// <summary>
        /// set the system prompt for this chat gpt conversation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ChatGPTConversation System(string message)
        {
            mSystemMessage.content = message;
            return this;
        }
        /// <summary>
        /// say something to chat gpt
        /// </summary>
        /// <param name="message"></param>
        public void Say(string message)
        {
            Cancel();
            if (mSaveHistory == false)
                RestartConversation();
            else
                RemoveLastIfUser(); // make sure there are no consecutive user messages without a system message between them
            mHistory.Add(ChatApiMessage.User(message));
            StartWebRequest();           
        }
        [Serializable]
        class SavedArray
        {
            public SavedArray(ChatApiMessage[] history)
            {
                History = history;
            }
            public ChatApiMessage[] History;
        }

        public void Save(string name)
        {
            name = $"{name}.convo";
            string file = Path.Combine(Application.persistentDataPath, name);
            SavedArray arr = new SavedArray(mHistory.ToArray());
            File.WriteAllText(file, JsonUtility.ToJson(arr));
        }
        public bool Load(string name)
        {
            Cancel();
            RestartConversation();
            name = $"{name}.convo";
            string file = Path.Combine(Application.persistentDataPath, name);
            try
            {
                SavedArray arr= JsonUtility.FromJson<SavedArray>(File.ReadAllText(file));
                mHistory = new List<ChatApiMessage>(arr.History);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
        protected ChatApiRequest CreateChatRequest()
        {
            ChatApiRequest req = new ChatApiRequest();
            req.model = ModelNames.GetModelName(Authentication.Model);
            req.messages = mHistory.Prepend(mSystemMessage).ToArray();
            req.frequency_penalty = Frequency_Penalty;
            req.presence_penalty = Presence_Penalty;
            req.n = 1;
            req.temperature = Temperature;
            req.top_p = Top_P;
            req.max_tokens = Maximum_Length;
            return req;
        }
        void RemoveLastIfUser()
        {
            if (mHistory.Count > 0 && mHistory[mHistory.Count-1].role == ChatApiMessage.RoleUser)
                mHistory.RemoveAt(mHistory.Count - 1);
        }

        protected virtual void OnError(long code, string message)
        {
            RemoveLastIfUser();
        }

        protected virtual void OnResponse(ChatApiResponse response)
        {

        }

        protected UnityWebRequest CreateChatApiWebRequest()
        {
#if UNITY_2023_1_OR_NEWER
            var webRequest = UnityWebRequest.PostWwwForm(Authentication.CompletionUrl, string.Empty);
#else
            var webRequest = UnityWebRequest.Post(Authentication.CompletionUrl, string.Empty);
#endif
            webRequest.SetRequestHeader("Authorization", $"Bearer {Authentication.PrivateApiKey}");
            //webRequest.SetRequestHeader("User-Agent", "Bitsplash/ChatGpt");
            if (!string.IsNullOrEmpty(Authentication.Organization.Trim()))
                webRequest.SetRequestHeader("OpenAI-Organization", Authentication.Organization);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            var req = CreateChatRequest();
            string jsonData = JsonUtility.ToJson(req);
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            return webRequest;
        }

        protected void HandleWebRequestResult(UnityWebRequest webRequest)
        {
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    OnError(0, webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    OnError(webRequest.responseCode, webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string text = webRequest.downloadHandler.text;
                    var response = JsonUtility.FromJson<ChatApiResponse>(text);
                    OnResponse(response);
                    break;
            }
        }

        protected abstract bool StopWebRequest();
        protected abstract void StartWebRequest();
        /// <summary>
        /// delete all history of the conversation.
        /// </summary>
        public void RestartConversation()
        {
            mHistory.Clear();
        }
    }
}
