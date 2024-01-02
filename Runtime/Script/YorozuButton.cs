using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yorozu.UI
{
	public class YorozuButton : Button
	{
		[SerializeReference]
		private YorozuButtonModule[] _modules = new YorozuButtonModule[0];

#if UNITY_EDITOR

		internal YorozuButtonModule[] EditorModules
		{
			get => _modules;
			set => _modules = value;
		}

		protected override void OnValidate()
		{
			transition = Transition.None;
			
			foreach (var module in _modules)
				module?.SetUp(this, ConvertState(currentSelectionState));
			
			base.OnValidate();
		}

#endif

		private Coroutine _pressCoroutine = null;
		
		/// <summary>
		/// イベントをハンドルしづらいので拡張を呼び出ししてもらう
		/// </summary>
		[Obsolete("Use SetClick instead.")]
		public new ButtonClickedEvent onClick;

		private Action _clickEvent;

		protected override void Awake()
		{
			YorozuButtonManager.Register(this);
			base.Awake();
			transition = Transition.None;
			
			var s = ConvertState(currentSelectionState);
			foreach (var part in _modules)
				part.SetUp(this, s);
			
			base.onClick.AddListener(() => _clickEvent?.Invoke());
		}

		protected override void OnDestroy()
		{
			YorozuButtonManager.Unregister(this);
			_clickEvent = null;
			
			foreach (var part in _modules)
				part.DestroyFromOwner();
			base.OnDestroy();
		}

		/// <summary>
		/// Unity Event Update
		/// </summary>
		private void Update()
		{
			foreach (var module in _modules)
				module.UpdateFromOwner();
		}

		/// <summary>
		/// モジュールの取得
		/// </summary>
		public bool TryGetModule<T>(out T findModule) where T : YorozuButtonModule
		{
			var data = _modules.FirstOrDefault(p => p.GetType() == typeof(T));
			if (data == null)
			{
				findModule = null;
				return false;
			}

			findModule = data as T;
			return true;
		}

		/// <summary>
		/// クリック処理を登録
		/// </summary>
		public void SetClickEvent(Action action)
		{
			_clickEvent += action;
		}

		/// <summary>
		/// クリック処理を解除
		/// </summary>
		public void RemoveClickEvent(Action action)
		{
			_clickEvent -= action;
		}
		
		public void ClearClickEvent()
		{
			_clickEvent = null;
		}

		/// <summary>
		/// クリック処理を実行する
		/// </summary>
		public void ClickInvoke()
		{
			YorozuButtonManager.ClickRegister();
			UISystemProfilerApi.AddMarker("Button.onClick", this);
			base.onClick.Invoke();
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			// 連打同時押し対応
			if (!YorozuButtonManager.Clickable ||
			    _pressCoroutine != null || 
			    !IsActive() || 
			    !IsInteractable()
			   )
				return;
			
			foreach (var part in _modules)
				part.Press();
			
			_pressCoroutine = StartCoroutine(WaitPress());
		}
		
		/// <summary>
		/// モジュールの動作を待つ
		/// </summary>
		private IEnumerator WaitPress()
		{
			bool any;
			while (true)
			{
				any = false;
				yield return null;
					
				// 全パーツがクリック処理するまで待つ
				foreach (var m in _modules)
				{
					if (m.CurrentState == YorozuButtonModule.State.Break)
					{
						_pressCoroutine = null;
						yield break;
					}
				}

				foreach (var m in _modules)
				{
					if (m.CurrentState == YorozuButtonModule.State.Processing)
					{
						any = true;
						break;
					}
				}

				if (any)
					continue;
					
				break;
			}

			YorozuButtonManager.PlaySound(this);
			ClickInvoke();
			_pressCoroutine = null;
		}

		/// <summary>
		/// Stateの変化を監視
		/// </summary>
		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			
			if (!gameObject.activeInHierarchy)
				return;

			var s = ConvertState(state);
			
			foreach (var module in _modules)
				module?.DoStateTransition(s, instant);
		}

		private static YorozuButtonModule.SelectionState ConvertState(SelectionState state)
		{
			switch (state)
			{
				case SelectionState.Highlighted:
					return YorozuButtonModule.SelectionState.Highlighted;
				case SelectionState.Pressed:
					return YorozuButtonModule.SelectionState.Pressed;
				case SelectionState.Selected:
					return YorozuButtonModule.SelectionState.Selected;
				case SelectionState.Disabled:
					return YorozuButtonModule.SelectionState.Disabled;
				case SelectionState.Normal:
					return YorozuButtonModule.SelectionState.Normal;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

#region EventSystems
		
		/// <summary>
		/// 状態がわかればいいので処理をまとめる
		/// </summary>
		private bool isPointerDown = false;
		private bool isPointerInside = false;
		
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			isPointerDown = false;
			foreach (var module in _modules)
				module.OnPointerChange(eventData, isPointerDown, isPointerInside);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			
			if (eventData.button != PointerEventData.InputButton.Left)
				return;
			
			isPointerDown = true;
			foreach (var module in _modules)
				module.OnPointerChange(eventData, isPointerDown, isPointerInside);
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			if (eventData == null || eventData.pointerEnter == null || eventData.pointerEnter.GetComponentInParent<Selectable>() != this)
				return;

			isPointerInside = true;
			foreach (var module in _modules)
				module.OnPointerChange(eventData, isPointerDown, isPointerInside);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			if (eventData == null || eventData.pointerEnter == null || eventData.pointerEnter.GetComponentInParent<Selectable>() != this)
				return;
			
			isPointerInside = false;
			foreach (var module in _modules)
				module.OnPointerChange(eventData, isPointerDown, isPointerInside);
		}
		
#endregion
		
	}
}
