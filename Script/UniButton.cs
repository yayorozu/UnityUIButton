using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yorozu.UI
{
	public class UniButton : Selectable, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeReference]
		private UniButtonModuleAbstract[] _modules = new UniButtonModuleAbstract[0];

#if UNITY_EDITOR

		internal UniButtonModuleAbstract[] Modules
		{
			get => _modules;
			set => _modules = value;
		}

#endif

		private bool _isPress;
		private bool _waitPress;

		private Action _clickAction;

		protected override void Awake()
		{
			base.Awake();
			transition = Transition.None;
			_isPress = false;

			foreach (var part in _modules)
				part.SetUp(this);
		}

		private void Update()
		{
			foreach (var module in _modules)
				module.UpdateFromOwner();
		}

		public bool TryGetModule<T>(out T findModule) where T : UniButtonModuleAbstract
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

		public void SetInteractable(bool enable)
		{
			interactable = enable;
			foreach (var part in _modules)
				part.SetInteractable(enable);
		}

		public void SetClickAction(Action action)
		{
			_clickAction = action;
		}

		public void RemoveClickAction()
		{
			_clickAction = null;
		}

		private void Press()
		{
			if (_waitPress)
				return;

			if (!interactable)
				return;

			if (!IsActive())
				return;

			foreach (var part in _modules)
				part.Press();

			StartCoroutine(PressImpl());
		}

		private IEnumerator PressImpl()
		{
			_waitPress = true;
			// 全パーツがクリック処理するまで待つ
			foreach (var part in _modules)
				yield return new WaitUntil(() => part.Clickable);

			_clickAction?.Invoke();
			_waitPress = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			Press();
		}

		private void Enter(PointerEventData eventData)
		{
			if (_isPress)
				return;

			_isPress = true;
			foreach (var module in _modules)
				module.OnPointerEnter(eventData);
		}

		private void Exit(PointerEventData eventData)
		{
			if (!_isPress)
				return;

			_isPress = false;
			foreach (var module in _modules)
				module.OnPointerExit(eventData);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			Exit(eventData);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			Enter(eventData);
		}

#if false
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			Enter(eventData);
		}
#endif

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			Exit(eventData);
		}
	}
}
