using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Status
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PlayerStat
    {
        public float BaseValue;

        protected bool isDirty = true;
        protected float lastBaseValue;

        protected float _value;
        public virtual float Value
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (!isDirty && lastBaseValue == BaseValue) return _value;
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
                return _value;
            }
        }

        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        public PlayerStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public PlayerStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
        }

        public void SetValue(float value)
        {
            BaseValue = value;
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            int numRemovals = statModifiers.RemoveAll(mod => mod.Source == source);

            if (numRemovals > 0)
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual void Initialize()
        {
            foreach (var mod in statModifiers)
            {
                statModifiers.Remove(mod);   
            }
        }

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; //if (a.Order == b.Order)
        }

        public virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;

            statModifiers.Sort(CompareModifierOrder);

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue += mod.Value;
                }
            }

            // Workaround for float calculation errors, like displaying 12.00001 instead of 12
            return (float)Math.Round(finalValue, 4);
        }
    }
}

