using System;
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
        public float Exp; // Level will be automatically calculated by Exp
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
        public PlayerStat LuckySeven; // BasicStar Recycling;
        public PlayerStat DiagonalStar;
        public PlayerStat ThrowingStar;
        [Space] 
        public PlayerStat AssassinationTraining;

        public Reward[] rewards;

        private LevelInfo[] _levelTable;
        private readonly List<PlayerStat> activatedSkills = new();

        public class TemporalStat
        {
            
        }


        private void Awake()
        {
            var levelTableAsset = Resources.Load<TextAsset>("JSON/level").ToString();
            _levelTable = JsonUtility.FromJson<LevelInfoList>(levelTableAsset).levels;
            SetActivatedSkills();
        }

        private void Start()
        {
            Level = _levelTable[0].value;
            foreach (var reward in rewards)
            {
                reward.Equip(this);
            }
        }

        public void TakeDamage(float damage)
        {
            Hp.SetValue(Hp.CalculateFinalValue() - damage);
            Debug.Log($"Took damage: {damage} / currentHp: {Hp}");

            // ReSharper disable once InvertIf
            if (Hp.CalculateFinalValue() <= 0)
            {
                GameManager.Instance.SetIsGameOver(true);
                Debug.Log($"Game Over / currentHp: {Hp}");
            }
        }

            // List<string> strings = new() { "level", "nextLevelExp", "Exp", "rewards", "_levelTable", "activatedSkills" };
        public void SetActivatedSkills()
        {
            var fields = typeof(Player).GetFields().ToList(); // 클래스의 변수들 가져오기

            foreach (var field in fields)
            {
                Debug.Log(field); // 예를 들어 여기선 Status.PlayerStat ThrowingStar 라고 찍힙니다
                try
                {
                    PlayerStat stat = field.GetValue(typeof(PlayerStat)) as PlayerStat; // 여기서 에러..
            
                    Debug.Log(field);
                    s(stat);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    // 에러 내용
                    // System.ArgumentException:
                    // Field AssassinationTraining defined on type Status.Player
                    // is not a field on the target object which is of type System.RuntimeType.
                    // Parameter name: obj
                }
            }

            void s(PlayerStat stat)
            {
                try { if (stat.CalculateFinalValue() >= 1) activatedSkills.Add((stat)); }
                catch (Exception e )
                {
                    Debug.Log("why");
                    Debug.Log(e);
                }
            }
        }

        public List<PlayerStat> GetActivatedSkills(bool set)
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
