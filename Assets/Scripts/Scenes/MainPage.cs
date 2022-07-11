using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum Button
{
    Start,
    Training,
    Settings
}

public class MainPage : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> titleTexts;
    [SerializeField] private List<TextMeshProUGUI> menuTexts;

    private float _liveTIme;

    private void Start()
    {
        foreach (var text in titleTexts.Concat(menuTexts))
        {
            text.color = new Color(0, 0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        _liveTIme += Time.fixedDeltaTime;

        if (_liveTIme > 1.5f) return;
        foreach (var text in titleTexts)
        {
            text.color = new Color(
                text.color.r + Time.fixedDeltaTime, 
                text.color.g + Time.fixedDeltaTime, 
                text.color.b + Time.fixedDeltaTime, 
                text.color.a + Time.fixedDeltaTime);
        }
        
        for (var i = 0; i < menuTexts.Count; i++)
        {
            var text = menuTexts[i];
            
            if (i == 0) 
            {
                text.color = new Color(
                    text.color.r + Time.fixedDeltaTime, 
                    text.color.g + Time.fixedDeltaTime, 
                    text.color.b + Time.fixedDeltaTime, 
                    text.color.a + Time.fixedDeltaTime);
            }
            else
            {
                text.color = new Color(
                    text.color.r + Time.fixedDeltaTime, 
                    text.color.g + Time.fixedDeltaTime, 
                    text.color.b + Time.fixedDeltaTime, 
                    text.color.a + Time.fixedDeltaTime / 20);
            }
        }
    }
}
