using Entity;
using UnityEngine;

namespace UI
{
    public class MiniHealthBar : MonoBehaviour
    {
        #region Variables

        private EntityBase _entityBase;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            _entityBase = GetComponentInParent<EntityBase>();
        }

        private void OnEnable()
        {
            _entityBase.OnFlip += HandleFlip;
        }

        private void OnDisable()
        {
            _entityBase.OnFlip -= HandleFlip;
        }

        #endregion

        private void HandleFlip() => transform.rotation = Quaternion.identity;
    }
}