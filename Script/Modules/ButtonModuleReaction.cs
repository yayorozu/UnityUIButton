using System;
using Yorozu.Easing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public class ButtonModuleReaction : YorozuButtonModuleAbstract
	{
		[SerializeField]
		private ButtonReactionData _data;

		private float _time;
		private Vector3 _defaultScale;
		private Vector2 _defaultPivot;

		private bool _isEnter;

		protected override void Prepare()
		{
			_defaultScale = _rect.localScale;
			_defaultPivot = _rect.pivot;

			if (_data == null)
			{
				_data = ScriptableObject.CreateInstance<ButtonReactionData>();
				_data.Init();
			}
		}

		private void ResetRect()
		{
			SetPivot(_defaultPivot);
			_rect.localScale = _defaultScale;
		}

		public override void SetInteractable(bool enable)
		{
			ResetRect();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (_isEnter)
				return;

			_isEnter = true;
			_time = _data.ReactionTime;
			SetPivot(Vector2.one / 2f);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			if (!_isEnter)
				return;

			_isEnter = false;
			_time = _data.ReactionTime;
		}

		protected override void Update()
		{
			if (_time <= 0f)
				return;

			SetScale((_data.ReactionTime - _time) / _data.ReactionTime);
			_time -= Time.deltaTime;

			_clickable = _time <= 0f;
			if (_time <= 0f && !_isEnter)
				ResetRect();
		}

		/// <summary>
		/// Scale を変更する
		/// </summary>
		private void SetScale(float t)
		{
			var type = _isEnter ? _data.ReactionEase : _data.ReturnEase;
			var begin = _isEnter ? _defaultScale : _defaultScale * _data.ReactionScale;
			var end = _isEnter ? _defaultScale * _data.ReactionScale : _defaultScale;

			_rect.localScale = new Vector3(
				Ease.Eval(type, t, _data.ReactionTime, begin.x, end.x),
				Ease.Eval(type, t, _data.ReactionTime, begin.y, end.y),
				1
				);
		}

		/// <summary>
		/// Pivot 変更
		/// </summary>
		private void SetPivot(Vector2 v)
		{
			var size = _rect.rect.size;
			var deltaPivot = _rect.pivot - v;
			var deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
			_rect.pivot = v;
			_rect.localPosition -= deltaPosition;
		}
	}
}
