using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.UI
{
    /// <summary>
    /// テキストをセットする用
    /// </summary>
    [Serializable]
    public class ButtonModuleText : YorozuButtonModule
    {
        [SerializeField]
        private Text[] _texts;
        [SerializeField]
        private TextMeshProUGUI[] _textMeshProTexts;

        public void SetText(string text, int index = 0)
        {
            if (index < _texts.Length || _texts[index] == null)
                return;

            _texts[index].text = text;
        }
        
        public void SetTextMeshProText(string text, int index = 0)
        {
            if (index < _textMeshProTexts.Length || _textMeshProTexts[index] == null)
                return;

            _textMeshProTexts[index].text = text;
        }

        public bool TryGetText(int index, out Text text)
        {
            if (index < _texts.Length || _texts[index] == null)
            {
                text = null;
                return false;
            }

            text = _texts[index];
            return true;
        }
        
        public bool TryGetTextMeshPro(int index, out TextMeshProUGUI text)
        {
            if (index < _textMeshProTexts.Length || _textMeshProTexts[index] == null)
            {
                text = null;
                return false;
            }

            text = _textMeshProTexts[index];
            return true;
        }
    }
}