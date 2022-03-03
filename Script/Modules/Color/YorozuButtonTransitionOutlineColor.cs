using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.UI
{
	[Serializable]
	public class YorozuButtonTransitionOutlineColor : YorozuButtonModule
	{
		[SerializeField]
		private Outline[] _outlines = new Outline[0];
		[SerializeField]
		private ButtonColorData _data;
		
		[NonSerialized]
		private CanvasRenderer[] _canvasRenderers;
		[NonSerialized]
		private Color[] _startColors;

		private Color _targetColor;
		private float _fadeTime;
		private float _fadeDuration;
		
#if UNITY_EDITOR
		public override void EditorDrawer(YorozuButton button)
		{
			if (GUILayout.Button("Find Child Outlines"))
			{
				_outlines = button.GetComponentsInChildren<Outline>();
			}
		}
#endif
		
		protected override void Prepare(SelectionState state)
		{
			if (_data == null)
			{
				_data = ScriptableObject.CreateInstance<ButtonColorData>();
			}
			_canvasRenderers = _outlines
				.Select(o => o.GetComponent<CanvasRenderer>())
				.ToArray();

			_startColors = new Color[_canvasRenderers.Length];

			_fadeDuration = _data.duration;
		}

		public override void DoStateTransition(SelectionState state, bool instant)
		{
			if (_outlines.Length <= 0 || _data == null)
				return;

			for (var i = 0; i < _canvasRenderers.Length; i++) 
				_startColors[i] = _canvasRenderers[i].GetColor();
			
			_targetColor = _data.GetColor(state);
			_fadeTime = 0f;
		}
		
		protected override void Update()
		{
			if (_fadeTime >= _fadeDuration)
				return;

			_fadeTime += Time.deltaTime;
			SetColor(_fadeTime / _fadeDuration);
		}
		
		private void SetColor(float t)
		{
			for (var i = 0; i < _canvasRenderers.Length; i++)
			{
				var canvasRenderer = _canvasRenderers[i];
				var color = Color.Lerp(_startColors[i], _targetColor, t);
				canvasRenderer.SetColor(color);
			}
		}
	}
}
