using System;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.UI
{
	[Serializable]
	public class ButtonModuleTransitionColor : YorozuButtonModule
	{
		[SerializeField]
		private Graphic[] _graphics = Array.Empty<Graphic>();

		[SerializeField]
		private ButtonColorData _data;
		
#if UNITY_EDITOR
		public override void EditorDrawer(YorozuButton button)
		{
			if (GUILayout.Button("Find Child Graphics"))
			{
				_graphics = button.GetComponentsInChildren<Graphic>();
			}
		}
#endif

		protected override void Prepare(SelectionState state)
		{
			if (_data == null)
			{
				_data = ScriptableObject.CreateInstance<ButtonColorData>();
			}
		}

		public override void DoStateTransition(SelectionState state, bool instant)
		{
			var color = _data.GetColor(state);
			foreach (var graphic in _graphics)
			{
				graphic.CrossFadeColor(color, instant ? 0f : _data.duration, true, true);
			}
		}
	}
}
