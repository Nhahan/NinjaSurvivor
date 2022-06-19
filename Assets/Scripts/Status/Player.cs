using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Player player;

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
    public PlayerStat ThrowingStar;

    public Reward[] rewards;

    void Awake()
    {
        foreach (Reward reward in rewards)
        {
            reward.Equip(player);
        }
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

        foreach (string skill in skills)
        {
            Debug.Log(skill);
        }

        return skills;
    }
}
