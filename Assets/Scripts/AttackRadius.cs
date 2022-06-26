using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRadius : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.Instance.AddTarget(col.gameObject);       
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        GameManager.Instance.RemoveTarget(col.gameObject);
    }
}
