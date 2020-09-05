using System.Collections;
using System.Collections.Generic;
using Yorozu.Easing;
using UnityEngine;

namespace Yorozu.UI
{
	[CreateAssetMenu(fileName = "ButtonReactionData", menuName = Const.MENU_PATH + "UI/ButtonReactionData")]
	public class ButtonReactionData : ScriptableObject
	{
		[Range(0.1f, 1f)]
		public float ReactionTime = 0.3f;

		[Range(0.5f, 1.5f)]
		public float ReactionScale = 0.9f;

		public EaseType ReactionEase = EaseType.OutExpo;
		public EaseType ReturnEase = EaseType.InOutBack;

		public void Init()
		{
			ReactionTime = 0.3f;
			ReactionScale = 0.9f;
			ReactionEase = EaseType.OutExpo;
			ReturnEase = EaseType.InOutBack;
		}
	}
}