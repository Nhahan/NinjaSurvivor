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

        private int Level; // level starts form 0
        private float nextLevelExp;
        public PlayerStat Exp; // Level will be automatically calculated by Exp
        public PlayerStat ExpMultiplier; // Earn Exp(100+ExpMultiplier)
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

        private LevelInfo[] _levelTable;
        

        private void Awake()
        {
            var levelTableAsset = Resources.Load<TextAsset>("JSON/level").ToString();
            _levelTable = JsonUtility.FromJson<LevelInfoList>(levelTableAsset).levels;
        }

        private void Start()
        {
            Level = _levelTable[0].value;
            foreach (var reward in rewards)
            {
                reward.Equip(player);
            }
        }

        private void Update()
        {
            
        }

        public void TakeDamage(float damage)
        {
            Hp.SetValue(Hp.CalculateFinalValue() - damage);
            Debug.Log($"Took damage: {damage} / currentHp: {Hp}");
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
        public class LevelInfo
        {
            public int value;
            public float exp;
        }
        
        [System.Serializable]
        public class LevelInfoList
        {
            public LevelInfo[] levels;
        }
        
        private void LevelUp()
        {
            nextLevelExp = _levelTable[Level].exp;
            Level += 1;
            Debug.Log($"LevelUp! to {Level}");
            GameManager.Instance.LevelUpEvent();
        }

        public void EarnExp(float exp)
        {
            var calculatedExp = exp * (100 + ExpMultiplier.CalculateFinalValue()) / 100;
            Debug.Log($"GetExp: {calculatedExp}");
            Exp.SetValue(Exp.CalculateFinalValue() + calculatedExp);
            
            if (!(Exp.CalculateFinalValue() >= nextLevelExp)) return;
            LevelUp();
        }
    }
}
