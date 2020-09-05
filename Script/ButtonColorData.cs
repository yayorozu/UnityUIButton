using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yorozu.UI
{
	/// <summary>
	/// ボタン色データ 
	/// </summary>
	[CreateAssetMenu(fileName = "ButtonColorData", menuName = Const.MENU_PATH + "UI/ButtonColorData")]
	public class ButtonColorData : ScriptableObject
	{
		// デフォルト
		public Color NormalColor = new Color(1f, 1f, 1f);

		// 押せない
		public Color DisabledColor = new Color(0.6f, 0.6f, 0.6f);

		// 押したとき
		public Color PressColor = new Color(0.5f, 0.5f, 0.5f);

		public Color GetColor(ButtonColorType type)
		{
			switch (type)
			{
				case ButtonColorType.Normal:
					return NormalColor;
				case ButtonColorType.Pressed:
					return PressColor;
				case ButtonColorType.Disabled:
					return DisabledColor;
			}

			return NormalColor;
		}
	}

	[CreateAssetMenu(fileName = "ButtonColorTMProData", menuName = Const.MENU_PATH + "UI/ButtonColorTMProData")]
	public class ButtonColorTMProData : ScriptableObject
	{
		// デフォルト
		public Material NormalMaterial;

		// 押せない
		public Material DisabledMaterial;

		// 押したとき
		public Material PressMaterial;

		public Material GetMaterial(ButtonColorType type)
		{
			switch (type)
			{
				case ButtonColorType.Normal:
					return NormalMaterial;
				case ButtonColorType.Pressed:
					return PressMaterial;
				case ButtonColorType.Disabled:
					return DisabledMaterial;
			}

			return NormalMaterial;
		}
	}

	[CreateAssetMenu(fileName = "ButtonColorSetData", menuName = Const.MENU_PATH + "UI/ButtonColorSetData")]
	public class ButtonColorSetData : ScriptableObject
	{
		// アクティブ
		public Color ActiveColor = new Color(1f, 1f, 1f);

		// 非アクティブ
		public Color InactiveColor = new Color(0f, 0f, 0f);

		public Color GetColor(bool enable)
		{
			return enable ? ActiveColor : InactiveColor;
		}
	}

	[CreateAssetMenu(fileName = "ButtonColorSetTMProData", menuName = Const.MENU_PATH + "UI/ButtonColorSetTMProData")]
	public class ButtonColorSetTMProData : ScriptableObject
	{
		// アクティブ
		public Color ActiveColor = new Color(1f, 1f, 1f);

		public Material ActiveMaterial;

		// 非アクティブ
		public Color InactiveColor = new Color(0f, 0f, 0f);
		public Material InactiveMaterial;

		public Color GetColor(bool enable)
		{
			return enable ? ActiveColor : InactiveColor;
		}

		public Material GetMaterial(bool enable)
		{
			return enable ? ActiveMaterial : InactiveMaterial;
		}
	}
}