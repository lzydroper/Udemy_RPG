using UnityEngine;

namespace Entity
{
    public class EntityAnimationTriggers : MonoBehaviour
    {
        private EntityBase entity;

        private void Awake()
        {
            entity = GetComponentInParent<EntityBase>();
        }

        private void CurrentStateTrigger()
        {
            entity.CallCurrentStateTrigger();
        }
    }
}