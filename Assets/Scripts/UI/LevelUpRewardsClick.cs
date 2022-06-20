using Status;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class LevelUpRewardsClick : MonoBehaviour
{
    [SerializeField] private Player player;
    private Sprite _sprite;
    private Sprite _frameSprite;

    private void Start()
    {
        GameManager.Instance.post.profile.TryGetSettings(out Bloom bloom);
        bloom.intensity.value = 4.5f;
    }

    public void GetReward()
    {
        _sprite = GetComponent<Image>().sprite;
        var levelUpRewards = transform.parent.parent.gameObject.GetComponent<LevelUpRewards>();
        var reward = levelUpRewards.GetRandomRewards().Find(r => r.Icon == _sprite);
        
        reward.Equip(player);
        Debug.Log($"Equipped: {reward.Name} / " +
                  $"BasicStar: {player.BasicStar.CalculateFinalValue()} / " +
                  $"LuckySeven: {player.LuckySeven.CalculateFinalValue()} / " +
                  $"DiagonalStar: {player.DiagonalStar.CalculateFinalValue()} / " +
                  $"ThrowingStar: {player.ThrowingStar.CalculateFinalValue()}");
        
        levelUpRewards.HideRewards();
    }
}
