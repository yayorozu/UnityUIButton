using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yorozu.UI
{
	/// <summary>
	/// TextMeshPro の Material はフェードできないので押したタイミングで切り替わる
	/// </summary>
	[Serializable]
	public class ButtonModuleTransitionTextMeshProMaterial : YorozuButtonModule
	{
		[SerializeField]
		private TextMeshProUGUI[] _texts = Array.Empty<TextMeshProUGUI>();
		[SerializeField]
		private ButtonTMProOutlineData _data;
		
		public override void DoStateTransition(SelectionState state, bool instant)
		{
			if (_data == null)
				return;
			
			var material = _data.GetMaterial(state);
			foreach (var text in _texts)
			{
				text.fontMaterial = material;
			}
		}
	}
}
