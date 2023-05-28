using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private MusicTheme _musicTheme;

    [SerializeField] private AudioSource _baseMusic;
    [SerializeField] private AudioSource _blueMusicLayer;
    [SerializeField] private AudioSource _orangeMusicLayer;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _baseMusic.clip = _menuMusic;
        _baseMusic.PlayScheduled(AudioSettings.dspTime);
        GameManager.Instance.onGameStarted += PlayMusicTheme;
    }

    public void SetVolume(float amount)
    {
        _baseMusic.volume = amount;
        _blueMusicLayer.volume = amount;
        _orangeMusicLayer.volume = amount;
    }

    public void PlayMusicTheme(bool gameStart)
    {
        if (!gameStart) return;
        
        _baseMusic.clip = _musicTheme.MusicBase;
        _blueMusicLayer.clip = _musicTheme.BlueLayer;
        _orangeMusicLayer.clip = _musicTheme.OrangeLayer;

        _baseMusic.PlayScheduled(AudioSettings.dspTime);
        _blueMusicLayer.PlayScheduled(AudioSettings.dspTime);
        _orangeMusicLayer.PlayScheduled(AudioSettings.dspTime);

        _blueMusicLayer.mute = true;
        _orangeMusicLayer.mute = true;


    }

    public void PlayOrangeLayer()
    {
        _orangeMusicLayer.mute = false;
    }

    public void PlayerBlueLayer()
    {
        _blueMusicLayer.mute = false;
    }
}