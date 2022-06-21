using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdSkills;
using Status;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelUpRewards : MonoBehaviour
{
    [SerializeField] private GameObject[] rewardSlots;
    [SerializeField] private GameObject[] slotFrames;
    [SerializeField] private List<Reward> rewards = new();
    
    private List<Reward> _adSkillRewards = new();
    private List<Reward> _apSkillRewards = new();
    private List<Reward> _subSkillRewards = new();
    private List<Reward> _itemRewards = new();
    private List<Reward> _onlySkillRewards = new();

    private readonly List<Reward> _randomRewards = new();

    private GameObject _framesGrid;
    private GameObject _rewardsGrid;
    private GameObject _textGrid;
    
    private void Start()
    {
        SetSlotSettingsOnGrids();

        Debug.Log($"_rewards count: {rewards.Count}");
        _adSkillRewards = rewards.FindAll(r => r.RewardType.Equals(RewardType.AdSkill));
        _apSkillRewards = rewards.FindAll(r => r.RewardType.Equals(RewardType.ApSkill));
        _subSkillRewards = rewards.FindAll(r => r.RewardType.Equals(RewardType.SubSkill));

        _onlySkillRewards = _adSkillRewards.Concat(_apSkillRewards).Concat(_subSkillRewards).ToList();

        _itemRewards = rewards.FindAll(r => r.RewardType.Equals(RewardType.Item));
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
        SetRewardsOnSlots();
        SetFramesColor();
        
        GameManager.Instance.AllStop();
        
        _framesGrid.SetActive(true);
        _rewardsGrid.SetActive(true);
        _textGrid.SetActive(true);
    }
    
    public void HideRewards()
    {
        GameManager.Instance.Resume();
        
        _randomRewards.Clear();
        _framesGrid.SetActive(false);
        _rewardsGrid.SetActive(false);
        _textGrid.SetActive(false);
        
        GameManager.Instance.post.isGlobal = false;
    }
    
    private void SetRewardsOnSlots()
    {
        var activatedSkills = GameManager.Instance.GetPlayer().GetActivatedSkills();
        var activatedSkillsConvertedToSting = activatedSkills.ConvertAll(s => s.ToString()).ToList();

        for (var i = 0; i < rewardSlots.Length; i++)
        {
            // Set icon
            rewardSlots[i].GetComponent<Image>().sprite = _randomRewards[i].Icon;

            // Set text
            var skillIndex = activatedSkillsConvertedToSting.IndexOf(_randomRewards[i].name);

            _textGrid.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = 
                skillIndex == -1 ? "New" : "Lv " + activatedSkills[skillIndex].CalculateFinalValue();
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
            // ReSharper disable once NotAccessedVariable
            var frame = slotFrames[i].GetComponent<Image>().color; 
            switch (rewardTypes[i])
            {
                case RewardType.AdSkill: frame = new Color32(255, 70, 125, 255); break;
                case RewardType.ApSkill: frame = new Color32(50, 100, 255, 255); break;
                case RewardType.SubSkill: frame = new Color32(255, 144, 255, 255); break;
                case RewardType.Training: frame = new Color32(122, 217, 105, 100); break;
                case RewardType.Item: frame = new Color32(255, 255, 255, 255); break;
                default:
                    throw new ArgumentNullException();
            }
        }
    }
    
    private void SetSlotSettingsOnGrids()
    {
        _framesGrid = transform.GetChild(0).gameObject;
        _rewardsGrid = transform.GetChild(1).gameObject;
        _textGrid = transform.GetChild(2).gameObject;
    }
}
