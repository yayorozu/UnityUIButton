#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Yorozu.UI
{
	[CustomEditor(typeof(UniButton), false)]
	public class UniButtonEditor : Editor
	{
		private UniButton _base;
		private Type[] _types;
		private SerializedProperty _moduleProperty;
		private SerializedProperty _interactable;
		private Dictionary<Type, TypeInfo> _cacheInfos;

		private class TypeInfo
		{
			public int Index => _index;
			public bool HasComponent => _index >= 0;
			public UniButtonModuleAbstract Instance => _owner._base.Modules[_index];

			private readonly UniButtonEditor _owner;
			private readonly Type _type;

			private int _index;
			public readonly bool IsOverrideMethod;

			public TypeInfo(Type type, UniButtonEditor owner)
			{
				_type = type;
				_owner = owner;

				UpdateIndex();

				var method = type.GetMethod("DrawEditor", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
				if (method == null)
					IsOverrideMethod = false;
				else
					IsOverrideMethod = method == method.GetBaseDefinition();
			}

			public void UpdateIndex()
			{
				_index = -1;
				for (var i = 0; i < _owner._base.Modules.Length; i++)
				{
					if (_owner._base.Modules[i].GetType() != _type)
						continue;

					_index = i;
				}
			}

			public void Draw()
			{
				if (!HasComponent)
					return;

				_owner._moduleProperty.serializedObject.UpdateIfRequiredOrScript();
				DrawProperty(_owner._moduleProperty.GetArrayElementAtIndex(_index));
				_owner._moduleProperty.serializedObject.ApplyModifiedProperties();
			}

			private void DrawProperty(SerializedProperty property)
			{
				var depth = property.depth;
				var iterator = property.Copy();
				for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
				{
					if (iterator.depth < depth)
						return;

					depth = iterator.depth;

					using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
						EditorGUILayout.PropertyField(iterator, true);
				}
			}
		}

		protected void OnEnable()
		{
			_base = target as UniButton;

			// 必要タイプを取得
			_types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(UniButtonModuleAbstract)))
				.ToArray();

			_moduleProperty = serializedObject.FindProperty("_modules");
			_interactable = serializedObject.FindProperty("m_Interactable");

			_cacheInfos = new Dictionary<Type, TypeInfo>();
			foreach (var type in _types)
			{
				var info = new TypeInfo(type, this);
				_cacheInfos.Add(type, info);
			}
		}

		private void OnDestroy()
		{
			if (target != null)
				return;

			_cacheInfos.Clear();
		}

		public override void OnInspectorGUI()
		{
			if (_types == null || _types.Length <= 0)
				return;

			if (_moduleProperty == null)
				return;

			serializedObject.UpdateIfRequiredOrScript();

			EditorGUILayout.PropertyField(_interactable);

			serializedObject.ApplyModifiedProperties();

			GUILayout.Space(10);

			DrawModules();
		}

		private void DrawModules()
		{
			EditorGUILayout.HelpBox("Modules", MessageType.None);
			foreach (var type in _types)
				using (new GUILayout.VerticalScope("box"))
				{
					using (var check = new EditorGUI.ChangeCheckScope())
					{
						ToggleFoldout(type.Name, _cacheInfos[type].HasComponent);
						if (check.changed)
						{
							serializedObject.Update();
							var m = _base.Modules;
							if (_cacheInfos[type].HasComponent)
								ArrayUtility.RemoveAt(ref m, _cacheInfos[type].Index);
							else
								ArrayUtility.Add(ref m, (UniButtonModuleAbstract) Activator.CreateInstance(type));
							_base.Modules = m;

							foreach (var pair in _cacheInfos)
								pair.Value.UpdateIndex();

							serializedObject.ApplyModifiedProperties();
							GUIUtility.ExitGUI();
						}
					}

					if (_cacheInfos[type].IsOverrideMethod && _cacheInfos[type].Index >= 0)
						_cacheInfos[type].Instance.DrawEditor(_base, _moduleProperty.GetArrayElementAtIndex(_cacheInfos[type].Index));
					else
						_cacheInfos[type].Draw();
				}
		}

		private static void ToggleFoldout(string label, bool foldout)
		{
			var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 18);
			GUI.Toggle(rect, foldout, new GUIContent(label), new GUIStyle("ShurikenModuleTitle"));
			rect.y -= 1;
			rect.x += 1;
			GUI.Toggle(rect, foldout, GUIContent.none, EditorStyles.toggle);
		}
	}
}

#endif
