using System.Linq;
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

    private void Awake()
    {
        foreach (Reward reward in rewards)
        {
            reward.Equip(player);
        }
    }
}
