using Yorozu.Easing;
using UnityEngine;
namespace Yorozu.UI
{
	public interface IReactionData
	{
		float ReactionTime { get; }
		/// <summary>
		/// リアクション時のスケールを返す
		/// </summary>
		/// <param name="t">0f ~ 1fの時間</param>
		/// <param name="reverse">押したときか</param>
		/// <param name="defaultScale">もともとのUIのScale</param>
		/// <returns></returns>
		void DoReaction(RectTransform rectTransform, float t, bool reverse, Vector3 defaultScale);
	}
	
	[CreateAssetMenu(fileName = "ButtonReactionData", menuName = "Yorozu/UI/ButtonReactionData")]
	public class ButtonReactionData : ScriptableObject, IReactionData
	{
		[SerializeField]
		[Range(0.1f, 1f)]
		private float _reactionTime = 0.3f;

		float IReactionData.ReactionTime => _reactionTime;

		[SerializeField]
		[Range(0.5f, 1.5f)]
		private float _reactionScale = 0.9f;

		[SerializeField]
		private EaseType _reactionEase = EaseType.OutExpo;
		[SerializeField]
		private EaseType _returnEase = EaseType.InOutBack;

		void IReactionData.DoReaction(RectTransform rectTransform, float t, bool reaction, Vector3 defaultScale)
		{
			var type = reaction ? _reactionEase : _returnEase;
			var begin = reaction ? defaultScale : defaultScale * _reactionScale;
			var end = reaction ? defaultScale * _reactionScale : defaultScale;

			rectTransform.localScale = new Vector3(
				Ease.Eval(type, t, _reactionTime, begin.x, end.x),
				Ease.Eval(type, t, _reactionTime, begin.y, end.y),
				1
			);
		}
	}
}
