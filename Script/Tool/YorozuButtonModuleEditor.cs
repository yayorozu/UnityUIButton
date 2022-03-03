#if UNITY_EDITOR

using System;
using UnityEditor;

namespace Yorozu.UI
{
	internal partial class YorozuButtonEditor
	{
		private class ModuleType
		{
			internal int Index => _index;
			internal bool HasComponent => _index >= 0;
			private YorozuButtonModule Instance => _owner._button.EditorModules[_index];

			private readonly YorozuButtonEditor _owner;
			private readonly Type _type;

			private int _index;

			internal ModuleType(Type type, YorozuButtonEditor owner)
			{
				_type = type;
				_owner = owner;

				UpdateIndex();
			}

			internal void UpdateIndex()
			{
				_index = -1;
				for (var i = 0; i < _owner._button.EditorModules.Length; i++)
				{
					if (_owner._button.EditorModules[i].GetType() != _type)
						continue;

					_index = i;
				}
			}

			internal void Draw()
			{
				if (!HasComponent)
					return;

				_owner._moduleProperty.serializedObject.UpdateIfRequiredOrScript();
				DrawProperty(_owner._moduleProperty.GetArrayElementAtIndex(_index));
				
				if (_index >= 0)
					Instance.EditorDrawer(_owner._button);

				_owner._moduleProperty.serializedObject.ApplyModifiedProperties();
			}

			private void DrawProperty(SerializedProperty property)
			{
				if (!property.hasVisibleChildren)
					return;

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
	}
}

#endif