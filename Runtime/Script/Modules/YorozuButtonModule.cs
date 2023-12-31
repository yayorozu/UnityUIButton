using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yorozu.UI
{
	[Serializable]
	public abstract class YorozuButtonModule
	{

#if UNITY_EDITOR

		/// <summary>
		/// Inspector OnGUI で呼び出される
		/// </summary>
		public virtual void EditorDrawer(YorozuButton button)
		{
		}

#endif

		/// <summary>
		/// OnPointerClick が呼ばれた際に状態を判定する
		/// </summary>
		public enum State
		{
			/// <summary>
			/// クリック可能
			/// </summary>
			Clickable,
			/// <summary>
			/// 処理中
			/// </summary>
			Processing,
			/// <summary>
			/// クリック処理を破棄
			/// </summary>
			Break,
		}
		
		/// <summary>
		/// Same Selectable.SelectionState
		/// </summary>
		public enum SelectionState
		{
			Normal,
			Highlighted,
			Pressed,
			Selected,
			Disabled,
		}

		internal State CurrentState => state;
		protected State state = State.Clickable;
		
		[SerializeField, HideInInspector]
		protected YorozuButton owner;
		
		protected RectTransform rectTransform { get; private set; }

		protected GameObject gameObject { get; private set; }

		internal void SetUp(YorozuButton owner, SelectionState state)
		{
			this.owner = owner;
			rectTransform = owner.transform as RectTransform;
			gameObject = owner.gameObject;
			
			Prepare(state);
		}

		/// <summary>
		/// 破棄処理
		/// </summary>
		internal void DestroyFromOwner()
		{
			Dispose();
			owner = null;
			rectTransform = null;
			gameObject = null;
		}

		protected virtual void Dispose()
		{
		}

		internal void UpdateFromOwner()
		{
			if (owner == null)
				return;

			Update();
		}

		protected virtual void Prepare(SelectionState state)
		{
		}

		protected virtual void Update()
		{
		}

		/// <summary>
		/// ボタンのクリックが呼ばれた際に呼び出される
		/// </summary>
		public virtual void Press()
		{
		}

		public virtual void DoStateTransition(SelectionState state, bool instant)
		{
		}
		
		public virtual void OnPointerChange(PointerEventData eventData, bool isDown, bool isInside)
		{
		}
		
		/// <summary>
		/// コルーチンを利用する際に使う
		/// </summary>
		protected Coroutine StartCoroutine(IEnumerator routine)
		{
			return owner.StartCoroutine(routine);
		}
	}
}
