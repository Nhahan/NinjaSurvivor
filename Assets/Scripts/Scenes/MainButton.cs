using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    private MainPage _mainPage;
    
    public Button currentButton;

    private bool _isChangeScene;

    private void Start()
    {
        _mainPage = transform.parent.parent.GetComponent<MainPage>();
    }

    public void FixedUpdate()
    {
        if (_isChangeScene)
        {
            _mainPage.Dim();
        }
    }

    public void OnBtnClick()
    {
        switch (currentButton)
        {
            case Button.Start:
                _isChangeScene = true;
                StartCoroutine(ChangeScene("Stage1"));
                break;
            case Button.Training:
                Debug.Log("Training");
                break;
            case Button.Settings:
                Debug.Log("Settings");
                break;
        }
    }

    private IEnumerator ChangeScene(string scene)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(scene);
    }
}
