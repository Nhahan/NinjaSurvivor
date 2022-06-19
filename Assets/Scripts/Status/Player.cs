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
        public int Level = 1;
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

        private void Awake()
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

        public List<string> GetActivatedSkills()
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

        private void SetLevel()
        {
            Level = player.Exp.CalculateFinalValue() switch
            {
                10 => 1,
                20 => 2,
                30 => 3,
                40 => 4,
                50 => 5,
                60 => 6,
                _ => 7
            };
        }
    }
}
