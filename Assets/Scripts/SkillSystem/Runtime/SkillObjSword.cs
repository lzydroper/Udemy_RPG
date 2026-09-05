using System;
using Data;
using Entity;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillObjSword : SkillObjBase<SkillSwordThrow>
    {
        #region Variables

        protected Transform player;
        protected Rigidbody2D rb;
        protected Animator ani;
        protected bool shouldComeback;
        protected float comebackSpeed = 20;
        protected float maxAllowedDistance = 25;

        #endregion

        #region LifecycleMethods

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ani = GetComponentInChildren<Animator>();
        }

        protected virtual void Update()
        {
            transform.right = rb.linearVelocity;
            HandleComeback();
        }

        #endregion
        
        public virtual void SetupSword(SkillSwordThrow skill, Vector2 velocity)
        {
            Setup(skill);
            rb.linearVelocity = velocity;
            player = skill.skillManager.player.transform;
        }

        public void GetSwordBackToPlayer()
        {
            shouldComeback = true;
            ani.SetBool("isSpinning", true);
        }

        protected void HandleComeback()
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > maxAllowedDistance)
                GetSwordBackToPlayer();

            if (shouldComeback == false)
                return;

            transform.position = Vector2.MoveTowards(
                transform.position, player.position, comebackSpeed * Time.deltaTime);

            if (distance < .5f)
                Destroy(gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            StopSword(collision);
            DamageInRadius(transform, 1, out _);
        }

        protected void StopSword(Collider2D collision)
        {
            rb.simulated = false;
            transform.parent = collision.transform;
        }
    }
}