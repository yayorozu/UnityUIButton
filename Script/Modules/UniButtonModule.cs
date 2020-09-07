using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public abstract class UniButtonModuleAbstract
	{

#if UNITY_EDITOR

		/// <summary>
		/// 必要なら override する
		/// </summary>
		public virtual void DrawEditor(UniButton button, UnityEditor.SerializedProperty property)
		{
		}

#endif

		protected UniButton _main;
		protected RectTransform _rect;

		public bool Clickable => _clickable;
		[SerializeField, HideInInspector]
		protected bool _clickable = true;

		internal void SetUp(UniButton main)
		{
			_main = main;
			_rect = main.transform as RectTransform;
			Prepare();
		}

		internal void UpdateFromOwner()
		{
			if (_main == null)
				return;

			Update();
		}

		protected virtual void Prepare()
		{
		}

		protected virtual void Update()
		{
		}

		public virtual void SetInteractable(bool enable)
		{
		}

		public virtual void Press()
		{
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
		}

		/// <summary>
		/// メインボタンのクリック処理を無効化できる
		/// </summary>
		public virtual bool IsBreakClick()
		{
			return false;
		}
	}
}
