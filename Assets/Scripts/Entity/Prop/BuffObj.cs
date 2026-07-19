using System;
using System.Collections;
using Stats;
using UnityEngine;

namespace Entity.Prop
{
    [Serializable]
    public class Buff
    {
        public StatType type;
        public float value;
    }
    
    public class BuffObj : MonoBehaviour
    {
        #region Variables

        private SpriteRenderer sr;
        
        [Header("Buff Settings")] 
        [SerializeField] private Buff[] buffs;
        [SerializeField] private string buffName;
        [SerializeField] private float buffDuration = 4f;
        [SerializeField] private bool canBeUsed = true;

        [Header("Float Move Detail")]
        [SerializeField] private float floatSpeed = 1f;
        [SerializeField] private float floatRange = .2f;
        private Vector2 oriPosition;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            oriPosition = transform.position;
        }

        private void Update()
        {
            if (canBeUsed)
            {
                var position = new Vector2(oriPosition.x,
                    oriPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatRange);
                transform.position = position;
            }
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D player)
        {
            if (!canBeUsed) return;
            EntityStat stat = player.GetComponent<EntityStat>();
            StartCoroutine(BuffCo(buffDuration, stat));
        }

        private IEnumerator BuffCo(float duration, EntityStat stat)
        {
            canBeUsed = false;
            sr.color = Color.clear;
            ApplyAllBuffs(stat, true);
            yield return new WaitForSeconds(duration);
            ApplyAllBuffs(stat, false);
            // return to pool or just destroy ?
            Destroy(gameObject);
        }
        
        private void ApplyAllBuffs(EntityStat stat, bool enable)
        {
            foreach (var buff in buffs)
            {
                if (enable)
                    stat.GetStatByType(buff.type).AddModifier(GetInstanceID(), buffName, buff.value);
                else
                    stat.GetStatByType(buff.type).RemoveModifier(GetInstanceID());
            }
        }
    }
}