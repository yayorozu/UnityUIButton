using UnityEngine;

namespace Yorozu.UI
{
    public class YorozuButtonSetting : ScriptableObject
    {
        internal enum ClickTiming
        {
            /// <summary>
            /// リアクションが終わってから
            /// </summary>
            EndReaction,
            /// <summary>
            /// 押したタイミング
            /// </summary>
            Pressed,
        }
        
        /// <summary>
        /// クリック処理を行うタイミング
        /// ButtonModuleReaction で利用する
        /// </summary>
        [SerializeField]
        internal ClickTiming timing = ClickTiming.Pressed;

        [SerializeField]
        internal float longClickTime = 1f;

        /// <summary>
        /// リアクションの動きに使うデータ
        /// </summary>
        [SerializeField]
        internal ButtonReactionData reactionData;
    }
}