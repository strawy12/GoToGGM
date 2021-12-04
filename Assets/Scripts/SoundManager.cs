using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleTon<SoundManager>
{
    private AudioClip[] effectSounds = null;
    private AudioClip[] bgms = null;
    private AudioSource bgmAudio;
    private AudioSource effectAudio;

    bool IsMain
    {
        get
        {
            return SceneManager.GetActiveScene().ToString() == "Main";
        }
    }

    private void Awake()
    {
        SoundManager[] smanagers = FindObjectsOfType<SoundManager>();
        if (smanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        bgmAudio = GetComponent<AudioSource>();
        effectAudio = transform.GetChild(0).GetComponent<AudioSource>();

       bgms = Resources.LoadAll<AudioClip>("Audios/BGMs");
        effectSounds = Resources.LoadAll<AudioClip>("Audios/Effects");
    }

    private void Start()
    {
        VolumeSetting(DataManager.Inst.CurrentPlayer);
        //if (SceneManager.GetActiveScene().name == "Title")
        //{
        //    StartVolumeSetting();
        //}
    }

    public void StartVolumeSetting()
    {
        string SAVE_PATH = Application.persistentDataPath + "/Save";
        string SAVE_FILENAME = "/SaveFile.txt";
        string json = "";
        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            Player player = JsonUtility.FromJson<Player>(json);

            if (player != null)
            {
                VolumeSetting(player);
                return;
            }
        }

        VolumeSetting();
    }

    public void VolumeSetting(Player player)
    {
        bgmAudio.volume = player.bgmVolume;
        effectAudio.volume = player.effectVolume;
        bgmAudio.mute = player.bgmMute;
        effectAudio.mute = player.effectMute;
    }

    public void VolumeSetting()
    {
        bgmAudio.volume = 0.5f;
        effectAudio.volume = 0.5f;
        bgmAudio.mute = false;
        effectAudio.mute = false;
    }

    public void BGMVolume(float value)
    {
        if (bgmAudio == null) return;
        bgmAudio.volume = value;

        if (IsMain)
        {
            DataManager.Inst.CurrentPlayer.bgmVolume = value;
        }
    }

    public void BGMMute(bool isMute)
    {
        bgmAudio.mute = isMute;
    }
    public void EffectMute(bool isMute)
    {
        effectAudio.mute = isMute;
    }

    public void EffectVolume(float value)
    {
        if (effectAudio == null) return;
        effectAudio.volume = value;

        DataManager.Inst.CurrentPlayer.effectVolume = value;
    }
    public void SetBGM(int bgmNum)
    {
        bgmAudio.Stop();
        bgmAudio.clip = bgms[bgmNum];
        bgmAudio.Play();
    }
    public void SetEffectSound(int effectNum)
    {
        effectAudio.Stop();

        effectAudio.clip = effectSounds[effectNum];
        effectAudio.Play();
    }
    public void StopBGM()
    {
        bgmAudio.Stop();
    }

    public float GetEffectSoundLength()
    {
        return effectAudio.clip.length;
    }

}
