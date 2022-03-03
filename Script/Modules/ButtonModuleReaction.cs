using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public class ButtonModuleReaction : YorozuButtonModule
	{
		private enum ReactionState
		{
			Ping,
			Pong,
		}
		
		private IReactionData _data;
		
		private float _time;
		private Vector3 _defaultScale;
		private Vector2 _defaultPivot;

		private ReactionState _reactionState;
		
		protected override void Prepare(SelectionState state)
		{
			_defaultScale = rectTransform.localScale;
			_defaultPivot = rectTransform.pivot;

			_data = YorozuButtonManager.ReactionData;
			_time = _data.ReactionTime;
			_reactionState = ReactionState.Pong;
		}

		private void ResetRect()
		{
			SetPivot(_defaultPivot);
			rectTransform.localScale = _defaultScale;
		}
		
		private void DoReaction(ReactionState reactionState)
		{
			if (!owner.IsInteractable())
				return;
			
			if (_reactionState == reactionState) 
				return;
			
			if (reactionState == ReactionState.Pong && _reactionState != ReactionState.Ping)
				return;

			_reactionState = reactionState;
			_time = 0f;
			
			if (_reactionState == ReactionState.Pong && YorozuButtonManager.Setting.timing == YorozuButtonSetting.ClickTiming.Pressed)
			{
				state = State.Clickable;
			}
		}

		public override void OnPointerChange(PointerEventData eventData, bool isDown, bool isInside)
		{
			DoReaction(isDown && isInside ? ReactionState.Ping : ReactionState.Pong);
		}

		public override void DoStateTransition(SelectionState state, bool instant)
		{
			switch (state)
			{
				case SelectionState.Disabled:
					ResetRect();
					break;
			}
		}

		protected override void Update()
		{
			if (_time >= _data.ReactionTime)
				return;

			_data.DoReaction(rectTransform,
				_time / _data.ReactionTime,
				_reactionState == ReactionState.Ping, 
				_defaultScale
			);

			_time += Time.deltaTime;

			if (YorozuButtonManager.Setting.timing != YorozuButtonSetting.ClickTiming.Pressed)
			{
				state = _time <= 0f ? State.Clickable : State.Processing;
			}

			if (_time < _data.ReactionTime)
				return;
			
			if (_reactionState == ReactionState.Pong)
				ResetRect();
		}

		/// <summary>
		/// Pivot 変更
		/// </summary>
		private void SetPivot(Vector2 v)
		{
			var size = rectTransform.rect.size;
			var deltaPivot = rectTransform.pivot - v;
			var deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
			rectTransform.pivot = v;
			rectTransform.localPosition -= deltaPosition;
		}
	}
}
