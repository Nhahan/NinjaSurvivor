using Status;
using TMPro;
using UnityEngine;

public class ProgressExp : MonoBehaviour
{
    [SerializeField] private GameObject levelTextObject;
    
    private Player _player;
    private float _maxExp;
    private float _currentExp;
    private RectTransform _x;
    private float _y;
    private float _z;
    private TextMeshProUGUI _levelText;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _maxExp = _player.nextLevelExp - _player.previousExp;
        _currentExp = _player.Exp - _player.previousExp;
        _x = GetComponent<RectTransform>();
        _y = GetComponent<RectTransform>().localScale.y;
        _z = GetComponent<RectTransform>().localScale.z;
        _levelText = levelTextObject.GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        _levelText.text = "Level " + _player.Level;
        _maxExp = _player.nextLevelExp - _player.previousExp;
        _currentExp = _player.Exp - _player.previousExp;
        _x.localScale = _player.Level == 1 ? new Vector3(0, _y, _z) : new Vector3(_currentExp / _maxExp, _y, _z);
    }
}
