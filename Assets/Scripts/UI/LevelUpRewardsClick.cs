using Status;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class LevelUpRewardsClick : MonoBehaviour
{
    private Player _player;
    private Sprite _sprite;
    private Sprite _frameSprite;
    private TextMeshProUGUI _rewardText;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        GameManager.Instance.post.profile.TryGetSettings(out Bloom bloom);
        bloom.intensity.value = 5.5f;
    }

    public void GetReward()
    {
        _sprite = GetComponent<Image>().sprite;
        var levelUpRewards = transform.parent.parent.gameObject.GetComponent<LevelUpRewards>();
        var reward = levelUpRewards.GetRandomRewards().Find(r => r.Icon == _sprite);
        
        reward.Equip(_player);
        Debug.Log($"Equipped: {reward.Name} / " +
                  $"BasicStar: {_player.BasicStar.CalculateFinalValue()} / " +
                  $"LuckySeven: {_player.LuckySeven.CalculateFinalValue()} / " +
                  $"DiagonalStar: {_player.DiagonalStar.CalculateFinalValue()} / " +
                  $"ThrowingStar: {_player.ThrowingStar.CalculateFinalValue()}");
        
        levelUpRewards.HideRewards();
    }
}
