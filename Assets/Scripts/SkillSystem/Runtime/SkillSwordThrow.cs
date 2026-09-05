using UnityEngine;
using VInspector;

namespace SkillSystem.Runtime
{
    public class SkillSwordThrow : SkillBase
    {
        #region Variables

        private SkillObjSword sword;
        private float currentThrowPower => upgradeType switch
        {
            SkillUpgradeType.SwordThrow => regularThrowPower,
            SkillUpgradeType.SwordThrow_Pierce => pierceThrowPower,
            SkillUpgradeType.SwordThrow_Spin => spinThrowPower,
            SkillUpgradeType.SwordThrow_Bounce => bounceThrowPower,
            _ => throw new System.NotImplementedException(),
        };
        
        [Header("Trajectory Prediction")]
        [SerializeField] private GameObject predictionDotPrefab;
        [SerializeField] private int numOfDots = 20;
        [SerializeField] private float spaceOfDots = 0.05f;
        [SerializeField] private float gravity = 3.5f;
        private Transform[] predictionDots;
        private Transform predictionDotParent;
        private Vector2 confirmedDirection;
        
        [Tab("Regular Sword Upgrade")]
        [Header("Regular Sword Upgrade")]
        [SerializeField] private GameObject regularSwordPrefab;
        [SerializeField] private float regularThrowPower = 5f;
        [EndTab]

        [Tab("Pierce Sword Upgrade")]
        [Header("Pierce Sword Upgrade")]
        [SerializeField] private GameObject pierceSwordPrefab;
        public int amountToPierce = 2;
        [SerializeField] private float pierceThrowPower = 5;
        [EndTab]

        [Tab("Spin Sword Upgrade")]
        [Header("Spin Sword Upgrade")]
        [SerializeField] private GameObject spinSwordPrefab;
        public int maxDistance = 5;
        public float attacksPerSecond = 6;
        public float maxSpinDuration = 3;
        [SerializeField] private float spinThrowPower = 5;
        [EndTab]

        [Tab("Bounce Sword Upgrade")]
        [Header("Bounce Sword Upgrade")]
        [SerializeField] private GameObject bounceSwordPrefab;
        public int bounceCount = 5;
        public float bounceSpeed = 12;
        [SerializeField] private float bounceThrowPower = 5;
        [EndTab]

        #endregion

        #region LifecycleMethods

        protected override void Awake()
        {
            base.Awake();
            GeneratePredictionDots();
            EnableDots(false);
        }

        #endregion

        public override bool IsCanUseSkill()
        {
            if (sword != null)
            {
                sword.GetSwordBackToPlayer();
                return false;
            }
            return base.IsCanUseSkill();
        }

        public void ThrowSword()
        {
            SetupSword();
            SetOnCooldown();
        }
        
        private void SetupSword()
        {
            GameObject prefab = upgradeType switch
            {
                SkillUpgradeType.SwordThrow => regularSwordPrefab,
                SkillUpgradeType.SwordThrow_Pierce => pierceSwordPrefab,
                SkillUpgradeType.SwordThrow_Spin => spinSwordPrefab,
                SkillUpgradeType.SwordThrow_Bounce => bounceSwordPrefab,
                _ => throw new System.NotImplementedException(),
            };
            var go = Instantiate(prefab, predictionDots[1].position, Quaternion.identity);
            var velocity = confirmedDirection * (currentThrowPower * 10);
            sword = go.GetComponent<SkillObjSword>();
            sword.SetupSword(this, velocity);
        }
        
        #region Trajectory
        
        public void ConfirmedDirection(Vector2 direction) => confirmedDirection = direction;

        public void EnableDots(bool enable)
        {
            predictionDotParent.gameObject.SetActive(enable);
        }

        public void PredictTrajectory(Vector3 direction)
        {
            for (int i = 0; i < numOfDots; i++)
            {
                predictionDots[i].position = GetTrajectory(direction, i * spaceOfDots);
            }
        }

        private Vector3 GetTrajectory(Vector3 direction, float t)
        {
            Vector3 initialVelocity = direction * (currentThrowPower * 10);
            Vector3 gravityEffect = Physics2D.gravity * (0.5f * gravity * t * t);
            return skillManager.player.transform.position + initialVelocity * t + gravityEffect;
        }

        private void GeneratePredictionDots()
        {
            predictionDotParent = new GameObject("PredictionDots").transform;
            predictionDots = new Transform[numOfDots];

            for (int i = 0; i < numOfDots; i++)
            {
                predictionDots[i] =  Instantiate(predictionDotPrefab, predictionDotParent).transform;
            }
        }
        
        #endregion
    }
}