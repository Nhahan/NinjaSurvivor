using Status;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class LevelUpRewardsClick : MonoBehaviour
{
    [SerializeField] private Player player;
    private Sprite _sprite;
    private Bloom _bloom;
    private bool _isGoingUp = true;

    private void Start()
    {
        GameManager.Instance.post.profile.TryGetSettings(out _bloom);
        _bloom.intensity.value = 4.5f;
        _bloom.anamorphicRatio.value = -1;
    }

    private void LateUpdate()
    {
        _isGoingUp = _bloom.intensity.value switch
        {
            < 4.6f => true,
            > 7.2f => false,
            _ => _isGoingUp
        };

        switch (_isGoingUp)
        {
            case true:
                _bloom.intensity.value = Mathf.Lerp(_bloom.intensity.value, 7.3f, 1.5f * Time.deltaTime);
                _bloom.anamorphicRatio.value = Mathf.Lerp(_bloom.anamorphicRatio.value, 0.5f, 0.3f * Time.deltaTime);
                break;
            case false:
                _bloom.intensity.value = Mathf.Lerp(_bloom.intensity.value, 4.5f, 1f * Time.deltaTime);
                _bloom.anamorphicRatio.value = Mathf.Lerp(_bloom.anamorphicRatio.value, -1f, 0.3f * Time.deltaTime);
                break;
        }
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
