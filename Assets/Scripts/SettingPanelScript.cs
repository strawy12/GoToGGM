using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelScript : MonoBehaviour
{
    [SerializeField] private Slider effectSlider = null;
    [SerializeField] private Slider bgmSlider = null;
    [SerializeField] private Toggle effectMute = null;
    [SerializeField] private Toggle bgmMute = null;
    [SerializeField] private Slider FontSlider = null;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void BGMVolume(float value)
    {
        SoundManager.Inst.BGMVolume(value);
    }

    public void BGMMute(bool isMute)
    {
        SoundManager.Inst.BGMMute(isMute);
    }

    public void EffectMute(bool isMute)
    {
        SoundManager.Inst.EffectMute(isMute);
    }

    public void EffectVolume(float value)
    {
        SoundManager.Inst.EffectVolume(value);
    }
    public void SetBGM(int bgmNum)
    {
        SoundManager.Inst.SetBGM(bgmNum);
    }
    public void SetEffectSound(int effectNum)
    {
        SoundManager.Inst.SetEffectSound(effectNum);
    }

    public float GetEffectSoundLength()
    {
        return SoundManager.Inst.GetEffectSoundLength();
    }

    public void SetSettingPanel()
    {
        effectSlider.value = DataManager.Inst.CurrentPlayer.effectVolume;
        bgmSlider.value = DataManager.Inst.CurrentPlayer.bgmVolume;
        FontSlider.value = (float)DataManager.Inst.CurrentPlayer.fontSize / 100f;
        if (DataManager.Inst.CurrentPlayer.bgmMute)
            bgmMute.isOn = true;
        if (DataManager.Inst.CurrentPlayer.effectMute)
            effectMute.isOn = true;
    }
}
