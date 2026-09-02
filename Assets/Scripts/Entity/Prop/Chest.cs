using System.Collections;
using Data;
using Entity.Interfaces;
using UnityEngine;

namespace Entity.Prop
{
    public class Chest : MonoBehaviour, IDamageable
    {
        #region Variables

        private Animator ani;
        private Rigidbody2D rb;
        private Collider2D col;
        private EntityVFX entityVFX;

        [Header("OpenDetail")] 
        [SerializeField] private Vector2 openVelocity = new(0f, 3f);
        
        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            entityVFX = GetComponent<EntityVFX>();
        }

        #endregion

        public bool TryTakeDamage(in DamageContext context)
        {
            entityVFX.PlayOnDamageVFX();
            ani.SetBool("open", true);
            rb.linearVelocity = openVelocity;
            StartCoroutine(DelayClose());
            return true;
        }

        private IEnumerator DelayClose()
        {
            yield return new WaitForSeconds(1f);
            rb.simulated = false;
            col.enabled = false;
            this.enabled = false;
        }
    }
}