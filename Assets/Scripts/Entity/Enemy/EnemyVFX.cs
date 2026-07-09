using UnityEngine;

namespace Entity.Enemy
{
    public class EnemyVFX : EntityVFX
    {
        #region Variables

        [SerializeField] private GameObject canCounterAlert;

        #endregion

        #region LifecycleMethods



        #endregion
        
        public void SwitchAlertEnable(bool enable) => canCounterAlert.SetActive(enable);
    }
}