using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Status
{
    public enum RewardType
    {
        AdSkill,
        ApSkill,
        SubSkill,
        Training,
        Item,
    }

    [CreateAssetMenu]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Reward : ScriptableObject
    {
        public RewardType RewardType;
        public string Name;
        public Sprite Icon;
        public int MaxCount;
        [Space]
        public int MaxHpBonus;
        public int HpBonus;
        public int AttackDamageBonus;
        public int DefenseBonus;
        public int AttackSpeedBonus;
        public int MovementSpeedBonus;
        public int CooltimeBonus;
        public int CriticalBonus;
        public int CriticalDamageBonus;
        [Space]
        public float MaxHpPercentBonus;
        public float HpPercentBonus;
        public float AttackDamagePercentBonus;
        public float DefensePercentBonus;
        public float AttackSpeedPercentBonus;
        public float MovementSpeedPercentBonus;
        public float CooltimePercentBonus;
        public float CriticalPercentBonus;
        public float CriticalDamagePercentBonus;
        [Space] // AdSkill
        public int BasicStar;
        public int LuckySeven;
        public int DiagonalStar;
        public int ThrowingStar;
        public int Slash;
        public int Gyeok;
        [Space] 
        public int Flamer;
        public int ExplosiveShuriken;
        public int LightningStrike;
        public int FireCross;
        [Space] 
        public int AssassinationTraining; // 공격속도 5% 증가
        public int BodycoreTraining; // 방어력 5% 증가
        public int FootworkTraining; // 이동속도 5% 증가
        public int MuscleTraining; // 공격력 1 증가
        public int PhysicalTraining; // 방어력 1 증가

        [Space] 
        public string memo;

        public void Equip(Player p)
        {
            // simple plus minus
            if (MaxHpBonus != 0)
                p.MaxHp.AddModifier(new StatModifier(MaxHpBonus, StatModType.Flat, this));
            if (HpBonus != 0)
                p.Hp.AddModifier(new StatModifier(HpBonus, StatModType.Flat, this));
            if (AttackDamageBonus != 0)
                p.AttackDamage.AddModifier(new StatModifier(AttackDamageBonus, StatModType.Flat, this));
            if (DefenseBonus != 0)
                p.Defense.AddModifier(new StatModifier(DefenseBonus, StatModType.Flat, this));
            if (AttackSpeedBonus != 0)
                p.AttackSpeed.AddModifier(new StatModifier(AttackSpeedBonus, StatModType.Flat, this));
            if (MovementSpeedBonus != 0)
                p.MovementSpeed.AddModifier(new StatModifier(MovementSpeedBonus, StatModType.Flat, this));
            if (CooltimeBonus != 0)
                p.Cooltime.AddModifier(new StatModifier(CooltimeBonus, StatModType.Flat, this));
            if (CriticalBonus != 0)
                p.Critical.AddModifier(new StatModifier(CriticalBonus, StatModType.Flat, this));
            if (CriticalDamageBonus != 0)
                p.Critical.AddModifier(new StatModifier(CriticalBonus, StatModType.Flat, this));

            // percentage
            if (MaxHpPercentBonus != 0)
                p.MaxHp.AddModifier(new StatModifier(MaxHpPercentBonus, StatModType.PercentMult, this));
            if (HpPercentBonus != 0)
                p.Hp.AddModifier(new StatModifier(HpPercentBonus, StatModType.PercentMult, this));
            if (AttackDamagePercentBonus != 0)
                p.AttackDamage.AddModifier(new StatModifier(AttackDamagePercentBonus, StatModType.PercentMult, this));
            if (DefensePercentBonus != 0)
                p.Defense.AddModifier(new StatModifier(DefensePercentBonus, StatModType.PercentMult, this));
            if (AttackSpeedPercentBonus != 0)
                p.AttackSpeed.AddModifier(new StatModifier(AttackSpeedPercentBonus, StatModType.PercentMult, this));
            if (MovementSpeedPercentBonus != 0)
                p.MovementSpeed.AddModifier(new StatModifier(MovementSpeedPercentBonus, StatModType.PercentMult, this));
            if (CooltimePercentBonus != 0)
                p.Cooltime.AddModifier(new StatModifier(CooltimePercentBonus, StatModType.PercentMult, this));
            if (CriticalPercentBonus != 0)
                p.Critical.AddModifier(new StatModifier(CriticalPercentBonus, StatModType.PercentMult, this));
            if (CriticalDamagePercentBonus != 0)
                p.CriticalDamage.AddModifier(new StatModifier(CriticalDamagePercentBonus, StatModType.PercentMult, this));

            // adSkills
            if (BasicStar != 0)
                p.BasicStar.AddModifier(new StatModifier(BasicStar, StatModType.Flat, this));
            if (LuckySeven != 0)
                p.LuckySeven.AddModifier(new StatModifier(LuckySeven, StatModType.Flat, this));
            if (DiagonalStar != 0)
                p.DiagonalStar.AddModifier(new StatModifier(DiagonalStar, StatModType.Flat, this));
            if (ThrowingStar != 0)
                p.ThrowingStar.AddModifier(new StatModifier(ThrowingStar, StatModType.Flat, this));            
            if (Slash != 0)
                p.Slash.AddModifier(new StatModifier(Slash, StatModType.Flat, this));            
            if (Gyeok != 0)
                p.Gyeok.AddModifier(new StatModifier(Gyeok, StatModType.Flat, this));
            
            // apSkills
            if (Flamer != 0)
                p.Flamer.AddModifier(new StatModifier(Flamer, StatModType.Flat, this));
            if (ExplosiveShuriken != 0)
                p.ExplosiveShuriken.AddModifier(new StatModifier(ExplosiveShuriken, StatModType.Flat, this));
            if (LightningStrike != 0)
                p.LightningStrike.AddModifier(new StatModifier(LightningStrike, StatModType.Flat, this));
            if (FireCross != 0)
                p.FireCross.AddModifier(new StatModifier(FireCross, StatModType.Flat, this));
            
            // Training
            if (AssassinationTraining != 0)
                p.AssassinationTraining.AddModifier(new StatModifier(AssassinationTraining, StatModType.Flat, this));
            if (BodycoreTraining != 0)
                p.AssassinationTraining.AddModifier(new StatModifier(AssassinationTraining, StatModType.Flat, this));
            if (FootworkTraining != 0)
                p.AssassinationTraining.AddModifier(new StatModifier(AssassinationTraining, StatModType.Flat, this));
            if (MuscleTraining != 0)
                p.AssassinationTraining.AddModifier(new StatModifier(AssassinationTraining, StatModType.Flat, this));
            if (PhysicalTraining != 0)
                p.AssassinationTraining.AddModifier(new StatModifier(AssassinationTraining, StatModType.Flat, this));
        }

        public void Unequip(Player p)
        {
            p.MaxHp.RemoveAllModifiersFromSource(this);
            p.Hp.RemoveAllModifiersFromSource(this);
            p.AttackDamage.RemoveAllModifiersFromSource(this);
            p.Defense.RemoveAllModifiersFromSource(this);
            p.AttackSpeed.RemoveAllModifiersFromSource(this);
            p.MovementSpeed.RemoveAllModifiersFromSource(this);
            p.Cooltime.RemoveAllModifiersFromSource(this);
            p.Critical.RemoveAllModifiersFromSource(this);
            p.CriticalDamage.RemoveAllModifiersFromSource(this);

            p.BasicStar.RemoveAllModifiersFromSource(this);
            p.LuckySeven.RemoveAllModifiersFromSource(this);
            p.DiagonalStar.RemoveAllModifiersFromSource(this);
            p.ThrowingStar.RemoveAllModifiersFromSource(this);            
            p.Slash.RemoveAllModifiersFromSource(this);
            p.Gyeok.RemoveAllModifiersFromSource(this);
            
            p.Flamer.RemoveAllModifiersFromSource(this);
            p.ExplosiveShuriken.RemoveAllModifiersFromSource(this);
            p.LightningStrike.RemoveAllModifiersFromSource(this);
            p.FireCross.RemoveAllModifiersFromSource(this);
            
            p.AssassinationTraining.RemoveAllModifiersFromSource(this);
            p.BodycoreTraining.RemoveAllModifiersFromSource(this);
            p.FootworkTraining.RemoveAllModifiersFromSource(this);
            p.MuscleTraining.RemoveAllModifiersFromSource(this);
            p.PhysicalTraining.RemoveAllModifiersFromSource(this);
        }
    }
}