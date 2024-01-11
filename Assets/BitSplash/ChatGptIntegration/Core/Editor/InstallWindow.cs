using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    public class InstallWindow : EditorWindow
    {
        [SerializeField]
        int Selected = 0;
        [SerializeField]
        string serverAddress = "";
        static string[] Options = new string[] { "Local", "Server" }; 
        AuthFileData mFileData = new AuthFileData();
        ApiAuthenticationSettings mAuthSettings;
        string authFilePath;
        bool changed = false;

        [MenuItem("Tools/ChatGPT for Games/Install",priority =10)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<InstallWindow>(true,"Chat GPT Install Window");
        }

        private void OnEnable()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            authFilePath = $"{userPath}/.openai/auth.json";
            Load();
            if (mAuthSettings.CompletionUrl != ApiAuthenticationSettings.defaultUrl)
                Selected = 1;
            else
                Selected = 0;
        }
        private void OnDisable()
        {
            mAuthSettings = null;
        }
        ApiAuthenticationSettings LoadFromResource()
        {
            ApiAuthenticationSettings Authentication;

            Authentication = Resources.Load<ApiAuthenticationSettings>("DefaultChatGptApiAuthExt");
            if (Authentication == null)
                Authentication = Resources.Load<ApiAuthenticationSettings>("DefaultChatGptApiAuth");
            if (Authentication == null)
                Authentication = Resources.Load<ApiAuthenticationSettings>("GPTAuth");
            if (Authentication == null)
                Authentication = Resources.Load<ApiAuthenticationSettings>("ChatGPTForGames/GPTAuth");

            if (Authentication == null)
            {          
                Authentication = ScriptableObject.CreateInstance<ApiAuthenticationSettings>();
                string dir = "Assets/Resources/ChatGPTForGames";
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);
                AssetDatabase.CreateAsset(Authentication, $"{dir}/GPTAuth.asset");
                AssetDatabase.SaveAssets();
            }
            return Authentication;
        }
        void Load()
        {
            mAuthSettings = LoadFromResource();
            serverAddress = mAuthSettings.CompletionUrl;
            if(File.Exists(authFilePath) == false &&
                !String.IsNullOrEmpty(mAuthSettings.PrivateApiKey))
            {
                mFileData = new AuthFileData();
                mFileData.api_key = mAuthSettings.PrivateApiKey;
                mFileData.api_organization = mAuthSettings.Organization;
                Save();
                return;
            }
            try
            {
                string data = File.ReadAllText(authFilePath);
                mFileData = JsonUtility.FromJson<AuthFileData>(data);
            }
            catch(Exception)
            {
                mFileData = new AuthFileData();
            }
        }
        void Save()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dir = $"{userPath}/.openai";
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            string authPath = $"{dir}/auth.json";
            File.WriteAllText(authPath,JsonUtility.ToJson(mFileData));

        }
        private void OnGUI()
        {         
            GUILayout.Label("API Authentication", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            mFileData.api_key = EditorGUILayout.TextField("Api Secret Key", mFileData.api_key);
            mFileData.api_organization = EditorGUILayout.TextField("Organization", mFileData.api_organization);
            if (EditorGUI.EndChangeCheck())
                changed = true;
            if (changed)
            {
                if (GUILayout.Button("Save"))
                {
                    Save();
                    changed = false;
                }
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (File.Exists(authFilePath))
            {
                Selected = GUILayout.SelectionGrid(Selected, Options, 2);

                if (Selected == 1)
                {
                    serverAddress = EditorGUILayout.TextField(new GUIContent("server address"),serverAddress);
                }
                else
                {
                    EditorGUILayout.HelpBox("Include the api key in production builds at your own risk. Best way is to set up the included node js server", MessageType.Warning);
                }
                
                if (GUILayout.Button("Write Resource File"))
                {
                    if (Selected == 1) // server
                    {
                        mAuthSettings.PrivateApiKey = "";
                        mAuthSettings.Organization = "";
                        mAuthSettings.CompletionUrl = serverAddress;
                    }
                    else
                    {
                        mAuthSettings.PrivateApiKey = mFileData.api_key;
                        mAuthSettings.Organization = mFileData.api_organization;
                        mAuthSettings.CompletionUrl = ApiAuthenticationSettings.defaultUrl;
                    }
                    EditorUtility.SetDirty(mAuthSettings);
                    //string path = AssetDatabase.GetAssetPath(mAuthSettings);
                    AssetDatabase.SaveAssets();
                    EditorUtility.DisplayDialog("GPT for Games","Resource file written","ok");
                }
            }
        }
    }
}
