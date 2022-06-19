using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Status
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Player player;

        public PlayerStat Exp; // Level will be automatically calculated by Exp
        public PlayerStat ExpEfficiency; // Earn Exp(100+ExpEfficiency)
        [Space]
        public PlayerStat MaxHp;
        public PlayerStat Hp;
        public PlayerStat AttackDamage;
        public PlayerStat Defense;
        public PlayerStat AttackSpeed;
        public PlayerStat MovementSpeed;
        public PlayerStat Cooltime;
        public PlayerStat Critical;
        public PlayerStat CriticalDamage;
        [Space]
        public PlayerStat BasicStar;
        public PlayerStat LuckySeven; // BasicStar related Upgrade
        public PlayerStat ThrowingStar;

        public Reward[] rewards;

        private LevelList _levelList;

        private void Awake()
        {
            var jsonAsset = Resources.Load<TextAsset>("JSON/level.json");
            _levelList = JsonUtility.FromJson<LevelList>(jsonAsset.text);
        }

        private void Start()
        {
            foreach (var reward in rewards)
            {
                reward.Equip(player);
            }
        }

        private void Update()
        {
            SetLevel();
        }

        public void TakeDamage(float damage)
        {
            Hp.SetValue(Hp.CalculateFinalValue() - damage);
        }

        public IEnumerable<string> GetActivatedSkills()
        {
            List<string> skills = new();
            if (BasicStar.CalculateFinalValue() >= 1) { skills.Add("BasicStar"); }
            if (ThrowingStar.CalculateFinalValue() >= 1) { skills.Add("ThrowingStar"); };

            foreach (var skill in skills)
            {
                Debug.Log(skill);
            }

            return skills;
        }

        [System.Serializable]
        public class Level
        {
            public int value;
            public float exp;
        }
        
        [System.Serializable]
        public class LevelList
        {
            public Level[] levels;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void SetLevel()
        {
            foreach (var level in _levelList.levels)
            {
                Debug.Log(level);
            }
        }
    }
}
