using Core.Utils;
using Data;
using Entity;
using Generated;
using SkillSystem;
using SkillSystem.Runtime;
using Stats;
using UnityEngine;
using VInspector;

namespace DefaultNamespace
{
    public class Tester : MonoBehaviour
    {
        #region Variables

        [SerializeField] SkillTimeEcho skillTimeEcho;

        #endregion

        [Button]
        private void TryUseSkill()
        {
            skillTimeEcho.UseSkill();
        }
    }
}