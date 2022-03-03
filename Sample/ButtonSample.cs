using UnityEngine;
using Yorozu.UI;

public class ButtonSample : MonoBehaviour
{
	[SerializeField]
	private YorozuButton button;

	private void Awake()
	{
		button.SetClickEvent(Click);
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
