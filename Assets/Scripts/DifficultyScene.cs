using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class DifficultyScene : MonoBehaviour
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

    public void OnBtnCancel()
    {
        GameManager.Goto(null);
    }

    public void OnBtnEasy()
    {
        DoPlay(0);
    }

    public void OnBtnMiddle()
    {
        DoPlay(1);
    }

    public void OnBtnHard()
    {
        DoPlay(2);
    }

    public void DoPlay(int level)
    {
        GameManager.level = level;

        GameManager.Goto("Play");
    }
}
