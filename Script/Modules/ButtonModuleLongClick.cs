using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public class ButtonModuleLongClick : YorozuButtonModuleAbstract
	{
		[SerializeField]
		private float _handleTime = 1f;

		private float _time;
		private bool _isPress;
		private Action _action;
		private bool _isBreak;

		public override void OnPointerEnter(PointerEventData eventData)
		{
			_time = 0f;
			_clickable = false;
			_isPress = true;
			_isBreak = false;
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			_isBreak = true;
			_clickable = true;
			_isPress = false;
			if (_time < _handleTime)
			{
				_main.DoClick();
			}
		}

		public override bool IsBreakClick()
		{
			return _isBreak;
		}

		protected override void Update()
		{
			if (!_isPress)
			{
				return;
			}

			if (_time >= _handleTime)
				return;
			
			_time += Time.deltaTime;
			if (_time >= _handleTime)
			{
				_action?.Invoke();
				_isBreak = true;
			}
		}

		public void SetAction(Action action)
		{
			_action = action;
		}
	}
}
