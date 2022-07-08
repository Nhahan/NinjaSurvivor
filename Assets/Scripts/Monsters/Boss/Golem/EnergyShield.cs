using System.Collections;
using System.Collections.Generic;
using Monsters;
using Monsters.Boss;
using Status;
using UnityEngine;

public class EnergyShield : Monster, IMonster
{
    private Player _player;
    private Rigidbody2D _rb;
    private GameObject _golem;
    
    private float _monsterHp = 150f;
    private float _monsterDefense = 0;

    private Material _defaultMaterial;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();

        _defaultMaterial = GetComponent<SpriteRenderer>().material;
        
        Destroy(gameObject, 4.25f);
    }

    private void FixedUpdate()
    {
        transform.position = _golem.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;
    
        var damage = _player.MaxHp.CalculateFinalValue() * 0.2f - _player.Defense.CalculateFinalValue();
        Flash();
        
        _player.TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    { 
        _monsterHp = _monsterHp - damage + _monsterDefense;
        Flash();

        if (_monsterHp > 0) return;
        _golem.GetComponent<Golem>().SetIsShieldOnToFalse();
        Destroy(gameObject, 0.05f);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected override IEnumerator FlashRoutine()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.material = GameManager.Instance.GetFlashMaterial();
        yield return new WaitForSeconds(Duration);

        SpriteRenderer.material = _defaultMaterial;
        _flashRoutine = null;
    }

    public void SetGolem(GameObject golem, bool isAngry)
    {
        _golem = golem;
        if (isAngry)
        {
            GetComponent<SpriteRenderer>().color = new Color(200f, 0f, 0f, 31f);
            _monsterHp *= 2f;
            Debug.Log(GetComponent<SpriteRenderer>().color + "/ " + _monsterHp);
        }
    }
}
