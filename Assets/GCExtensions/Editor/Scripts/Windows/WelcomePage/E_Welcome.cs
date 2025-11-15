#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GCGames.Editors
{
    public class E_Welcome : EditorWindow
    {
        GUISkin _skin = null;
        bool showGetStarted = true;
        bool showHelp = false;


        [MenuItem("Window/Reset/Reset Welcome Page")]
        public static void ResetWelcomePage()
        {
            AssetDatabase.DeleteAsset("Assets/GCExtensions/Resources/WelcomeStatus.asset");
        }

        [MenuItem("GC Extensions/Welcome Page", false, 300)]
        public static void CB_OpenWelcomePage()
        {
            EditorWindow window = GetWindow<E_Welcome>(true);
            window.maxSize = new Vector2(500, 346);
            window.minSize = window.maxSize;
        }

        private void OnEnable()
        {
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            titleContent = new GUIContent("Welcome To The GCExtensions");
        }

        private void OnGUI()
        {
            GUI.skin = _skin;
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), E_Colors.e_c_blue_3);
            EditorGUI.DrawRect(new Rect(0, 0, position.width, 80), E_Colors.e_c_blue_5);
            GUI.DrawTexture(new Rect(80, 15, 350, 51), E_Helpers.LoadImage(new Vector2(2048, 2048), E_Core.e_Title));

            EditorGUILayout.Space(85);

            EditorGUI.DrawRect(new Rect(0, 85, position.width, 333), E_Colors.e_c_blue_5);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Getting Started"))
            {
                showGetStarted = true;
                showHelp = false;
            }
            else if (GUILayout.Button("Additional Help"))
            {
                showGetStarted = false;
                showHelp = true;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            if (showGetStarted)
            {
                EditorGUILayout.LabelField("How Do I Use This Add-On?", GUI.skin.label);
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Inside the package you need to install PlayFab and TMP.", MessageType.Warning);
                EditorGUILayout.LabelField("0. To add extensions go to \"Gercam / Extensions\". In these extensions " +
                    "you will have prefabs, a test scene, a login menu and custom scripts.", GUI.skin.textArea);
                EditorGUILayout.LabelField("1. You will have scripts to verify if " +
                    "they have all the necessary components.", GUI.skin.textArea);
                EditorGUILayout.LabelField("2. You will have a menu where you can" +
                    " automatically add the prefabs for the login menu.", GUI.skin.textArea);
                EditorGUILayout.LabelField("3. You can clone the components of an object, to an array of objects. " +
                    "The values of the variables will be the same as the cloned one.", GUI.skin.textArea);
            }
            if (showHelp)
            {
                EditorGUILayout.LabelField("How Can I Get Help If I Get Stuck?", GUI.skin.label);
                EditorGUILayout.LabelField("The fastest way to get help is to join the Discord team and ask for help " +
                    "in the assets channel. Here is a link you can use to join the discord team: \n\n" +
                    "https://discord.gg/3B4fQw", GUI.skin.textArea);
                if(GUILayout.Button("Send Email"))
				{
                    Application.OpenURL("https://mail.google.com/mail/u/7/#inbox?compose=GTvVlcSMTtTcBqfHCwlkfSJJpWKTjgVGLmcsNQvJmXDhTCsZLxnVJfJzGJjxnpFxwvLPfKPpRjbCQ");
				}
                if (GUILayout.Button("Join Discord"))
                {
                    Application.OpenURL("https://discord.gg/3B4fQw");
                }
                if (GUILayout.Button("Youtbe"))
                {
                    Application.OpenURL("https://www.youtube.com/channel/UCCcDQmyJeWjE6qzev968GXQ?view_as=subscriber");
                }

            }
            if (E_WelcomeState.DisplayWelcomeScreen) { };
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif