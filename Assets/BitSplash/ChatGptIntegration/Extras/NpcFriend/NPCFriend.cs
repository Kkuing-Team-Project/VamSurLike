using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitSplash.AI.GPT.Extras
{
    public class NPCFriend : MonoBehaviour
    {
        public TMP_InputField QuestionField;
        public TMP_Text AnswerField;
        public Button SubmitButton;

        public string NpcDirection = "Answer as a helpful Ninjitsu master of stealth";
        public string[] Facts;
        public bool TrackConversation = false;
        public int MaximumTokens = 600;
        [Range(0f, 1f)]
        public float Temperature = 0f;
        ChatGPTConversation Conversation;

        void Start()
        {
            SetUpConversation();
        }
        void SetUpConversation()
        {
            Conversation = ChatGPTConversation.Start(this)
                .MaximumLength(MaximumTokens)
                .SaveHistory(TrackConversation)
                .System(string.Join("\n", Facts) + "\n" + NpcDirection);
            Conversation.Temperature = Temperature;
        }
        public void SendClicked()
        {
            Conversation.Say(QuestionField.text);
            SubmitButton.interactable = false;
        }

        void OnConversationResponse(string text)
        {
            AnswerField.text = text;
            SubmitButton.interactable = true;
        }
        void OnConversationError(string text)
        {
            Debug.Log("Error : " + text);
            Conversation.RestartConversation();
            AnswerField.text = "Sorry , I Got confused for a moment, what did you just ask ?";
            SubmitButton.interactable = true;
        }

        private void OnValidate()
        {
            SetUpConversation();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}