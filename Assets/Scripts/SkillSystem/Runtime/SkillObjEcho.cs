using Core.Pool;
using Core.Utils;
using Data;
using Entity;
using Entity.Enemy;
using Entity.Interfaces;
using Generated;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillObjEcho : SkillObjBase<SkillTimeEcho>, IDamageable, IPoolReturn
    {
        #region Variables

        [SerializeField] private float wispMoveSpeed = 15;
        [SerializeField] private LayerMask groundLayer;
        private bool shouldMoveToPlayer;

        private Transform player;
        private TrailRenderer wispTrail;
        private EntityHealth playerHealth;
        private EntityStatusHandler statusHandler;

        public int maxAttacks { get ; private set; } 
        
        private Rigidbody2D rb;
        private Animator ani;

        private bool isStopped;
        
        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ani = GetComponentInChildren<Animator>();
        }
        
        private void Update()
        {
            ani.SetFloat("yVelocity", rb.linearVelocity.y);
            HandleStop();
        }

        #endregion

        public void SetupEcho(SkillTimeEcho skill)
        {
            Setup(skill);
            
            isStopped = false;
        }

        public void OnReturn()
        {
            GameManager.Pool.RentAsync(Keys.Pool.SmokeCloudVFX, position: transform.position);
            rb.linearVelocity = Vector2.zero;
            isStopped = false;
        }

        private void HandleStop()
        {
            if (isStopped)
                return;
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, Vector2.down, 1.5f, groundLayer);

            if(hit.collider != null)
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        public bool TryTakeDamage(in DamageContext damageContext)
        {
            this.Return();
            return true;
        }
    }
}