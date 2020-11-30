using System;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.UI
{
	[Serializable]
	public class YorozuButtonColorOutline : YorozuButtonColor
	{
		[SerializeField]
		private Outline[] _outlines = new Outline[0];

		[SerializeField]
		private ButtonColorData _colorData;

		private Color[] _caches;

		protected override void SetColor(float t, ButtonColorType current, ButtonColorType next)
		{
			if (_outlines.Length <= 0 || _colorData == null)
				return;

			var color = Color.Lerp(_colorData.GetColor(current), _colorData.GetColor(next), t);
			for (var i = 0; i < _outlines.Length; ++i)
			{
				if (_outlines[i] == null)
					continue;

				if (_caches == null || _caches.Length <= i || _caches[i] == Color.white)
				{
					_outlines[i].effectColor = color;
					continue;
				}

				_outlines[i].effectColor = color * _caches[i];
			}
		}

		protected override void Cache()
		{
			if (_caches == null || _caches.Length != _outlines.Length)
				_caches = new Color[_outlines.Length];

			for (var i = 0; i < _outlines.Length; ++i)
			{
				if (_outlines[i] == null)
					continue;

				_caches[i] = _outlines[i].effectColor;
			}
		}
	}
}
