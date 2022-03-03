using System.Collections.Generic;

namespace Yorozu.UI
{
	/// <summary>
	/// ボタンが押された際のイベント
	/// </summary>
	public delegate void ButtonSound(string key);
	
	public static partial class YorozuButtonManager
	{
		/// <summary>
		/// 現在アクティブなボタン一覧
		/// </summary>
		public static IEnumerable<YorozuButton> Buttons => _activeButtons;

		/// <summary>
		/// サウンドをセットしているボタンをクリックした際のイベント
		/// </summary>
		public static event ButtonSound SoundEvent  
		{
			add => _soundEvent += value;
			remove => _soundEvent -= value;
		}
		
		/// <summary>
		/// 連続クリック時間を更新
		/// </summary>
		public static void SetClickLockTime(float time)
		{
			_clickLockTime = time;
		}

		/// <summary>
		/// リアクションの上書き
		/// </summary>
		public static void SetReaction(IReactionData data)
		{
			ReactionData = data;
		}
	}
}
