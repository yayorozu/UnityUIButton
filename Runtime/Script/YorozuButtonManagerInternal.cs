using System.Collections.Generic;
using UnityEngine;

namespace Yorozu.UI
{
    public static partial class YorozuButtonManager
    {
        private static List<YorozuButton> _activeButtons = new List<YorozuButton>(20);

        private static ButtonSound _soundEvent;

        private static float _clickLockTime = 0.5f;
        /// <summary>
        /// 最後にクリックした時間
        /// </summary>
        private static float _lastClickTime = 0f;

        /// <summary>
        /// 最後にクリックしてから一定時間経過しているか
        /// </summary>
        internal static bool Clickable => UnityEngine.Time.realtimeSinceStartup - _lastClickTime > _clickLockTime;

        internal static YorozuButtonSetting Setting;

        internal static IReactionData ReactionData;

        /// <summary>
        /// インスタンス作成したタイミングでロード処理を行う
        /// </summary>
        static YorozuButtonManager()
        {
            Setting = Resources.Load<YorozuButtonSetting>("YorozuButton/Setting");
            if (Setting == null)
            {
                Setting = ScriptableObject.CreateInstance<YorozuButtonSetting>();
            }

            var rd = Setting.reactionData;
            if (rd == null)
                rd = ScriptableObject.CreateInstance<ButtonReactionData>();

            ReactionData = rd;
        }

        internal static void Register(YorozuButton button)
        {
            if (_activeButtons.Contains(button))
                return;

            _activeButtons.Add(button);
        }

        internal static void Unregister(YorozuButton button)
        {
            if (!_activeButtons.Contains(button))
                return;

            _activeButtons.Remove(button);
        }

        /// <summary>
        /// ボタンに登録されたKeyを伝播
        /// </summary>
        internal static void PlaySound(YorozuButton button)
        {
            if (!button.TryGetModule<ButtonModuleSound>(out var module))
                return;

            if (string.IsNullOrEmpty(module.Key))
                return;

            _soundEvent?.Invoke(module.Key);
        }

        /// <summary>
        /// 最後にクリックした時間を記録
        /// </summary>
        internal static void ClickRegister()
        {
            _lastClickTime = UnityEngine.Time.realtimeSinceStartup;
        }
    }
}