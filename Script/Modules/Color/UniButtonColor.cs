using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yorozu.UI
{
	public enum ButtonColorType
	{
		Normal,
		Disabled,
		Pressed,
	}

	[Serializable]
	public abstract class UniButtonColor : UniButtonModuleAbstract
	{
		[SerializeField]
		[Range(0f, 1f)]
		private float _fadeDuration = 0.1f;

		private float _fadeTime;
		private ButtonColorType _currentColorType = ButtonColorType.Normal;
		private ButtonColorType _nextColorType = ButtonColorType.Normal;

		public override void OnPointerEnter(PointerEventData eventData)
		{
			_fadeTime = 0f;
			_currentColorType = _nextColorType;
			_nextColorType = ButtonColorType.Pressed;
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			_fadeTime = 0f;
			_currentColorType = _nextColorType;
			_nextColorType = ButtonColorType.Normal;
		}

		protected override void Prepare()
		{
			_nextColorType = _currentColorType = _main.interactable ? ButtonColorType.Normal : ButtonColorType.Disabled;
		}

		public override void SetInteractable(bool enable)
		{
			_nextColorType = _currentColorType = enable ? ButtonColorType.Normal : ButtonColorType.Disabled;
			DoCache();
			SetColor(1f, _currentColorType, _nextColorType);
		}

		protected override void Update()
		{
			if (_currentColorType == _nextColorType)
				return;

			if (_fadeTime >= _fadeDuration)
				return;

			_fadeTime += Time.deltaTime;

			SetColor(_fadeTime / _fadeDuration, _currentColorType, _nextColorType);
			if (_fadeTime >= _fadeDuration)
				_currentColorType = _nextColorType;
		}

		protected abstract void SetColor(float t, ButtonColorType current, ButtonColorType next);

		private void DoCache()
		{
			if (_currentColorType != ButtonColorType.Normal)
				return;

			Cache();
		}

		protected abstract void Cache();
	}
}
