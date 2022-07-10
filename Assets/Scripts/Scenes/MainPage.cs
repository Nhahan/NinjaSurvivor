using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainPage : MonoBehaviour
{
    private List<TextMeshProUGUI> _titleTexts;
    private List<TextMeshProUGUI> _menuTexts;

    private void Start()
    {
        _titleTexts = transform.GetComponentsInChildren<Transform>().Select(child => child.GetComponent<TextMeshProUGUI>()).ToList();
        _menuTexts = transform.GetComponentsInChildren<Transform>().Select(child => child.GetComponent<TextMeshProUGUI>()).ToList();
    }
}
