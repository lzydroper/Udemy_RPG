using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "SkillDataSO", menuName = "Data/Skill/SkillDataSO", order = 0)]
    public class SkillDataSo : ScriptableObject
    {
        [Header("Skill Description")]
        public string Name;
        [TextArea]
        public string Description;
        public Sprite Icon;
    }
}