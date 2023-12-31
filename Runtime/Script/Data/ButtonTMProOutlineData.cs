using UnityEngine;

namespace Yorozu.UI
{
    [CreateAssetMenu(fileName = "ButtonTMProOutlineData", menuName = "Yorozu/UI/ButtonColorTMProData")]
    public class ButtonTMProOutlineData : ScriptableObject
    {
        [SerializeField]
        private Material _normalCMaterial;

        [SerializeField]
        private Material _disabledMaterial;

        [SerializeField]
        private Material _pressMaterial;

        [SerializeField]
        private Material _selectedMaterial;

        [SerializeField]
        private Material _highlightMaterial;

        /// <summary>
        /// Material 取得
        /// </summary>
        internal Material GetMaterial(YorozuButtonModule.SelectionState state)
        {
            var material = state switch
            {
                YorozuButtonModule.SelectionState.Highlighted => _highlightMaterial,
                YorozuButtonModule.SelectionState.Pressed => _pressMaterial,
                YorozuButtonModule.SelectionState.Selected => _selectedMaterial,
                YorozuButtonModule.SelectionState.Disabled => _disabledMaterial,
                _ => _normalCMaterial
            };

            return material;
        }
    }
}