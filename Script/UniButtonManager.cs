using System.Collections.Generic;

namespace Yorozu.UI
{
	public static class UniButtonManager
	{
		private static List<UniButton> _activeButtons = new List<UniButton>(20);

		public static IEnumerable<UniButton> Buttons => _activeButtons;

		private static float _clickLockTime = 0.5f;

		/// <summary>
		/// 最後にクリックした時間
		/// </summary>
		private static float _lastClickTime = 0f;

		/// <summary>
		/// 最後にクリックしてから一定時間経過しているか
		/// </summary>
		internal static bool Clickable => UnityEngine.Time.realtimeSinceStartup - _lastClickTime > _clickLockTime;

		internal static void Register(UniButton button)
		{
			if (_activeButtons.Contains(button))
				return;

			_activeButtons.Add(button);
		}

		internal static void Unregister(UniButton button)
		{
			if (!_activeButtons.Contains(button))
				return;

			_activeButtons.Remove(button);
		}

		internal static void ClickRegister()
		{
			_lastClickTime = UnityEngine.Time.realtimeSinceStartup;
		}

		/// <summary>
		/// 連続クリック時間を更新
		/// </summary>
		public static void SetClickLockTime(float time)
		{
			_clickLockTime = time;
		}
	}
}
