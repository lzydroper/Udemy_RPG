using Core.Utils;
using Generated;
using Skills;
using UnityEngine;
using VInspector;

namespace DefaultNamespace
{
    public class Tester : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Transform target;

        #endregion

        [Button]
        private void SummonSkillObjShard()
        {
            if (GameManager.Pool.TryRent<SkillObjShard>(Keys.Pool.SkillObjShard, out var shard,
                    position: target.position))
            {
                shard.SetupShard(2.5f);
            }
        }
    }
}