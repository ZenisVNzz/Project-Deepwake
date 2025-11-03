using UnityEditor;
using UnityEngine;


namespace GCGames.Editors
{
	public class GCAddExtensions : EditorWindow
	{
		GUISkin _skin = null;
		Color _titleColor;



		[MenuItem("GC Extensions/Extensions", false, 0)]
		private static void GC_Editor()
		{
			EditorWindow window = GetWindow<GCAddExtensions>(true);
			window.maxSize = new Vector2(500, 270);
			window.minSize = window.maxSize;
		}
		private void OnEnable()
		{
			if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

			//Make window title
			this.titleContent = new GUIContent("Extensions", null, "Steps to setup scripts");
		}

		private void OnGUI()
		{
			//Apply the gui skin
			GUI.skin = _skin;
			EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), E_Colors.e_c_blue_5);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Extensions - PlayFab", _skin.label);
			EditorGUILayout.Space();



			//Apply Body Title/Description
			EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));
			EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));
			EditorGUILayout.LabelField("PlayFab with TMP", _skin.label);
			EditorGUILayout.LabelField("Press the button to import the package.", _skin.textField);
			EditorGUILayout.HelpBox("Remember to install TMP.", MessageType.Warning);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(true));
			EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();


			// Add options buttons to create prefabs
			// Add PlayFab
			EditorGUILayout.BeginHorizontal();
			if (GUI.Button(new Rect(15, 175, 470, 30), "1. Add PlayFab.", _skin.button))
			{
				Application.OpenURL("https://docs.microsoft.com/es-es/gaming/playfab/sdks/unity3d/");
			}
			EditorGUILayout.EndHorizontal();


			// Add extensions
			EditorGUILayout.BeginHorizontal();
			if (GUI.Button(new Rect(15, 220, 470, 30), "2. Add Extensions for PlayFab", _skin.button))
			{
				if (EditorUtility.DisplayDialog("Add GCExtension.", "Are you sure you want to add the extensions? " +
						"Make sure you have the PlayFab and the TMP first", "Continue", "Cancel"))
				{
					AssetDatabase.ImportPackage(E_Core.e_importPackage, true);
				}
			}
			EditorGUILayout.EndHorizontal();


		}
	}
}
