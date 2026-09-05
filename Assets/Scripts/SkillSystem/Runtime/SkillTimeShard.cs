using System.Collections;
using Core.Utils;
using Generated;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillTimeShard : SkillBase
    {
        #region Variables

        private SkillObjShard currShard;
        [SerializeField] private float detonateDelayRegular = 2;

        [Header("Moving Shard Upgrade")]
        [SerializeField] private float shardSpeed = 7;

        [Header("Multicast Shard Upgrade")]
        [SerializeField] private int maxCharges = 3;
        [SerializeField] private int currentCharges;
        [SerializeField] private bool isRecharging;

        [Header("Teleport Shard Upgrade")]
        [SerializeField] private float detonateDelayTeleport = 10;

        [Header("Health Rewind Shard Upgrade")]
        [SerializeField] private float savedHealthPercent;

        #endregion

        #region LifecycleMethods

        private void Start()
        {
            currentCharges = maxCharges;
        }

        #endregion

        public override void UseSkill()
        {
            if (!IsCanUseSkill())
                return;
            // 由于upgradeType是Enum，所以用Switch而不是教程的雷霆大if
            switch (upgradeType)
            {
                case SkillUpgradeType.Shard: 
                    HandleShardRegular(); break;
                case SkillUpgradeType.Shard_MoveToEnemy:
                    HandleShardRegular(); break;
                case SkillUpgradeType.Shard_Multicast:
                    HandleShardMulticast(); break;
                case  SkillUpgradeType.Shard_Teleport:
                    HandleShardTeleport(); break;
                case SkillUpgradeType.Shard_TeleportHpRewind:
                    HandleShardHpRewind(); break;
                default:
                    Debug.LogError($"Unknown upgrade type {upgradeType}"); break;
            }
        }

        public void OnShardExplode()
        {
            currShard = null;
            if (upgradeType is SkillUpgradeType.Shard_Teleport or SkillUpgradeType.Shard_TeleportHpRewind)
            {
                SetOnCooldown();
            }
        }

        public void SummonShard(bool isTeleport = false)
        {
            var detonateDelay = isTeleport ? detonateDelayTeleport : detonateDelayRegular;
            if (GameManager.Pool.TryRent<SkillObjShard>(Keys.Pool.SkillObjShard, out var shard,
                    position: transform.position))
            {
                shard.SetupShard(this, detonateDelay);
                if (upgradeType is SkillUpgradeType.Shard_MoveToEnemy or SkillUpgradeType.Shard_Multicast)
                    shard.MoveTowardClosest(shardSpeed);
                currShard = shard;
            }
        }

        private void HandleShardRegular()
        {
            SummonShard();
            SetOnCooldown();
        }

        // TODO: 这代码太狗屎了，这里必定得改，逻辑耦合成啥样了都
        private void HandleShardMulticast()
        {
            if (currentCharges <= 0)
                return;
            SummonShard();
            if (isRecharging == false)
                StartCoroutine(ShardRechargeCo());
        }
    
        private IEnumerator ShardRechargeCo()
        {
            isRecharging = true;
            while (currentCharges < maxCharges)
            {
                yield return new WaitForSeconds(cooldown);
                currentCharges++;
            }
            isRecharging = false;
        }

        private void SwapShardAndPlayer()
        {
            if (currShard == null)
                return;
            var shardPos = currShard.transform.position;
            var playerPos = skillManager.player.transform.position;
            skillManager.player.Teleport(shardPos);
            currShard.transform.position = playerPos;
            currShard.Explode();
        }

        private void HandleShardTeleport()
        {
            if (currShard == null)
                SummonShard(true);
            else
                SwapShardAndPlayer();
        }

        private void HandleShardHpRewind()
        {
            if (currShard == null)
            {
                SummonShard(true);
                savedHealthPercent = skillManager.player.health.GetHealthPercent();
            }
            else
            {
                SwapShardAndPlayer();
                skillManager.player.health.SetHealthToPercent(savedHealthPercent);
            }
        }
    }
}