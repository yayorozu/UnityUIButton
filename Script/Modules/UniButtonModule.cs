using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public abstract class UniButtonModuleAbstract
	{
		protected UniButton _main;
		protected RectTransform _rect;

		public bool Clickable => _clickable;
		protected bool _clickable = true;

		public void SetUp(UniButton main)
		{
			_main = main;
			_rect = main.transform as RectTransform;
			Prepare();
		}

		public void UpdateFromOwner()
		{
			if (_main == null)
				return;

			Update();
		}

		protected virtual void Prepare() { }

		protected virtual void Update() { }

		public virtual void SetInteractable(bool enable)
		{
		}

		public virtual void Press()
		{
		}

		public abstract void OnPointerEnter(PointerEventData eventData);

		public abstract void OnPointerExit(PointerEventData eventData);

#if UNITY_EDITOR

		/// <summary>
		/// 必要なら override する
		/// </summary>
		public virtual void DrawEditor(UniButton button, UnityEditor.SerializedProperty property)
		{
		}

#endif

	}
}
