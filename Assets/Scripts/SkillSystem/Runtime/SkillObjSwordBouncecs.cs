using System.Collections.Generic;
using Data;
using Entity;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillObjSwordBounce : SkillObjSword
    {
        #region Variables

        private int bounceCount;
        private Transform currentTarget;
        private readonly HashSet<Transform> hitTargets = new();
        private bool isBouncing;
        private bool isReturning;

        #endregion

        #region LifecycleMethods
        
        protected override void Update()
        {
            HandleComeback();

            if (isBouncing)
                HandleBounceMove();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            // 防止意外响应
            if (isBouncing || isReturning)
                return;
            rb.simulated = false;
            isBouncing = true;

            // 首次碰撞的目标
            hitTargets.Add(collision.transform);
            DamageInRadius(transform, 1);

            TryBounceNext();
        }

        #endregion

        private void HandleBounceMove()
        {
            // 目标在飞行途中丢失（如死亡），立即寻找下一个
            if (currentTarget == null)
            {
                TryBounceNext();
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, 
                skill.bounceSpeed * Time.deltaTime);

            // 击中目标
            if (Vector2.Distance(transform.position, currentTarget.position) < 0.75f)
            {
                DamageInRadius(transform, 1);
                hitTargets.Add(currentTarget);
                bounceCount--;
                TryBounceNext();
            }
        }

        private void TryBounceNext()
        {
            if (bounceCount <= 0)
            {
                StopBouncingAndReturn();
                return;
            }

            currentTarget = GetNextTarget();

            if (currentTarget == null)
            {
                StopBouncingAndReturn();
            }
        }
        
        private void StopBouncingAndReturn()
        {
            isBouncing = false;
            isReturning = true;
            currentTarget = null;
            GetSwordBackToPlayer();
        }

        private Transform GetNextTarget()
        {
            // 动态抓取当前位置周围的敌人
            Collider2D[] colliders = GetDetectTargets(transform, 10f);
            if (colliders == null || colliders.Length == 0) return null;

            List<Transform> unhitEnemies = new List<Transform>();
            List<Transform> allAliveEnemies = new List<Transform>();

            foreach (var col in colliders)
            {
                if (col == null) continue;
                Transform enemy = col.transform;
                allAliveEnemies.Add(enemy);

                if (!hitTargets.Contains(enemy))
                    unhitEnemies.Add(enemy);
            }

            if (allAliveEnemies.Count == 0) return null;

            // 优先从未弹过的目标中选；如果全弹过了，重置历史从活着的敌人里选
            List<Transform> candidates;
            if (unhitEnemies.Count == 0)
            {
                hitTargets.Clear();
                candidates = allAliveEnemies;
            }
            else
            {
                candidates = unhitEnemies;
            }

            // 随机返回一个候选目标
            return candidates[Random.Range(0, candidates.Count)];
        }

        public override void SetupSword(SkillSwordThrow skill, Vector2 velocity)
        {
            base.SetupSword(skill, velocity);
            ani.SetBool("isSpinning", true);
            isBouncing = false;
            isReturning = false;
            bounceCount = skill.bounceCount;
            hitTargets.Clear();
        }
    }
}