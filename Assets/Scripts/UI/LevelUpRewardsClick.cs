using Status;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpRewardsClick : MonoBehaviour
{
    [SerializeField] private Player player;
    private Sprite _sprite;

    public void GetReward()
    {
        _sprite = GetComponent<Image>().sprite;
        var levelUpRewards = transform.parent.parent.gameObject.GetComponent<LevelUpRewards>();
        var reward = levelUpRewards.GetRandomRewards().Find(r => r.Icon == _sprite);
        
        reward.Equip(player);
        
        levelUpRewards.HideRewards();
    }
}
