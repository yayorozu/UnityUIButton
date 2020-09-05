using UnityEngine;
using Yorozu.UI;
#if UNITY_EDITOR
using Yorozu;
using UnityEditor;
#endif

public class ButtonSample : MonoBehaviour
{
	[SerializeField]
	private UniButton button;

	private void Awake()
	{
		button.SetClickAction(OnClick);
	}

	private void OnClick()
	{
		Debug.Log("Click");
	}
}

#if UNITY_EDITOR

public class SampleButtonScene : EditorWindow
{
	private static readonly string SCENE_PATH = "Assets/Plugins/UniLib/ButtonExtension/Sample/";

	[MenuItem(Const.MENU_SAMPLE_PATH + "ButtonExtension/OpenSampleScene")]
	private static void OpenCommonScene()
	{
		UnityEditor.SceneManagement.EditorSceneManager.OpenScene(SCENE_PATH + "ButtonSample.unity");
	}
}

#endif