using System;
using UnityEngine;

namespace UI
{
    public class UITooltip : MonoBehaviour
    {
        #region Variables

        private RectTransform rct;
        [SerializeField] private Vector2 closePosition = new (9999f, 9999f);
        [SerializeField] private Vector2 offset = new (300f, 20f);

        #endregion

        #region LifecycleMethods

        protected virtual void Awake()
        {
            rct = GetComponent<RectTransform>();
        }

        private void Start()
        {
            Show(false, rct);
        }

        private void OnDisable()
        {
            Show(false, null);
        }

        #endregion

        public virtual void Show(bool enable, RectTransform targetRct)
        {
            if (enable == false)
            {
                rct.position = closePosition;
                return;
            }
            UpdatePosition(targetRct);
        }

        private void UpdatePosition(RectTransform targetRct)
        {
            float screenCenterX = Screen.width / 2f;
            float screenTop = Screen.height;
            float screenBtm = 0f;
            // 如果目标显示位置在左，则右移，否则左移
            Vector2 mvePos = targetRct.position;
            mvePos.x += mvePos.x < screenCenterX ? offset.x : -offset.x;
            // 如果将要显示的位置的y，rct的顶部，超过屏幕顶部，则从顶部下移，否则从底部上移
            float rctHeightHalf = rct.sizeDelta.y / 2f;
                // rct.sizeDelta.y / 2f;
                // rct.rect.height / 2f;
            float rctTop = mvePos.y + rctHeightHalf;
            float rctBtm = mvePos.y - rctHeightHalf;
            if (rctTop > screenTop)
                mvePos.y = screenTop - rctHeightHalf - offset.y;
            if (rctBtm < screenBtm)
                mvePos.y = screenBtm + rctHeightHalf + mvePos.y;
            
            rct.position = mvePos;
        }
    }
}