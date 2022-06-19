using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Status;
using UnityEngine;
using UnityEngine.UI;

[SuppressMessage("ReSharper", "CheckNamespace")]
[SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
public class LevelUpRewards : MonoBehaviour
{
    [SerializeField] private GameObject rewardSlot1;
    [SerializeField] private GameObject rewardSlot2;
    [SerializeField] private GameObject rewardSlot3;
    
    private readonly List<Reward> _rewards = new();
    private readonly List<Reward> _adSkillRewards = new();
    private readonly List<Reward> _apSkillRewards = new();
    private readonly List<Reward> _subSkillRewards = new();
    private readonly List<Reward> _onlySkillRewards = new();
    
    private void Start()
    {
        _rewards.AddRange(Resources.LoadAll<Reward>("Rewards"));
        _adSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.AdSkill));
        _apSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.ApSkill));
        _subSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.SubSkill));
        _onlySkillRewards.AddRange(_adSkillRewards);
        _onlySkillRewards.AddRange(_apSkillRewards);
        _onlySkillRewards.AddRange(_subSkillRewards);
    }

    private List<Reward> GetRandomRewards(int count)
    {
        List<Reward> randomRewards = new();
        var total = _rewards.Count;
        for (var i = 0; i < count; i ++){
            randomRewards.Add(_rewards[Random.Range(0, total)]);
        }

        return randomRewards;
    }

    private void SetRewards(List<Reward> rewards)
    {
        rewardSlot1.GetComponent<Image>().sprite = rewards[0].Icon;
        rewardSlot2.GetComponent<Image>().sprite = rewards[1].Icon;
        rewardSlot3.GetComponent<Image>().sprite = rewards[2].Icon;
    }

    public void ShowRewards()
    {
        var randomRewards = GetRandomRewards(3);
        SetRewards(randomRewards);
        GameManager.Instance.AllStop();
    }
}
