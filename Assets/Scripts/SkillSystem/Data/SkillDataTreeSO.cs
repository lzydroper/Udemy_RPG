using System;
using UnityEngine;

namespace SkillSystem.Data
{
    public enum ConDirType
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
    }

    [Serializable]
    public class SkillConnection
    {
        public SkillDataSo parent;
        public SkillDataSo child;
        public ConDirType direction;
        public float length;
        public float rotationOffset;
    }

    [CreateAssetMenu(fileName = "SkillDataTreeSO", menuName = "Data/Skill/SkillDataTreeSO", order = 0)]
    public class SkillDataTreeSO : ScriptableObject
    {
        public SkillDataSo[] skillData;
        public SkillConnection[] connections;
    }
}