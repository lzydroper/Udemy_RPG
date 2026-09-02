using Core.Utils;
using Data;
using Entity;
using Generated;
using SkillSystem;
using Stats;
using UnityEngine;
using VInspector;

namespace DefaultNamespace
{
    public class Tester : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Transform target;
        [SerializeField] private EntityStat entityStat;
        [SerializeField] private EntityVFX entityVFX;
        [SerializeField] private DamageScaleData damageScale;

        #endregion

        // [Button]
        // private void SummonSkillObjShard()
        // {
        //     if (GameManager.Pool.TryRent<SkillObjShard>(Keys.Pool.SkillObjShard, out var shard,
        //             position: target.position))
        //     {
        //         shard.SetupShard(2.5f, entityStat, entityVFX, damageScale);
        //         shard.MoveTowardClosest(3f);
        //     }
        // }
    }
}