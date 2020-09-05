using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Yorozu.UI;

namespace Sample
{
	[Serializable]
	public class ButtonSound : UniButtonModuleAbstract
	{
		public override void Press()
		{
			Debug.Log("PlaySound");
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
		}
	}
}
