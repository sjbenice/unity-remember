using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Utils.HandleBackButton();
    }

    public void OnBtnQuit()
    {
        GameManager.Goto("Quit");
    }

    public void OnBtnSetting()
    {
        GameManager.Goto("Setting");
    }

    public void OnBtnDifficulty()
    {
        GameManager.Goto("Difficulty");
    }

    public void OnBtnPlay()
    {
        GameManager.Goto("Play");
    }
}
