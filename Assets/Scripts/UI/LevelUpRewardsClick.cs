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
        
        levelUpRewards.HideRewards();
    }
}
