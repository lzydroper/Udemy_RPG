using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillDash : SkillBase
    {
        public void OnStartEffect()
        {
            if (upgradeType is SkillUpgradeType.Dash_WithEcho or SkillUpgradeType.Dash_WithEchoAndArrival)
                CreateClone();
        
            if (upgradeType is SkillUpgradeType.Dash_WithShard or SkillUpgradeType.Dash_WithShardAndArrival)
                CreateShard();
        }

        public void OnEndEffect()
        {
            if (upgradeType is SkillUpgradeType.Dash_WithEchoAndArrival)
                CreateClone();
        
            if (upgradeType is SkillUpgradeType.Dash_WithShardAndArrival)
                CreateShard();
        }

        private void CreateShard()
        {
            var skill = skillManager.GetSkillByType(SkillType.TimeShard) as SkillTimeShard;
            if (skill != null)
                skill.SummonShard();
        }

        private void CreateClone()
        {
            Debug.Log("Create time echo!;");
            // TODO: skill manager clone create clone
        }
    }
}