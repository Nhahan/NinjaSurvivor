using System;
using System.Collections.Generic;
using System.Linq;
using Status;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelUpRewards : MonoBehaviour
{
    [SerializeField] private GameObject[] rewardSlots;
    [SerializeField] private GameObject[] slotFrames;
    [SerializeField] private List<Reward> rewards = new();
    
    private readonly List<Reward> _adSkillRewards = new();
    private readonly List<Reward> _apSkillRewards = new();
    private readonly List<Reward> _subSkillRewards = new();
    private readonly List<Reward> _onlySkillRewards = new();

    private readonly List<Reward> _randomRewards = new();

    private GameObject _framesGrid;
    private GameObject _rewardsGrid;
    
    private void Start()
    {
        _framesGrid = transform.GetChild(0).gameObject;
        _rewardsGrid = transform.GetChild(1).gameObject;
        
        Debug.Log($"_rewards count: {rewards.Count}");
        _adSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.AdSkill));
        _apSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.ApSkill));
        _subSkillRewards.FindAll(r => r.RewardType.Equals(RewardType.SubSkill));
        _onlySkillRewards.AddRange(_adSkillRewards);
        _onlySkillRewards.AddRange(_apSkillRewards);
        _onlySkillRewards.AddRange(_subSkillRewards);
    }

    private void SetRandomRewards(int count)
    {
        var total = rewards.Count;
        for (var i = 0; i < count; i ++)
        {
            var randomNum = Random.Range(0, total);
            _randomRewards.Add(rewards[randomNum]);
        }
    }
    
    public void ShowRewards()
    {
        GameManager.Instance.post.isGlobal = true;
        SetRandomRewards(3);
        SetRewardsOnSlots(_randomRewards);
        GameManager.Instance.AllStop();
        _framesGrid.SetActive(true);
        _rewardsGrid.SetActive(true);
    }
    
    public void HideRewards()
    {
        GameManager.Instance.Resume();
        _randomRewards.Clear();
        _framesGrid.SetActive(false);
        _rewardsGrid.SetActive(false);
        GameManager.Instance.post.isGlobal = false;
    }
    
    private void SetRewardsOnSlots(List<Reward> rewardList)
    {
        for (var i = 0; i < rewardSlots.Length; i++)
        {
            rewardSlots[i].GetComponent<Image>().sprite = rewardList[i].Icon;
        }
    }

    public List<Reward> GetRandomRewards()
    {
        return _randomRewards;
    }

    public void SetFramesColor()
    {
        var rewardTypes = _randomRewards.Select(reward => reward.RewardType).ToList();
        for (var i = 0; i < rewardTypes.Count; i++)
        {
            var frame = slotFrames[i].GetComponent<Image>().color; 
            switch (rewardTypes[i])
            {
                case RewardType.AdSkill: frame = new Color32(1, 1, 1, 100); break;
                case RewardType.ApSkill: frame = new Color32(1, 1, 1, 100); break;
                case RewardType.SubSkill: frame = new Color32(1, 1, 1, 100); break;
                case RewardType.Training: frame = new Color32(1, 1, 1, 100); break;
                case RewardType.Item: frame = new Color32(1, 1, 1, 100); break;
                default:
                    throw new ArgumentNullException();
            }
        }
    }
}
