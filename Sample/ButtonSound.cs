using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Yorozu.UI;

namespace Sample
{
	[Serializable]
	public class ButtonSound : YorozuButtonModuleAbstract
	{
		public override void Press()
		{
			Debug.Log("PlaySound");
		}
	}
}
