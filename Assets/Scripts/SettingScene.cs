using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SettingScene : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;

    // Start is called before the first frame update
    void Start()
    {
        var ret = AudioManager.LoadVolumeSettings();

        musicSlider.value = ret.musicVolume;
        soundSlider.value = ret.sfxVolume;
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

    public void OnBtnSave()
    {
        AudioManager.SaveVolumeSettings(musicSlider.value, soundSlider.value);

        OnBtnCancel();
    }
}
