using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEffect : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // public IEnumerator OnLevelUp()
    public void OnLevelUp()
    {
        // _animator.SetBool("isLeveling", true);
        // yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        // _animator.SetBool("isLeveling", false);
    }
}
