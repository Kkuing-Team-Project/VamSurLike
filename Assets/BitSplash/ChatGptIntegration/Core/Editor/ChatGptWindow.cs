using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    class ChatGptWindow : ChatGptWindowBase
    {
        const string DefaultSystem = "You are a helpful expert unity developer";
        const string DefaultDocument = "Add comments to the following code. Answer in code only";
        const string DefaultDebug = "Debug the following code";

        string mPromptText = "";
        AnswerString[] mResultText = new AnswerString[0];
        AnswerString Dragging;
        bool mWorking = false;
        Vector2 scrollPosition;
        GUIStyle richTextArea;
        GUIStyle answerTextArea;
        GUIStyle promptField;
        bool TrackConversation = true;
        int mPrevTime = 0;
        bool Droping = false;
        string mPropmtCodePath = null;
        StringBuilder mPromptBuilder = new StringBuilder();
        int mFileSelection = 0;
        string[] mFileOptions = new string[] { "Ask", "Debug", "Document" };
        string[] mSystemOptions = new string[] { DefaultSystem, DefaultDebug, DefaultDocument };
        bool mErrorDialogShown = false;

        [MenuItem("Tools/ChatGPT for Games/Chat GPT window")]
        public static void ShowWindow()
        {
            var win = EditorWindow.GetWindow<ChatGptWindow>("Chat GPT");
            win.mErrorDialogShown = false;
        }

        void Update()
        {
            if (Time.time != mPrevTime)
            {
                mPrevTime = (int)Time.time;
                Repaint();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable(); 
        }
        GUIStyle PromptField
        {
            get
            {
                if(promptField == null)
                {
                    promptField = new GUIStyle(EditorStyles.textArea);
                    promptField.fontSize = 14;
                    promptField.wordWrap = true;
                    
                }
                return promptField;
            }
        }
        GUIStyle AnswerTextArea
        {
            get
            {
                if(answerTextArea == null)
                {
                    answerTextArea = new GUIStyle(EditorStyles.wordWrappedLabel);
                    answerTextArea.fontSize = 14;
                }
                return answerTextArea;
            }
        }
        GUIStyle RichTextArea
        {
            get
            {
                if (richTextArea == null)
                { 
                    richTextArea = new GUIStyle(EditorStyles.textArea);
                    richTextArea.wordWrap = false;
                    richTextArea.fontSize = 14;
                    richTextArea.richText = true;  
                }
                return richTextArea;
            }
        }
        
        private void SendPrompt(string prompt,string system)
        {
            mPromptBuilder.Clear();
            mPromptBuilder.Append(prompt);
            if (!String.IsNullOrEmpty(mPropmtCodePath) ) 
            {
                try
                {
                    string text = File.ReadAllText(mPropmtCodePath);
                    mPromptBuilder.Append($"\n```{text}```");
                }
                catch(Exception)
                {
                    EditorUtility.DisplayDialog("Chat GPT For Games", "File not found, check if it was moved or deleted", "Ok");
                    return;
                }
            }
            mWorking = true;
            Conversation.System(system);
            Conversation.Say(mPromptBuilder.ToString());
        }
        private void ConversationPanel()
        {
            EditorGUILayout.BeginHorizontal();
            if (mWorking == false)
            {
                if (GUILayout.Button("Submit"))
                {
                    if (!String.IsNullOrEmpty(mPropmtCodePath))
                        SendPrompt(mPromptText, mSystemOptions[mFileSelection]);
                    else
                        SendPrompt(mPromptText, DefaultSystem);
                }
            }
            else
            {
                //GUI.enabled = false;
                int numDots = (((int)EditorApplication.timeSinceStartup) % 4);
                if (GUILayout.Button("Cancel" + new string('.', numDots)))
                {
                    Conversation.Cancel();
                    mWorking = false;
                }
                //GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal(); 
        }
        
        private void ShowCode(AnswerString str)
        {
            EditorGUI.indentLevel++;
            Rect r = EditorGUILayout.GetControlRect();
            EditorGUI.HelpBox(r, $"{str.CodeType} - Drag to project window", MessageType.Info);

            Color c = GUI.backgroundColor;
            //str.scroll = EditorGUILayout.BeginScrollView(str.scroll);
            EditorGUILayout.TextArea(str.HighlightCode, RichTextArea);
            //EditorGUILayout.EndScrollView(); 
            if (Event.current.type == EventType.MouseDrag &&
                    r.Contains(Event.current.mousePosition))
            {
                Dragging = str;
                StartDrag();
            }
            EditorGUI.indentLevel--;
        }
        void ShowDroppingLayout()
        {
            EditorGUILayout.BeginHorizontal();
            var size = EditorStyles.centeredGreyMiniLabel.fontSize;
            var fixedHeight = EditorStyles.centeredGreyMiniLabel.fixedHeight;
            EditorStyles.centeredGreyMiniLabel.fontSize = 14;
            EditorStyles.centeredGreyMiniLabel.fixedHeight = 18;
            EditorGUILayout.LabelField("Drop File Here", EditorStyles.centeredGreyMiniLabel);
            EditorStyles.centeredGreyMiniLabel.fontSize = size;
            EditorStyles.centeredGreyMiniLabel.fixedHeight = fixedHeight;
            EditorGUILayout.EndHorizontal();
        }
        void ShowPromptPanel()
        {
            bool remove = false;
            if (!String.IsNullOrEmpty(mPropmtCodePath))
            {
                Conversation.SaveHistory(false);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("File:", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(Path.GetFileName(mPropmtCodePath), EditorStyles.largeLabel, GUILayout.ExpandWidth(true));
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("Remove"))
                {
                    remove = true;
                    mPropmtCodePath = null;
                    mResultText = new AnswerString[0]; 
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                int prevSelection = mFileSelection;
                mFileSelection = GUILayout.SelectionGrid(mFileSelection, mFileOptions, mFileOptions.Length);
                if (prevSelection != mFileSelection)
                    mPromptText = "";
                EditorGUILayout.Space();
                if (mFileSelection == 0)
                    mPromptText = EditorGUILayout.TextArea(mPromptText, PromptField);
                else
                {
                    mPromptText = mSystemOptions[mFileSelection];
                    GUI.FocusControl("");
                }
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Prompt:", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                TrackConversation = GUILayout.Toggle(TrackConversation, "Track Messages");
                Conversation.SaveHistory(TrackConversation);
                EditorGUILayout.EndHorizontal();
                mPromptText = EditorGUILayout.TextArea(mPromptText, PromptField);
            }
            EditorGUILayout.Space();
            if (remove == true)
                mPromptText = "";
        }

        private void OnGUI()
        {
            if(Conversation == null)
            {
                if (mErrorDialogShown == false)
                {
                    EditorUtility.DisplayDialog("Chat GPT For Games", "You must setup authentication first. see the guide for more information", "Ok");
                    mErrorDialogShown = true;
                }
                //EditorGUILayout.LabelField("You must setup authentication first. see the guide for more information");
                Close();
                return;
            }
            HandleScriptDrop();

            EditorGUILayout.BeginVertical();
            if (Droping)
            {
                ShowDroppingLayout();
            }
            ShowPromptPanel();   
            ConversationPanel();
            if (mWorking)
                GUI.enabled = false;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for(int i=0; i<mResultText.Length; i++)
            {
                AnswerString str = mResultText[i];
                if (str.IsCode)
                    ShowCode(str);
                else
                    EditorGUILayout.TextArea(str.Text, AnswerTextArea);
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            if (Event.current.type == EventType.DragUpdated)
            {
                // Indicate that we don't accept drags ourselves

                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
            GUI.enabled = true;
        }

        void HandleScriptDrop()
        {
            if(Event.current.type == EventType.DragExited)
            {
                Droping = false;
                Event.current.Use();
            }
            if(Event.current.type == EventType.DragUpdated)
            {
                Droping = false;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    if (Path.GetExtension(DragAndDrop.paths[0]) == ".cs")
                    {
                        Droping = true;
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    }
                }
                if (Droping == false)
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                Event.current.Use();
            }
            if (Event.current.type == EventType.DragPerform)
            {
                Droping = false;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    mPropmtCodePath = DragAndDrop.paths[0];
                    mPromptText = "";
                    mResultText = new AnswerString[0];
                }
                Event.current.Use();
            }
        }
        private void StartDrag()
        {
            
            DragAndDrop.PrepareStartDrag();

            // Clear anything we don't use, else we might get weird behaviour when dropping on
            // some other control

            DragAndDrop.paths = null;
            DragAndDrop.objectReferences = new UnityEngine.Object[] { this };


            DragAndDrop.StartDrag("Create Code");

            DragAndDrop.RemoveDropHandler(ProjectHandler);
            DragAndDrop.AddDropHandler(ProjectHandler);

        }
        static void DropOnPath(string path,AnswerString code)
        {
            string className = CodeExtractor.getClassName(code.Text);
            string fileType = code.CodeType.ToLower();
            if (fileType == "csharp" || fileType == "c#")
                fileType = "cs";
            if (Path.HasExtension(path) && Path.GetExtension(path) == $".{fileType}")
            {
                if (!EditorUtility.DisplayDialog("Chat GPT For Games", "Would you like to overwrite the script file?", "Yes", "No"))
                    return;
                File.WriteAllText(path, code.Text);
                return;
            }
            string dir = path;
            if(Path.HasExtension(dir))
                dir = Path.GetDirectoryName(path);
            fileType = $".{fileType}";
            if (className == null)
                className = "GeneratedScript";
            string fullFileName = Path.Combine(dir, $"{className}{fileType}");
            int i = 2;
            while (File.Exists(fullFileName))
            {
                fullFileName = Path.Combine(dir, $"{className}{i}{fileType}");
                ++i;
            }
            File.WriteAllText(fullFileName, code.Text);
            AssetDatabase.ImportAsset(fullFileName);
        }
        static DragAndDropVisualMode ProjectHandler(int id, string path, bool perform)
        {
            if (DragAndDrop.objectReferences == null ||
                DragAndDrop.objectReferences.Length == 0 ||
                !(DragAndDrop.objectReferences[0] is ChatGptWindow))
                return DragAndDropVisualMode.None;
            if (perform)
            {
                var gptWindow = (ChatGptWindow)(DragAndDrop.objectReferences[0]);
                DragAndDrop.objectReferences = new UnityEngine.Object[0];
                if (gptWindow.Dragging != null)
                {
                    DropOnPath(path, gptWindow.Dragging);
                    gptWindow.Dragging = null;
                    
                }
                else
                    return DragAndDropVisualMode.None;
                //AssetDatabase.create
            }
           // else
           //     Debug.Log($"Dragging upon {path} {id}");
            return DragAndDropVisualMode.Move;
        }

        public override void OnConversationError(string text)
        {
            base.OnConversationError(text);
            Conversation.RestartConversation();
            Debug.Log(text);
            mResultText = new AnswerString[] { new AnswerString(false,$"Error:{text}\n\n Conversatoin restarted ", text) };
            mWorking = false;
            Repaint();
        }
        public override void OnConversationResponse(string text)
        {
            base.OnConversationResponse(text);
            mResultText = CodeExtractor.ExtractCode(text).ToArray();
            if(mFileSelection == 2) // answer in code only
            {
                if(mResultText.Length == 1)
                {
                    AnswerString str = mResultText[0];
                    str.IsCode = true;
                    str.CodeType = "csharp";
                }
            }
            foreach(AnswerString answer in mResultText)
            {
                if(answer.IsCode)
                {
                    answer.HighlightCode = Highlight.Highlight("C#", answer.Text);
                }
                else
                {
                  //  answer.Text = answer.Text.Replace("\n", "\n\n");
                }
            }
            mWorking = false;
            Repaint();
        }

    }
}
