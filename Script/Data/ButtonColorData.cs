using UnityEngine;

namespace Yorozu.UI
{
	/// <summary>
	/// ボタン色データ 
	/// </summary>
	[CreateAssetMenu(fileName = "ButtonColorData", menuName = "Yorozu/UI/ButtonColorData")]
	public class ButtonColorData : ScriptableObject
	{
		[SerializeField]
		private Color _normalColor = new Color(1f, 1f, 1f);
		
		[SerializeField]
		private Color _disabledColor = new Color(0.6f, 0.6f, 0.6f);
		
		[SerializeField]
		private Color _pressColor = new Color(0.8f, 0.8f, 0.8f);
		
		[SerializeField]
		private Color _selectedColor = new Color(1f, 1f, 1f);
		
		[SerializeField]
		private Color _highlightColor = new Color(1f, 1f, 1f);
		
		[SerializeField, Range(1f, 5f)]
		private float _colorMultiplier = 1f;
		
		[SerializeField, Range(0f, 5f)]
		private float _duration = 0.1f;

		internal float duration => _duration; 

		internal Color GetColor(YorozuButtonModule.SelectionState state)
		{
			var color = state switch
			{
				YorozuButtonModule.SelectionState.Highlighted => _highlightColor,
				YorozuButtonModule.SelectionState.Pressed => _pressColor,
				YorozuButtonModule.SelectionState.Selected => _selectedColor,
				YorozuButtonModule.SelectionState.Disabled => _disabledColor,
				_ => _normalColor
			};

			return color * _colorMultiplier;
		}
	}
}
