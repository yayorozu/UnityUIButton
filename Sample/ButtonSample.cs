using UnityEngine;
using Yorozu.UI;

public class ButtonSample : MonoBehaviour
{
	[SerializeField]
	private UniButton button;

	private void Awake()
	{
		button.SetClickAction(Click);
		if (button.TryGetModule<ButtonModuleLongClick>(out var module))
		{
			module.SetAction(LongClick);
		}
	}

	private void Click()
	{
		Debug.Log("Click");
	}

	private void LongClick()
	{
		Debug.Log("LongClick");
	}
}
