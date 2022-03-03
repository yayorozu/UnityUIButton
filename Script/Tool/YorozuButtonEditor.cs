#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Yorozu.UI
{
	[CustomEditor(typeof(YorozuButton), false)]
	internal partial class YorozuButtonEditor : ButtonEditor
	{
		private YorozuButton _button;
		private Type[] _types;
		private Dictionary<Type, ModuleType> _cacheInfos;
		
		private SerializedProperty _moduleProperty;
		private SerializedProperty _interactable;
		private SerializedProperty _navigation;
		
		private GUIContent _visualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

		private FieldInfo _showNavigationField;
		private FieldInfo _showNavigationKeyField;
		
		private bool _showNavigation;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			_button = target as YorozuButton;
			
			_showNavigationField = typeof(SelectableEditor).GetField("s_ShowNavigation", BindingFlags.NonPublic | BindingFlags.Static);
			_showNavigationKeyField = typeof(SelectableEditor).GetField("s_ShowNavigationKey", BindingFlags.NonPublic | BindingFlags.Static);

			// 必要タイプを取得
			_types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(YorozuButtonModule)))
				.ToArray();

			// モジュールのクラス名を変えた場合参照がおかしくなる
			_moduleProperty = serializedObject.FindProperty("_modules");
			_interactable = serializedObject.FindProperty("m_Interactable");
			_navigation = serializedObject.FindProperty("m_Navigation");

			_cacheInfos = new Dictionary<Type, ModuleType>();
			foreach (var type in _types)
			{
				var info = new ModuleType(type, this);
				_cacheInfos.Add(type, info);
			}

			_showNavigation = (bool) _showNavigationField.GetValue(null);
		}

		private void OnDestroy()
		{
			if (target != null)
				return;

			_cacheInfos.Clear();
		}

		public override void OnInspectorGUI()
		{
			//base.OnInspectorGUI();
			serializedObject.UpdateIfRequiredOrScript();
			
			EditorGUILayout.PropertyField(_interactable);
			EditorGUILayout.PropertyField(_navigation);
			EditorGUI.BeginChangeCheck();
			var toggleRect = EditorGUILayout.GetControlRect();
			toggleRect.xMin += EditorGUIUtility.labelWidth;
			
			_showNavigation = GUI.Toggle(toggleRect, _showNavigation, _visualizeNavigation, EditorStyles.miniButton);
			if (EditorGUI.EndChangeCheck())
			{
				var key = (string)_showNavigationKeyField.GetValue(null);
				_showNavigationField.SetValue(null, _showNavigation);
				EditorPrefs.SetBool(key, _showNavigation);
				SceneView.RepaintAll();
			}

			serializedObject.ApplyModifiedProperties();

			if (_types == null || _types.Length <= 0)
				return;

			if (_moduleProperty == null)
				return;
			
			GUILayout.Space(10);

			DrawModules();
		}

		private void DrawModules()
		{
			using (new GUILayout.VerticalScope("helpbox"))
			{
				EditorGUILayout.LabelField("Modules", EditorStyles.boldLabel);
			}
			
			foreach (var type in _types)
			{
				// データがなにかしらでなくなっている場合は再取得
				if (!_cacheInfos.ContainsKey(type))
				{
					OnEnable();
					GUIUtility.ExitGUI();
				}

				using (new GUILayout.VerticalScope("box"))
				{
					using (var check = new EditorGUI.ChangeCheckScope())
					{
						var label = type.Name.Replace("ButtonModule", "").Replace("YorozuButton", "");
						ToggleFoldout(label, _cacheInfos[type].HasComponent);
						if (check.changed)
						{
							serializedObject.Update();
							var m = _button.EditorModules;
							if (_cacheInfos[type].HasComponent)
								ArrayUtility.RemoveAt(ref m, _cacheInfos[type].Index);
							else
								ArrayUtility.Add(ref m, (YorozuButtonModule) Activator.CreateInstance(type));
							_button.EditorModules = m;

							foreach (var pair in _cacheInfos)
								pair.Value.UpdateIndex();

							serializedObject.ApplyModifiedProperties();
							GUIUtility.ExitGUI();
						}
					}

					using (new EditorGUI.IndentLevelScope())
					{
						_cacheInfos[type].Draw();
					}
				}
			}
		}

		private void ToggleFoldout(string label, bool foldout)
		{
			var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 17);
			GUI.Toggle(rect, foldout, new GUIContent(label), new GUIStyle("ShurikenModuleTitle"));
			rect.y -= 1;
			rect.x += 1;
			GUI.Toggle(rect, foldout, GUIContent.none, EditorStyles.toggle);
		}
	}
}

#endif
