using System.Collections;
using Core.Utils;
using Generated;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillTimeEcho : SkillBase
    {
        #region Variables
        
        [SerializeField] private float timeEchoDuration = 5f;

        [Header("Attack Upgrades")]
        [SerializeField] private int maxAttacks = 3;
        [SerializeField] private float duplicateChance = .3f;

        [Header("Heal wisp Upgrades")]
        [SerializeField] private float damagePercentHealed = .3f;
        [SerializeField] private float cooldownReducedInSeconds;

        #endregion

        #region LifecycleMethods

        

        #endregion

        public override void UseSkill()
        {
            if (!IsCanUseSkill())
                return;
            if (GameManager.Pool.TryRent<SkillObjEcho>(Keys.Pool.SkillObjEcho, out var skillObj, 
                    position: transform.position))
            {
                skillObj.SetupEcho(this);
                skillObj.ReturnAsync(duration: timeEchoDuration);
            }
            // 由于upgradeType是Enum，所以用Switch而不是教程的雷霆大if
            // switch (upgradeType)
            // {
            //     case SkillUpgradeType.TimeEcho: 
            //         HandleShardRegular(); break;
            //     case SkillUpgradeType.TimeEcho_SingleAttack:
            //         HandleShardRegular(); break;
            //     case SkillUpgradeType.TimeEcho_MultiAttack:
            //         HandleShardMulticast(); break;
            //     case SkillUpgradeType.TimeEcho_ChanceToDuplicate:
            //         HandleShardTeleport(); break;
            //     case SkillUpgradeType.TimeEcho_HealWisp:
            //         HandleShardHpRewind(); break;
            //     case SkillUpgradeType.TimeEcho_CleanseWisp:
            //         HandleShardHpRewind(); break;
            //     case SkillUpgradeType.TimeEcho_CooldownWisp:
            //         HandleShardHpRewind(); break;
            //     default:
            //         Debug.LogError($"Unknown upgrade type {upgradeType}"); break;
            // }
        }
    }
}