using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Status
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Player : MonoBehaviour
    {
        private int Level; // level starts form 0
        private float nextLevelExp;
        private float Exp; // Level will be automatically calculated by Exp
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
        public PlayerStat LuckySeven;
        public PlayerStat DiagonalStar;
        public PlayerStat ThrowingStar;
        [Space] 
        public PlayerStat Flamer;
        [Space] 
        public PlayerStat AssassinationTraining;

        public Reward[] rewards;

        private LevelInfo[] _levelTable;
        private readonly Dictionary<string, float> activatedSkills = new();

        private SpriteFlash sprite;
        
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
                reward.Equip(this);
            }

            sprite = GetComponent<SpriteFlash>();
            StartCoroutine(FindNearestObject(), 1.5f);
        }

        public void TakeDamage(float damage)
        {
            Hp.SetValue(Hp.CalculateFinalValue() - damage);
            sprite.Flash();
            Debug.Log($"Took damage: {damage} / currentHp: {Hp}");

            // ReSharper disable once InvertIf
            if (Hp.CalculateFinalValue() <= 0)
            {
                GameManager.Instance.SetIsGameOver(true);
                Debug.Log($"Game Over / currentHp: {Hp}");
            }
        }

        public void SetActivatedSkills()
        {
            var fields = typeof(Player).GetFields().ToList();

            foreach (var field in fields)
            {
                var statName = field.Name;
                // if (statName is "BasicStar" or "LuckySeven" or "DiagonalStar" or "ThrowingStar" or "Flamer")
                if (statName is not ("Level" or "nextLevelExp" or "Exp" or "ExpMultiplier" or "MaxHp" or "Hp" or "AttackDamage" or "Defense" or "AttackSpeed" or "MovementSpeed" or "Cooltime" or "Critical" or "CriticalDamage" or "rewards" or "_levelTable" or "activatedSkills"))
                {
                    var stat = (PlayerStat)field.GetValue(this);

                    if (activatedSkills.ContainsKey(statName))
                    {
                        activatedSkills[statName] = stat.CalculateFinalValue();
                    } 
                    else 
                    {
                        activatedSkills.Add(statName, stat.CalculateFinalValue());
                    }
                }
            }
        }

        public Dictionary<string, float> GetActivatedSkills(bool set)
        {
            if (set) SetActivatedSkills();
            return activatedSkills;
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
            Debug.Log($"LevelUp! to {Level}, currentExp: {Exp}");
            GameManager.Instance.LevelUpEvent();
        }

        public void EarnExp(float exp)
        {
            var calculatedExp = exp * (100 + ExpMultiplier.CalculateFinalValue()) / 100;
            Exp += calculatedExp;

            if (!(Exp >= nextLevelExp)) return;
            LevelUp();
        }
    }
}
