using System;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.UI
{
	[Serializable]
	public class UniButtonColorGraphic : UniButtonColor
	{
		[SerializeField]
		private Graphic[] _graphics = new Graphic[0];

		[SerializeField]
		private ButtonColorData _colorData;

		private Color[] _caches;

		protected override void SetColor(float t, ButtonColorType current, ButtonColorType next)
		{
			if (_graphics.Length <= 0 || _colorData == null)
				return;

			var color = Color.Lerp(_colorData.GetColor(current), _colorData.GetColor(next), t);
			for (var i = 0; i < _graphics.Length; ++i)
			{
				if (_graphics[i] == null)
					continue;
				if (_caches == null || _caches.Length <= i || _caches[i] == Color.white)
				{
					_graphics[i].color = color;
					continue;
				}

				_graphics[i].color = color * _caches[i];
			}
		}

		protected override void Cache()
		{
			if (_graphics.Length <= 0)
				return;

			if (_caches == null || _caches.Length != _graphics.Length)
				_caches = new Color[_graphics.Length];

			for (var i = 0; i < _graphics.Length; ++i)
			{
				if (_graphics[i] == null)
					continue;

				_caches[i] = _graphics[i].color;
			}
		}
	}
}
