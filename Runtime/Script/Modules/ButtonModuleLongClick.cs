using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public class ButtonModuleLongClick : YorozuButtonModule
	{
		private float _handleTime => YorozuButtonManager.Setting.longClickTime;

		private float _time;
		private bool _isPress;
		private Action _action;
		protected override void Prepare(SelectionState state)
		{
			// 長押しが有効な場合は通常のクリック処理は無効
			base.state = State.Break;
		}

		public override void OnPointerChange(PointerEventData eventData, bool isDown, bool isInside)
		{
			if (isDown && isInside)
			{
				_time = 0f;
			}
			else if (isInside && !isDown && _isPress)
			{
				owner.ClickInvoke();
			}
				
			_isPress = isDown && isInside; 
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
			// 長押し判定を通過するので通常のクリックを破棄
			if (_time >= _handleTime)
			{
				_isPress = false;
				_action?.Invoke();
			}
		}

		public void SetAction(Action action)
		{
			_action = action;
		}
	}
}
