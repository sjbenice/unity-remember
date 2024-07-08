using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class QuitScene : MonoBehaviour
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

    public void OnBtnYes()
    {
        Application.Quit();
    }

    public void OnBtnNo()
    {
        GameManager.Goto(null);
    }
}
