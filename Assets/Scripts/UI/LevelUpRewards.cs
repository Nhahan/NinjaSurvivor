using System;
using System.Collections.Generic;
using System.Linq;
using Status;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelUpRewards : MonoBehaviour
{
    [SerializeField] private GameObject[] rewardSlots;
    [SerializeField] private List<Reward> rewards = new();
    [SerializeField] private LevelUpEffect levelUpEffect;
    
    private List<Reward> _adSkillRewards = new();
    private List<Reward> _apSkillRewards = new();
    private List<Reward> _subSkillRewards = new();
    private List<Reward> _itemRewards = new();
    private List<Reward> _onlySkillRewards = new();

    private readonly List<Reward> _randomRewards = new();

    private GameObject _framesGrid;
    private GameObject _rewardsGrid;
    private GameObject _textGrid;
    private GameObject _levelGrid;
    
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

        // StartCoroutine(levelUpEffect.OnLevelUp());
        SetRandomRewards(3);
        SetRewardsOnSlots();
        
        GameManager.Instance.AllStop();
        
        _framesGrid.SetActive(true);
        _rewardsGrid.SetActive(true);
        _textGrid.SetActive(true);
        _levelGrid.SetActive(true);
    }
    
    public void HideRewards()
    {
        GameManager.Instance.Resume();
        
        _randomRewards.Clear();
        _framesGrid.SetActive(false);
        _rewardsGrid.SetActive(false);
        _textGrid.SetActive(false);
        _levelGrid.SetActive(false);
        
        GameManager.Instance.post.isGlobal = false;
    }
    
    private void SetRewardsOnSlots()
    {
        var activatedSkills = GameManager.Instance.GetPlayer().GetActivatedSkills(true);

        for (var i = 0; i < rewardSlots.Length; i++)
        {
            // Set icon
            rewardSlots[i].GetComponent<Image>().sprite = _randomRewards[i].Icon;

            // Set text
            var skillLevel = activatedSkills[_randomRewards[i].name];
            var newTmp = _textGrid.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            var levelTmp = _levelGrid.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if (skillLevel >= 1)
            {
                levelTmp.enabled = true;
                newTmp.enabled = false;
                levelTmp.text = "Lv " + skillLevel;
            }
            else
            {
                levelTmp.enabled = false;
                newTmp.enabled = true;
            }

            // Set frame
            var rewardType = _randomRewards[i].RewardType;
            switch (rewardType)
            {
                case RewardType.AdSkill: _framesGrid.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 70, 125, 255); break;
                case RewardType.ApSkill: _framesGrid.transform.GetChild(i).GetComponent<Image>().color = new Color32(50, 100, 255, 255); break;
                case RewardType.SubSkill: _framesGrid.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 144, 255, 255); break;
                case RewardType.Training: _framesGrid.transform.GetChild(i).GetComponent<Image>().color = new Color32(122, 217, 105, 100); break;
                case RewardType.Item: _framesGrid.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255); break;
                default:
                    throw new ArgumentNullException();
            }
        }
    }
    
    public List<Reward> GetRandomRewards()
    {
        return _randomRewards;
    }

    private void SetSlotSettingsOnGrids()
    {
        _framesGrid = transform.GetChild(0).gameObject;
        _rewardsGrid = transform.GetChild(1).gameObject;
        _textGrid = transform.GetChild(2).gameObject;
        _levelGrid = transform.GetChild(3).gameObject;
    }
}
