using Boo.Lang;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace GCGames.Editors
{
	public class GCEditorStatus : EditorWindow
	{
		GeneralArray listClone;
		Vector2 _scrollPos;
		GUISkin _skin = null;
		GameObject PlayerPrefab = null;
		public Component[] listScripts;
		int clear = 0;

		[MenuItem("GC Extensions/Check Components", false, 100)]
		private static void ZRC_EditorStatus()
		{
			EditorWindow window = GetWindow<GCEditorStatus>(true);
			window.maxSize = new Vector2(500, 600);
			window.minSize = window.maxSize;
		}
		private void OnEnable()
		{
			if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

			//Make window title
			this.titleContent = new GUIContent("Check Components", null, "Steps to setup scripts");
		}

		private void OnGUI()
		{
			GCColorHolder _original = new GCColorHolder(EditorStyles.label);
			GCColorHolder _originalFold = new GCColorHolder(EditorStyles.foldout);
			GCColorHolder _skinHolder = new GCColorHolder(_skin.label);
			//Apply the gui skin
			GUI.skin = _skin;
			Color norm = _skin.textField.normal.textColor;
			EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), E_Colors.e_c_blue_5);
			EditorGUI.DrawRect(new Rect(5, 5, position.width - 10, 40), E_Colors.e_c_blue_4);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Check Components in Object", _skin.label);
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(_skin.label, GUILayout.ExpandHeight(true));
			EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));


			GCEditor.SetColorToEditorStyle(_skinHolder, _skinHolder);
			PlayerPrefab = EditorGUI.ObjectField(new Rect(5, 50, 375, 16), "Game Object:", PlayerPrefab, typeof(GameObject), true) as GameObject;
			GCEditor.SetColorToEditorStyle(_original, _originalFold);

			EditorGUILayout.Space(25);






			if (PlayerPrefab != null)
			{
				EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));
				EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));


				_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(500));
				ScriptableObject scriptableObj = this;
				SerializedObject serialObj = new SerializedObject(scriptableObj);
				SerializedProperty serialProp = serialObj.FindProperty("listScripts");

				EditorGUILayout.PropertyField(serialProp, true);
				serialObj.ApplyModifiedProperties();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("1. Check", _skin.button))
				{
					CheckComponentes();

				}
				if (GUILayout.Button("2. Clone", _skin.button))
				{
					GeneralArray.OpenWindow();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("3. Clear", _skin.button))
				{
					Clear();
				}
				if (GUILayout.Button("4. Clear All", _skin.button))
				{
					if (EditorUtility.DisplayDialog("Clear All components.", "Are you sure you want to delete " +
						"all the components of this object?", "Continue", "Cancel"))
					{
						ClearAllList();
					}


				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				clear = EditorGUILayout.IntField("Remove Element Num: ", clear, _skin.customStyles[1]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndScrollView();

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();

			}




			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

		}

		void CheckComponentes()
		{
			EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));
			listScripts = new Component[PlayerPrefab.GetComponents<Component>().Length];

			for (int i = 0; i < listScripts.Length; i++)
			{
				listScripts[i] = PlayerPrefab.GetComponents<Component>()[i];
			}
			EditorGUILayout.EndHorizontal();

		}
		void Clear()
		{
			List<Component> result = new List<Component>();
			foreach (Component com in listScripts)
			{
				result.Add(com);
			}

			result.RemoveAt(clear);
			Debug.Log("You have destroyed the component: " + listScripts.GetValue(clear));
			Component remove = (Component)listScripts.GetValue(clear);
			result.ToArray(listScripts);
			DestroyImmediate(remove);

		}
		void ClearAllList()
		{
			foreach (Component comp in PlayerPrefab.GetComponents<Component>())
			{
				if (!(comp is Transform))
				{
					DestroyImmediate(comp);
				}
			}
		}

	}








	public class GeneralArray : EditorWindow
	{
		public Component[] copy;
		public Component[] components;
		public bool[] isComponents = new bool[1];
		GUISkin _skin = null;
		public GameObject[] ObjectToClone = null;
		public GameObject ClonerObject = null;
		Vector2 _scrollPos;
		Vector2 _scrollPos2 = new Vector2(350, 300);


		[MenuItem("GC Extensions/Clone Components", false, 100)]
		public static void OpenWindow()
		{
			EditorWindow window = GetWindow<GeneralArray>(true);
			window.maxSize = new Vector2(400, 630);
			window.minSize = window.maxSize;
		}

		private void OnEnable()
		{
			if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

			//Make window title
			this.titleContent = new GUIContent("List Components", null, "Steps to setup scripts");
		}


		private void OnGUI()
		{
			//Apply the gui skin
			GUI.skin = _skin;
			Color norm = _skin.textField.normal.textColor;
			EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), E_Colors.e_c_blue_5);
			EditorGUI.DrawRect(new Rect(5, 5, position.width - 10, 40), E_Colors.e_c_blue_4);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Check Components to Clone", _skin.label);
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Objects to Clone", _skin.button))
			{
				AddObjectToClone();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(_skin.label);
			EditorGUILayout.BeginVertical(_skin.box);



			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(150));

			ScriptableObject scriptableObj = this;
			SerializedObject serialObj = new SerializedObject(scriptableObj);
			SerializedProperty serialProp = serialObj.FindProperty("ObjectToClone");
			EditorGUILayout.PropertyField(serialProp, true);
			serialObj.ApplyModifiedProperties();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal(_skin.label);
			EditorGUILayout.BeginVertical(_skin.box);

			ClonerObject = EditorGUILayout.ObjectField("Cloner Object:", ClonerObject, typeof(GameObject), true) as GameObject;

			if (ClonerObject != null)
			{

				SetList();

				if (components.GetLength(0) != isComponents.GetLength(0))
				{
					isComponents = new bool[components.Length];
				}
				EditorGUILayout.LabelField("Components", _skin.label);

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Select All Components", _skin.button))
				{
					SelectAllComponents();
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(_skin.label);
				EditorGUILayout.BeginVertical(_skin.box);
				_scrollPos2 = EditorGUILayout.BeginScrollView(_scrollPos2, GUILayout.Height(200));

				for (int i = 0; i < components.Length; i++)
				{
					EditorGUILayout.BeginVertical();
					isComponents[i] = EditorGUILayout.Toggle(components[i].GetType().ToString(), isComponents[i], _skin.toggle);
					EditorGUILayout.EndVertical();
					if (isComponents[i])
					{

					}
					else
					{

					}
				}
				EditorGUILayout.EndScrollView();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("1. Copy Components", _skin.button))
				{
					CopyComponentes();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("2. Paste Components", _skin.button))
				{
					if (EditorUtility.DisplayDialog("Clone Scripts.", "Are you sure you want to clone all the components. " +
						"The components will be with the default values. " +
						"After cloning you can add the component presets.", "Continue", "Cancel"))
					{
						Paste();
					}


				}

				EditorGUILayout.EndHorizontal();

			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

		}
		#region Voids
		void SetList()
		{
			components = new Component[ClonerObject.GetComponents<Component>().Length];

			for (int i = 0; i < components.Length; i++)
			{
				components[i] = ClonerObject.GetComponents<Component>()[i];
			}
		}
		private void SelectAllComponents()
		{
			for (int i = 0; i < components.Length; i++)
			{
				isComponents[i] = true;
			}
		}

		private void AddObjectToClone()
		{
			ObjectToClone = new GameObject[Selection.gameObjects.Length];

			for (int i = 0; i < ObjectToClone.Length; i++)
			{
				ObjectToClone[i] = Selection.gameObjects[i];
			}
		}

		private void CopyComponentes()
		{
			copy = new Component[isComponents.Length];
			bool[] arrayBool = new bool[isComponents.Length];
			for (int i = 0; i < arrayBool.Length; i++)
			{
				arrayBool[i] = isComponents[i];
				if (arrayBool[i] == true)
				{
					copy[i] = ClonerObject.GetComponents<Component>()[i];
				}
			}
		}


		void Paste()
		{

			foreach (var targetGameObject in ObjectToClone)
			{
				if (!targetGameObject || copy == null) continue;
				foreach (var copiedComponent in copy)
				{
					if (!copiedComponent) continue;
					UnityEditorInternal.ComponentUtility.CopyComponent(copiedComponent);
					UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGameObject);
				}
			}

		}
		#endregion
	}
}
