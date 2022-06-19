using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Status;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpRewards : MonoBehaviour
{
    [SerializeField] private GameObject rewardSlot1;
    [SerializeField] private GameObject rewardSlot2;
    [SerializeField] private GameObject rewardSlot3;
    [SerializeField] private List<Reward> _rewards = new();
    
    private readonly List<Reward> _adSkillRewards = new();
    private readonly List<Reward> _apSkillRewards = new();
    private readonly List<Reward> _subSkillRewards = new();
    private readonly List<Reward> _onlySkillRewards = new();

    private readonly List<Reward> randomRewards = new();
    
    private void Start()
    {
        Debug.Log($"_rewards count: {_rewards.Count}");
        _adSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.AdSkill));
        _apSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.ApSkill));
        _subSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.SubSkill));
        _onlySkillRewards.AddRange(_adSkillRewards);
        _onlySkillRewards.AddRange(_apSkillRewards);
        _onlySkillRewards.AddRange(_subSkillRewards);
    }

    private void SetRandomRewards(int count)
    {
        var total = _rewards.Count;
        for (var i = 0; i < count; i ++)
        {
            var randomNum = Random.Range(0, total);
            randomRewards.Add(_rewards[randomNum]);
        }
    }

    private void SetRewardsOnSlots(List<Reward> rewards)
    {
        rewardSlot1.GetComponent<Image>().sprite = rewards[0].Icon;
        rewardSlot2.GetComponent<Image>().sprite = rewards[1].Icon;
        rewardSlot3.GetComponent<Image>().sprite = rewards[2].Icon;
    }

    public void ShowRewards()
    {
        SetRandomRewards(3);
        SetRewardsOnSlots(randomRewards);
        GameManager.Instance.AllStop();
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideRewards()
    {
        GameManager.Instance.Resume();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public List<Reward> GetRandomRewards()
    {
        return randomRewards;
    }
}
