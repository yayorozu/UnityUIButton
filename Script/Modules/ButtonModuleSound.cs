using System;
using UnityEngine;

namespace Yorozu.UI
{
	[Serializable]
	public class ButtonModuleSound : YorozuButtonModule
	{
		[SerializeField]
		private string _key;
		internal string Key => _key;
	}
}
