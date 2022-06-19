﻿using UnityEngine;

public enum RewardType
{
    AdSkill,
    ApSkill,
    SubSkill,
    Item,
}

[CreateAssetMenu]
public class Reward : ScriptableObject
{
    public RewardType RewardType;
    public string Name;
    public Sprite Icon;
    [Space]
    public int MaxHpBonus;
    public int HpBonus;
    public int AttackDamageBonus;
    public int DefenseBonus;
    public int AttackSpeedBonus;
    public int MovementSpeedBonus;
    public int CooltimeBonus;
    public int CriticalBonus;
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
    [Space]
    public int BasicStar;
    public int LuckySeven;
    public int ThrowingStar;

    public void Equip(Player p)
    {
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

        if (BasicStar != 0)
            p.BasicStar.AddModifier(new StatModifier(BasicStar, StatModType.Flat, this));
        if (LuckySeven != 0)
            p.LuckySeven.AddModifier(new StatModifier(LuckySeven, StatModType.Flat, this));
        if (ThrowingStar != 0)
            p.ThrowingStar.AddModifier(new StatModifier(ThrowingStar, StatModType.Flat, this));
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
        p.ThrowingStar.RemoveAllModifiersFromSource(this);
    }
}